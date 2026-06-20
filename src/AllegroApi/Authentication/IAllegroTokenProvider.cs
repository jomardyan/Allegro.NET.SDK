namespace AllegroApi.Authentication;

/// <summary>
/// Supplies (and, where supported, refreshes) the OAuth2 access token used to
/// authenticate requests to the Allegro API.
/// </summary>
public interface IAllegroTokenProvider
{
    /// <summary>
    /// Returns a valid access token, acquiring or refreshing one if necessary.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A valid bearer access token.</returns>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Invalidates the currently cached token so that the next call to
    /// <see cref="GetAccessTokenAsync"/> acquires a fresh one. Implementations
    /// that wrap a fixed token may treat this as a no-op.
    /// </summary>
    void Invalidate();
}
