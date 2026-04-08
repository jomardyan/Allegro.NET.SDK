using AllegroApi.Http;
using AllegroApi.Models.Billing;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing billing operations and invoices.
/// </summary>
public class BillingClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the BillingClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public BillingClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of billing entries sorted by date (newest first).
    /// </summary>
    /// <param name="marketplaceId">Marketplace identifier (e.g., "allegro-pl").</param>
    /// <param name="occurredAtGte">Filter entries from this date.</param>
    /// <param name="occurredAtLte">Filter entries to this date.</param>
    /// <param name="typeIds">List of billing type IDs to filter by.</param>
    /// <param name="offerId">Filter by offer ID.</param>
    /// <param name="orderId">Filter by order ID.</param>
    /// <param name="limit">Maximum number of entries to return (1-100, default: 100).</param>
    /// <param name="offset">Number of entries to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of billing entries.</returns>
    public System.Threading.Tasks.Task<BillingEntries> GetBillingEntriesAsync(
        string? marketplaceId = null,
        DateTime? occurredAtGte = null,
        DateTime? occurredAtLte = null,
        List<string>? typeIds = null,
        string? offerId = null,
        string? orderId = null,
        int limit = 100,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString(),
            ["offset"] = offset.ToString()
        };

        if (!string.IsNullOrEmpty(marketplaceId))
            queryParams["marketplaceId"] = marketplaceId;
        if (occurredAtGte.HasValue)
            queryParams["occurredAt.gte"] = occurredAtGte.Value.ToString("O");
        if (occurredAtLte.HasValue)
            queryParams["occurredAt.lte"] = occurredAtLte.Value.ToString("O");
        if (typeIds != null && typeIds.Count > 0)
        {
            for (int i = 0; i < typeIds.Count; i++)
                queryParams[$"type.id[{i}]"] = typeIds[i];
        }
        if (!string.IsNullOrEmpty(offerId))
            queryParams["offer.id"] = offerId;
        if (!string.IsNullOrEmpty(orderId))
            queryParams["order.id"] = orderId;

        return _httpClient.GetAsync<BillingEntries>(
            "/billing/billing-entries",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets a list of available billing types.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of billing types.</returns>
    public System.Threading.Tasks.Task<BillingTypesResponse> GetBillingTypesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<BillingTypesResponse>(
            "/billing/billing-types",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a list of invoices.
    /// </summary>
    /// <param name="limit">Maximum number of invoices to return (default: 20).</param>
    /// <param name="offset">Number of invoices to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of invoices.</returns>
    public System.Threading.Tasks.Task<InvoicesList> GetInvoicesAsync(
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<InvoicesList>(
            $"/billing/invoices?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific invoice by ID.
    /// </summary>
    /// <param name="invoiceId">The invoice identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Invoice details.</returns>
    public System.Threading.Tasks.Task<Invoice> GetInvoiceAsync(
        string invoiceId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(invoiceId);
        return _httpClient.GetAsync<Invoice>(
            $"/billing/invoices/{invoiceId}",
            null,
            cancellationToken);
    }
}
