using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Models.Common;
using Microsoft.Extensions.Logging;

namespace AllegroApi.Http;

/// <summary>
/// Base HTTP client for making requests to Allegro API
/// </summary>
public class AllegroHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly AllegroApiOptions _options;
    private readonly ILogger<AllegroHttpClient>? _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AllegroHttpClient(HttpClient httpClient, AllegroApiOptions options, ILogger<AllegroHttpClient>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;

        _options.Validate();

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.allegro.public.v1+json"));
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
        
        if (!string.IsNullOrEmpty(_options.AccessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);
        }

        if (!string.IsNullOrEmpty(_options.AcceptLanguage))
        {
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(_options.AcceptLanguage);
        }
    }

    /// <summary>
    /// Perform GET request
    /// </summary>
    public async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("GET", url);
            var response = await _httpClient.GetAsync(url, cancellationToken);
            return await HandleResponseAsync<T>(response);
        }, cancellationToken);
    }

    /// <summary>
    /// Perform GET request and return raw HTTP response (for binary files).
    /// </summary>
    public async Task<HttpResponseMessage> GetRawAsync(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("GET", url);
            var response = await _httpClient.GetAsync(url, cancellationToken);
            // Check for errors but don't deserialize - return raw response for binary content
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                await ThrowAppropriateException(response, content);
            }
            return response;
        }, cancellationToken);
    }

    /// <summary>
    /// Perform POST request
    /// </summary>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint, 
        TRequest data, 
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("POST", url, data);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            return await HandleResponseAsync<TResponse>(response);
        }, cancellationToken);
    }

    /// <summary>
    /// Perform POST request and return raw HttpResponseMessage (useful for obtaining headers like Location).
    /// </summary>
    public async Task<HttpResponseMessage> PostRawAsync<TRequest>(
        string endpoint,
        TRequest data,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        LogRequest("POST", url, data);
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var contentStr = await response.Content.ReadAsStringAsync();
            await ThrowAppropriateException(response, contentStr);
        }
        return response;
    }

    /// <summary>
    /// Perform POST request without response body
    /// </summary>
    public async Task PostAsync<TRequest>(
        string endpoint,
        TRequest data,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("POST", url, data);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            await HandleResponseAsync(response);
            return true;
        }, cancellationToken);
    }

    /// <summary>
    /// Perform PUT request
    /// </summary>
    public async Task<TResponse> PutAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("PUT", url, data);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
            var response = await _httpClient.PutAsync(url, content, cancellationToken);
            return await HandleResponseAsync<TResponse>(response);
        }, cancellationToken);
    }

    /// <summary>
    /// Perform PUT request without response body
    /// </summary>
    public async Task PutAsync<TRequest>(
        string endpoint,
        TRequest data,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("PUT", url, data);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
            var response = await _httpClient.PutAsync(url, content, cancellationToken);
            await HandleResponseAsync(response);
            return true;
        }, cancellationToken);
    }

    /// <summary>
    /// Perform PUT request with binary content (byte[]) and return raw HttpResponseMessage.
    /// Useful for uploading files to absolute URLs returned by the API.
    /// </summary>
    public async Task<HttpResponseMessage> PutRawAsync(
        string endpoint,
        byte[] data,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, null);
        LogRequest("PUT", url);
        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        var response = await _httpClient.PutAsync(url, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var contentStr = await response.Content.ReadAsStringAsync();
            await ThrowAppropriateException(response, contentStr);
        }
        return response;
    }

    /// <summary>
    /// Perform PATCH request
    /// </summary>
    public async Task<TResponse> PatchAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("PATCH", url, data);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return await HandleResponseAsync<TResponse>(response);
        }, cancellationToken);
    }

    /// <summary>
    /// Perform DELETE request
    /// </summary>
    public async Task DeleteAsync(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        await ExecuteWithRetryAsync(async () =>
        {
            LogRequest("DELETE", url);
            var response = await _httpClient.DeleteAsync(url, cancellationToken);
            await HandleResponseAsync(response);
            return true;
        }, cancellationToken);
    }

    private string BuildUrl(string endpoint, Dictionary<string, string>? queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return endpoint;

        var queryString = string.Join("&", queryParams.Select(kvp => 
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        
        return $"{endpoint}?{queryString}";
    }

    private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        var attemptCount = 0;
        var maxAttempts = _options.MaxRetryAttempts + 1;

        while (attemptCount < maxAttempts)
        {
            try
            {
                return await action();
            }
            catch (AllegroRateLimitException ex)
            {
                attemptCount++;
                if (attemptCount >= maxAttempts)
                    throw;

                var delay = ex.RetryAfterSeconds.HasValue 
                    ? TimeSpan.FromSeconds(ex.RetryAfterSeconds.Value)
                    : TimeSpan.FromMilliseconds(_options.RetryDelayMilliseconds * attemptCount);

                _logger?.LogWarning($"Rate limit exceeded. Retrying after {delay.TotalSeconds} seconds (attempt {attemptCount}/{maxAttempts})");
                await Task.Delay(delay, cancellationToken);
            }
            catch (AllegroServerException) when (attemptCount < maxAttempts)
            {
                attemptCount++;
                if (attemptCount >= maxAttempts)
                    throw;

                var delay = TimeSpan.FromMilliseconds(_options.RetryDelayMilliseconds * attemptCount);
                _logger?.LogWarning($"Server error occurred. Retrying after {delay.TotalMilliseconds}ms (attempt {attemptCount}/{maxAttempts})");
                await Task.Delay(delay, cancellationToken);
            }
            catch (AllegroNetworkException) when (attemptCount < maxAttempts)
            {
                attemptCount++;
                if (attemptCount >= maxAttempts)
                    throw;

                var delay = TimeSpan.FromMilliseconds(_options.RetryDelayMilliseconds * attemptCount);
                _logger?.LogWarning($"Network error occurred. Retrying after {delay.TotalMilliseconds}ms (attempt {attemptCount}/{maxAttempts})");
                await Task.Delay(delay, cancellationToken);
            }
            catch (Exception ex) when (ex is not AllegroApiException)
            {
                throw new AllegroApiException("Unexpected error occurred", ex);
            }
        }

        throw new AllegroApiException("Maximum retry attempts exceeded");
    }

    private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        LogResponse(response.StatusCode, content);

        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(content))
                return default!;

            try
            {
                return JsonSerializer.Deserialize<T>(content, _jsonOptions)!;
            }
            catch (JsonException ex)
            {
                throw new AllegroApiException("Failed to deserialize response", ex);
            }
        }

        await ThrowAppropriateException(response, content);
        return default!;
    }

    /// <summary>
    /// Read JSON body from HttpResponseMessage and deserialize to T using client's serializer settings.
    /// </summary>
    public async Task<T> ReadJsonAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            return default!;

        try
        {
            return JsonSerializer.Deserialize<T>(content, _jsonOptions)!;
        }
        catch (JsonException ex)
        {
            throw new AllegroApiException("Failed to deserialize response", ex);
        }
    }

    private async Task HandleResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        LogResponse(response.StatusCode, content);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowAppropriateException(response, content);
        }
    }

    private async Task ThrowAppropriateException(HttpResponseMessage response, string content)
    {
        ErrorsHolder? errors = null;
        AuthError? authError = null;

        try
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    authError = JsonSerializer.Deserialize<AuthError>(content, _jsonOptions);
                }
                else
                {
                    errors = JsonSerializer.Deserialize<ErrorsHolder>(content, _jsonOptions);
                }
            }
        }
        catch (JsonException)
        {
            // Ignore deserialization errors and use raw content
        }

        var message = errors?.Errors?.FirstOrDefault()?.Message 
            ?? authError?.ErrorDescription 
            ?? response.ReasonPhrase 
            ?? "Unknown error";

        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                throw new AllegroBadRequestException(message, errors?.Errors ?? new List<Error>());

            case HttpStatusCode.Unauthorized:
                throw new AllegroAuthenticationException(
                    authError?.ErrorDescription ?? "Authentication failed");

            case HttpStatusCode.Forbidden:
                var errorCode = errors?.Errors?.FirstOrDefault()?.Code;
                throw new AllegroAuthorizationException(message, errorCode ?? "FORBIDDEN");

            case HttpStatusCode.NotFound:
                throw new AllegroNotFoundException(message);

            case HttpStatusCode.Conflict:
                throw new AllegroConflictException(message, 
                    errors?.Errors?.FirstOrDefault()?.Code ?? "CONFLICT");

            case (HttpStatusCode)422: // Unprocessable Entity
                throw new AllegroUnprocessableEntityException(message, errors?.Errors ?? new List<Error>());

            case (HttpStatusCode)429: // Too Many Requests
                int? retryAfter = null;
                if (response.Headers.TryGetValues("Retry-After", out var retryValues))
                {
                    if (int.TryParse(retryValues.FirstOrDefault(), out var seconds))
                    {
                        retryAfter = seconds;
                    }
                }
                throw new AllegroRateLimitException(message, retryAfter);

            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout:
                throw new AllegroServerException(message, response.StatusCode);

            default:
                throw new AllegroApiException($"Request failed with status {(int)response.StatusCode}: {message}", 
                    response.StatusCode);
        }
    }

    private void LogRequest(string method, string url, object? data = null)
    {
        if (!_options.EnableLogging || _logger == null)
            return;

        _logger.LogDebug($"[Allegro API] {method} {url}");
        if (data != null)
        {
            _logger.LogDebug($"[Allegro API] Request body: {JsonSerializer.Serialize(data, _jsonOptions)}");
        }
    }

    private void LogResponse(HttpStatusCode statusCode, string content)
    {
        if (!_options.EnableLogging || _logger == null)
            return;

        _logger.LogDebug($"[Allegro API] Response: {(int)statusCode} {statusCode}");
        if (!string.IsNullOrWhiteSpace(content))
        {
            _logger.LogDebug($"[Allegro API] Response body: {content}");
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
