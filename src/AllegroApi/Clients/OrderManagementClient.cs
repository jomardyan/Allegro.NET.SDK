using AllegroApi.Http;
using AllegroApi.Models.Orders;
using Microsoft.Extensions.Logging;

namespace AllegroApi.Clients;

/// <summary>
/// Client for order management operations in Allegro API
/// </summary>
public class OrderManagementClient
{
    private readonly AllegroHttpClient _httpClient;
    private readonly ILogger<OrderManagementClient>? _logger;

    /// <summary>
    /// Initializes a new instance of the OrderManagementClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    /// <param name="logger">Optional logger for diagnostics.</param>
    public OrderManagementClient(AllegroHttpClient httpClient, ILogger<OrderManagementClient>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
    }

    /// <summary>
    /// Get orders (checkout forms)
    /// </summary>
    public async Task<OrdersSearchResult> GetOrdersAsync(
        OrderSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Getting orders");
        return await _httpClient.GetAsync<OrdersSearchResult>(
            "/order/checkout-forms",
            searchParams.ToQueryParams(),
            cancellationToken);
    }

    /// <summary>
    /// Get a specific order by ID
    /// </summary>
    public async Task<Order> GetOrderAsync(
        string orderId,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting order {orderId}");
        return await _httpClient.GetAsync<Order>(
            $"/order/checkout-forms/{orderId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets order events statistics containing the latest event ID and occurrence date.
    /// Provides the current starting point for reading events.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order event statistics.</returns>
    public System.Threading.Tasks.Task<OrderEventStats> GetOrderEventsStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Getting order events statistics");
        return _httpClient.GetAsync<OrderEventStats>(
            "/order/event-stats",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the list of available shipping carriers.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of carriers.</returns>
    public System.Threading.Tasks.Task<CarriersResponse> GetCarriersAsync(
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Getting carriers");
        return _httpClient.GetAsync<CarriersResponse>(
            "/order/carriers",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates the fulfillment status of an order.
    /// </summary>
    /// <param name="checkoutFormId">Checkout form identifier.</param>
    /// <param name="request">Fulfillment update request with new status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    public System.Threading.Tasks.Task UpdateFulfillmentStatusAsync(
        string checkoutFormId,
        FulfillmentUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(checkoutFormId);
        ArgumentNullException.ThrowIfNull(request);
        _logger?.LogInformation($"Updating fulfillment status for order {checkoutFormId}");
        return _httpClient.PutAsync<FulfillmentUpdateRequest, object>(
            $"/order/checkout-forms/{checkoutFormId}/fulfillment",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the list of invoices for an order.
    /// </summary>
    /// <param name="checkoutFormId">Checkout form identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of invoices.</returns>
    public System.Threading.Tasks.Task<OrderInvoicesResponse> GetOrderInvoicesAsync(
        string checkoutFormId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(checkoutFormId);
        _logger?.LogInformation($"Getting invoices for order {checkoutFormId}");
        return _httpClient.GetAsync<OrderInvoicesResponse>(
            $"/order/checkout-forms/{checkoutFormId}/invoices",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Downloads an invoice file.
    /// Returns the raw file content (PDF).
    /// </summary>
    /// <param name="checkoutFormId">Checkout form identifier.</param>
    /// <param name="invoiceId">Invoice identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Invoice file as byte array.</returns>
    public async System.Threading.Tasks.Task<byte[]> GetInvoiceFileAsync(
        string checkoutFormId,
        string invoiceId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(checkoutFormId);
        ArgumentNullException.ThrowIfNull(invoiceId);
        _logger?.LogInformation($"Downloading invoice {invoiceId} for order {checkoutFormId}");
        
        // For binary file downloads, we need to use the raw HTTP client
        var response = await _httpClient.GetRawAsync(
            $"/order/checkout-forms/{checkoutFormId}/invoices/{invoiceId}/file",
            null,
            cancellationToken);
        
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
