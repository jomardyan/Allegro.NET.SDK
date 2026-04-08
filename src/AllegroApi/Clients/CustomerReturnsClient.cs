using AllegroApi.Http;
using AllegroApi.Models.CustomerReturns;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing customer returns.
/// Provides functionality for listing, viewing, and rejecting customer returns.
/// Note: This API is in BETA status.
/// </summary>
public class CustomerReturnsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the CustomerReturnsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public CustomerReturnsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of customer returns filtered by query parameters.
    /// Rate limit: 25 requests per second per user, 50 requests per second per clientId.
    /// </summary>
    /// <param name="customerReturnId">One or more customer return IDs (optional).</param>
    /// <param name="orderId">One or more order IDs (optional).</param>
    /// <param name="buyerEmail">One or more buyer emails (optional).</param>
    /// <param name="buyerLogin">One or more buyer logins (optional).</param>
    /// <param name="status">Return status filter (optional). Allowed values: CREATED, DISPATCHED, IN_TRANSIT, DELIVERED, FINISHED, FINISHED_APT, REJECTED, COMMISSION_REFUND_CLAIMED, COMMISSION_REFUNDED, WAREHOUSE_DELIVERED, WAREHOUSE_VERIFICATION.</param>
    /// <param name="createdAtGte">Filter by creation date greater than or equal to (ISO 8601 format) (optional).</param>
    /// <param name="createdAtLte">Filter by creation date less than or equal to (ISO 8601 format) (optional).</param>
    /// <param name="marketplaceId">The marketplace ID where operation was made (optional).</param>
    /// <param name="limit">Limit of customer returns per page (1-1000, default 100) (optional).</param>
    /// <param name="offset">Offset for pagination (default 0) (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of customer returns matching the criteria.</returns>
    public System.Threading.Tasks.Task<CustomerReturnResponse> GetCustomerReturnsAsync(
        string? customerReturnId = null,
        string? orderId = null,
        string? buyerEmail = null,
        string? buyerLogin = null,
        string? status = null,
        string? createdAtGte = null,
        string? createdAtLte = null,
        string? marketplaceId = null,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(customerReturnId))
            queryParams["customerReturnId"] = customerReturnId;
        if (!string.IsNullOrEmpty(orderId))
            queryParams["orderId"] = orderId;
        if (!string.IsNullOrEmpty(buyerEmail))
            queryParams["buyer.email"] = buyerEmail;
        if (!string.IsNullOrEmpty(buyerLogin))
            queryParams["buyer.login"] = buyerLogin;
        if (!string.IsNullOrEmpty(status))
            queryParams["status"] = status;
        if (!string.IsNullOrEmpty(createdAtGte))
            queryParams["createdAt.gte"] = createdAtGte;
        if (!string.IsNullOrEmpty(createdAtLte))
            queryParams["createdAt.lte"] = createdAtLte;
        if (!string.IsNullOrEmpty(marketplaceId))
            queryParams["marketplaceId"] = marketplaceId;
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<CustomerReturnResponse>(
            "/order/customer-returns",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets detailed information about a specific customer return by its identifier.
    /// </summary>
    /// <param name="customerReturnId">Customer return identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed customer return information.</returns>
    public System.Threading.Tasks.Task<CustomerReturn> GetCustomerReturnByIdAsync(
        string customerReturnId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customerReturnId);
        return _httpClient.GetAsync<CustomerReturn>(
            $"/order/customer-returns/{customerReturnId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Rejects a customer return refund with the provided reason.
    /// </summary>
    /// <param name="customerReturnId">Customer return identifier.</param>
    /// <param name="rejectionRequest">Rejection request with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated customer return with rejection information.</returns>
    public System.Threading.Tasks.Task<CustomerReturn> RejectCustomerReturnRefundAsync(
        string customerReturnId,
        CustomerReturnRefundRejectionRequest rejectionRequest,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customerReturnId);
        ArgumentNullException.ThrowIfNull(rejectionRequest);
        return _httpClient.PostAsync<CustomerReturnRefundRejectionRequest, CustomerReturn>(
            $"/order/customer-returns/{customerReturnId}/rejection",
            rejectionRequest,
            null,
            cancellationToken);
    }
}
