namespace AllegroApi.Authentication;

/// <summary>
/// A token provider that returns a fixed, pre-acquired access token.
/// Used when the caller supplies an <c>AccessToken</c> directly; it cannot refresh.
/// </summary>
public sealed class StaticTokenProvider : IAllegroTokenProvider
{
    private readonly string _accessToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticTokenProvider"/> class.
    /// </summary>
    /// <param name="accessToken">The fixed access token.</param>
    public StaticTokenProvider(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("Access token cannot be null or empty.", nameof(accessToken));
        _accessToken = accessToken;
    }

    /// <inheritdoc />
    public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(_accessToken);

    /// <inheritdoc />
    public void Invalidate()
    {
        // A fixed token cannot be refreshed.
    }
}
