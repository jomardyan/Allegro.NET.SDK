using System.Net;
using System.Net.Http.Headers;

namespace AllegroApi.Authentication;

/// <summary>
/// A <see cref="DelegatingHandler"/> that attaches the OAuth2 bearer token from an
/// <see cref="IAllegroTokenProvider"/> to every outgoing request and, when enabled,
/// transparently refreshes the token once on a 401 response and retries.
/// </summary>
public sealed class AllegroAuthenticationHandler : DelegatingHandler
{
    private readonly IAllegroTokenProvider _tokenProvider;
    private readonly bool _autoRefresh;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllegroAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="tokenProvider">The token provider.</param>
    /// <param name="autoRefresh">Whether to refresh the token and retry once on a 401 response.</param>
    public AllegroAuthenticationHandler(IAllegroTokenProvider tokenProvider, bool autoRefresh = true)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _autoRefresh = autoRefresh;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.Unauthorized || !_autoRefresh)
            return response;

        // The token may have been revoked or expired server-side. Refresh once and retry.
        _tokenProvider.Invalidate();
        var refreshed = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);

        var retryRequest = await CloneRequestAsync(request, cancellationToken).ConfigureAwait(false);
        retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshed);

        response.Dispose();
        return await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        if (request.Content is not null)
        {
            var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            var newContent = new ByteArrayContent(bytes);
            foreach (var header in request.Content.Headers)
                newContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            clone.Content = newContent;
        }

        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

#if NET5_0_OR_GREATER
        foreach (var option in request.Options)
            ((IDictionary<string, object?>)clone.Options)[option.Key] = option.Value;
#endif

        return clone;
    }
}
