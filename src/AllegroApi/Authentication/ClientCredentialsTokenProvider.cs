using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AllegroApi.Exceptions;

namespace AllegroApi.Authentication;

/// <summary>
/// Acquires and caches an application access token from the Allegro OAuth2 token
/// endpoint using the <c>client_credentials</c> grant, refreshing it automatically
/// shortly before it expires. Thread-safe.
/// </summary>
public sealed class ClientCredentialsTokenProvider : IAllegroTokenProvider, IDisposable
{
    // Refresh slightly before the real expiry to avoid races with in-flight requests.
    private static readonly TimeSpan ExpirySafetyMargin = TimeSpan.FromSeconds(60);

    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly string _tokenEndpoint;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly SemaphoreSlim _gate = new(1, 1);

    private string? _accessToken;
    private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCredentialsTokenProvider"/> class.
    /// </summary>
    /// <param name="tokenEndpoint">The OAuth2 token endpoint URL.</param>
    /// <param name="clientId">The OAuth2 client identifier.</param>
    /// <param name="clientSecret">The OAuth2 client secret.</param>
    /// <param name="httpClient">Optional HttpClient to use for token requests. When omitted, an internal one is created and disposed with the provider.</param>
    public ClientCredentialsTokenProvider(string tokenEndpoint, string clientId, string clientSecret, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(tokenEndpoint))
            throw new ArgumentException("Token endpoint cannot be null or empty.", nameof(tokenEndpoint));
        if (string.IsNullOrWhiteSpace(clientId))
            throw new ArgumentException("Client id cannot be null or empty.", nameof(clientId));
        if (string.IsNullOrWhiteSpace(clientSecret))
            throw new ArgumentException("Client secret cannot be null or empty.", nameof(clientSecret));

        _tokenEndpoint = tokenEndpoint;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _ownsHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient();
    }

    /// <inheritdoc />
    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_accessToken is not null && DateTimeOffset.UtcNow < _expiresAt)
            return _accessToken;

        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_accessToken is not null && DateTimeOffset.UtcNow < _expiresAt)
                return _accessToken;

            await RequestTokenAsync(cancellationToken).ConfigureAwait(false);
            return _accessToken!;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <inheritdoc />
    public void Invalidate()
    {
        _accessToken = null;
        _expiresAt = DateTimeOffset.MinValue;
    }

    private async Task RequestTokenAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
        var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new AllegroAuthenticationException(
                $"Failed to obtain an access token from '{_tokenEndpoint}': {(int)response.StatusCode} {response.ReasonPhrase}. {body}");
        }

        OAuthTokenResponse? token;
        try
        {
            token = JsonSerializer.Deserialize<OAuthTokenResponse>(body);
        }
        catch (JsonException ex)
        {
            throw new AllegroAuthenticationException("Failed to parse the token endpoint response.", ex);
        }

        if (token is null || string.IsNullOrEmpty(token.AccessToken))
            throw new AllegroAuthenticationException("The token endpoint did not return an access token.");

        _accessToken = token.AccessToken;
        var lifetime = TimeSpan.FromSeconds(Math.Max(token.ExpiresIn, 0));
        // Subtract the safety margin, but never produce a non-positive lifetime.
        if (lifetime > ExpirySafetyMargin)
            lifetime -= ExpirySafetyMargin;
        _expiresAt = DateTimeOffset.UtcNow.Add(lifetime);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsHttpClient)
            _httpClient.Dispose();
        _gate.Dispose();
    }

    private sealed record OAuthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("scope")]
        public string? Scope { get; init; }
    }
}
