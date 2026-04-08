using AllegroApi.Http;
using AllegroApi.Models.Orders;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing commission refund claims.
/// Allows sellers to request refunds on transaction fees for problematic orders.
/// </summary>
public class RefundClaimsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the RefundClaimsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public RefundClaimsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of refund claims based on query parameters.
    /// </summary>
    /// <param name="offerId">Filter by offer identifier.</param>
    /// <param name="buyerLogin">Filter by buyer login.</param>
    /// <param name="status">Filter by claim status.</param>
    /// <param name="limit">Maximum number of claims to return (1-100, default 25).</param>
    /// <param name="offset">Index of first returned claim (default 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of refund claims.</returns>
    public System.Threading.Tasks.Task<RefundClaimsResponse> GetRefundClaimsAsync(
        string? offerId = null,
        string? buyerLogin = null,
        string? status = null,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(offerId))
            queryParams["lineItem.offer.id"] = offerId;
        if (!string.IsNullOrEmpty(buyerLogin))
            queryParams["buyer.login"] = buyerLogin;
        if (!string.IsNullOrEmpty(status))
            queryParams["status"] = status;
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<RefundClaimsResponse>(
            "/order/refund-claims",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets refund claim details by ID.
    /// </summary>
    /// <param name="claimId">Refund claim identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Refund claim details.</returns>
    public System.Threading.Tasks.Task<RefundClaim> GetRefundClaimAsync(
        string claimId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(claimId);
        return _httpClient.GetAsync<RefundClaim>(
            $"/order/refund-claims/{claimId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new refund claim for commission refund.
    /// </summary>
    /// <param name="request">Refund claim request with line item ID and reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created refund claim.</returns>
    public System.Threading.Tasks.Task<RefundClaim> CreateRefundClaimAsync(
        CreateRefundClaimRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CreateRefundClaimRequest, RefundClaim>(
            "/order/refund-claims",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Cancels a refund claim. This action cannot be undone.
    /// </summary>
    /// <param name="claimId">Refund claim identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    public System.Threading.Tasks.Task CancelRefundClaimAsync(
        string claimId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(claimId);
        return _httpClient.DeleteAsync(
            $"/order/refund-claims/{claimId}",
            null,
            cancellationToken);
    }
}
