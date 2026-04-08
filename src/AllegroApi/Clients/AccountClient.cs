using AllegroApi.Http;
using AllegroApi.Models.Account;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing account operations.
/// </summary>
public class AccountClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AccountClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public AccountClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets the current user's account information.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account information.</returns>
    public System.Threading.Tasks.Task<AccountInfoResponse> GetAccountInfoAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AccountInfoResponse>("/me", null, cancellationToken);
    }

    /// <summary>
    /// Gets the current user's account information (alias for GetAccountInfoAsync).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account information.</returns>
    public System.Threading.Tasks.Task<AccountInfoResponse> GetMeAsync(
        CancellationToken cancellationToken = default)
    {
        return GetAccountInfoAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the current user's account settings.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account settings.</returns>
    public System.Threading.Tasks.Task<AccountSettings> GetAccountSettingsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AccountSettings>("/account/settings", null, cancellationToken);
    }

    /// <summary>
    /// Gets current sales quality with at most 30 days history.
    /// The sales quality shows how well the seller is performing based on various metrics.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Sales quality history for the last 30 days.</returns>
    public System.Threading.Tasks.Task<SalesQualityHistoryResponse> GetSalesQualityAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<SalesQualityHistoryResponse>("/sale/quality", null, cancellationToken);
    }

    /// <summary>
    /// Gets Smart! seller classification report for a marketplace.
    /// The Smart! program is Allegro's seller excellence program.
    /// </summary>
    /// <param name="marketplaceId">Marketplace identifier (e.g., "allegro-pl", "allegro-cz"). If not specified, returns for seller's registration marketplace.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Smart! seller classification report.</returns>
    public System.Threading.Tasks.Task<SmartSellerClassificationReport> GetSmartClassificationAsync(
        string? marketplaceId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = marketplaceId != null
            ? new Dictionary<string, string> { ["marketplaceId"] = marketplaceId }
            : null;

        return _httpClient.GetAsync<SmartSellerClassificationReport>("/sale/smart", queryParams, cancellationToken);
    }
}
