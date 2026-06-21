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
        
        return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets order events (a stream of changes to the seller's orders).
    /// </summary>
    /// <param name="from">Identifier of the last processed event; only newer events are returned.</param>
    /// <param name="types">Optional event types to filter by.</param>
    /// <param name="limit">Maximum number of events to return (1-1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of order events.</returns>
    public System.Threading.Tasks.Task<OrderEventsList> GetOrderEventsAsync(
        string? from = null,
        IEnumerable<string>? types = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<string>();
        if (!string.IsNullOrEmpty(from))
            query.Add($"from={Uri.EscapeDataString(from)}");
        if (types != null)
            query.AddRange(types.Select(t => $"type={Uri.EscapeDataString(t)}"));
        if (limit.HasValue)
            query.Add($"limit={limit.Value}");

        var endpoint = query.Count > 0 ? $"/order/events?{string.Join("&", query)}" : "/order/events";
        return _httpClient.GetAsync<OrderEventsList>(endpoint, null, cancellationToken);
    }

    /// <summary>
    /// Gets the list of parcel tracking numbers (waybills) attached to an order.
    /// </summary>
    /// <param name="orderId">Order (checkout form) identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Parcel tracking numbers attached to the order.</returns>
    public System.Threading.Tasks.Task<CheckoutFormOrderWaybillResponse> GetOrderShipmentsAsync(
        string orderId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orderId);
        return _httpClient.GetAsync<CheckoutFormOrderWaybillResponse>(
            $"/order/checkout-forms/{orderId}/shipments",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Adds a parcel tracking number (waybill) to an order.
    /// </summary>
    /// <param name="orderId">Order (checkout form) identifier.</param>
    /// <param name="request">Waybill details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created waybill.</returns>
    public System.Threading.Tasks.Task<CheckoutFormAddWaybillCreated> AddOrderShipmentAsync(
        string orderId,
        CheckoutFormAddWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orderId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CheckoutFormAddWaybillRequest, CheckoutFormAddWaybillCreated>(
            $"/order/checkout-forms/{orderId}/shipments",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the carrier parcel tracking history for one or more waybills.
    /// </summary>
    /// <param name="carrierId">Carrier identifier.</param>
    /// <param name="waybills">Waybill (tracking) numbers to query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tracking history for the requested waybills.</returns>
    public System.Threading.Tasks.Task<CarrierParcelTrackingResponse> GetParcelTrackingAsync(
        string carrierId,
        IEnumerable<string> waybills,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(carrierId);
        ArgumentNullException.ThrowIfNull(waybills);
        var qs = string.Join("&", waybills.Select(w => $"waybill={Uri.EscapeDataString(w)}"));
        return _httpClient.GetAsync<CarrierParcelTrackingResponse>(
            $"/order/carriers/{carrierId}/tracking?{qs}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets Allegro pickup/drop-off points.
    /// </summary>
    /// <param name="carriers">Optional carrier identifiers to filter by.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of Allegro pickup/drop-off points.</returns>
    public System.Threading.Tasks.Task<AllegroPickupDropOffPointsResponse> GetAllegroPickupDropOffPointsAsync(
        IEnumerable<string>? carriers = null,
        CancellationToken cancellationToken = default)
    {
        var endpoint = "/order/carriers/ALLEGRO/points";
        if (carriers != null)
        {
            var qs = string.Join("&", carriers.Select(c => $"carriers={Uri.EscapeDataString(c)}"));
            if (qs.Length > 0)
                endpoint += $"?{qs}";
        }
        return _httpClient.GetAsync<AllegroPickupDropOffPointsResponse>(endpoint, null, cancellationToken);
    }

    /// <summary>
    /// Uploads a URL to a billing document associated with an order.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="link">Billing document link.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task UploadBillingDocumentLinkAsync(
        string orderId,
        NewOrderBillingDocumentLink link,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orderId);
        ArgumentNullException.ThrowIfNull(link);
        return _httpClient.PostAsync<NewOrderBillingDocumentLink>(
            $"/order/{orderId}/billing-documents/links",
            link,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Sets serial numbers for an order's line items.
    /// </summary>
    /// <param name="checkoutFormId">Checkout form (order) identifier.</param>
    /// <param name="request">Line items with their serial numbers.</param>
    /// <param name="revision">Optional checkout form revision for optimistic concurrency.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task SetLineItemsSerialNumbersAsync(
        string checkoutFormId,
        CheckoutFormLineItemsSetSerialNumbersRequest request,
        string? revision = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(checkoutFormId);
        ArgumentNullException.ThrowIfNull(request);
        var endpoint = $"/order/checkout-forms/{checkoutFormId}/serial-numbers";
        if (!string.IsNullOrEmpty(revision))
            endpoint += $"?checkoutForm.revision={Uri.EscapeDataString(revision)}";
        return _httpClient.PostAsync<CheckoutFormLineItemsSetSerialNumbersRequest>(
            endpoint,
            request,
            null,
            cancellationToken);
    }
}
