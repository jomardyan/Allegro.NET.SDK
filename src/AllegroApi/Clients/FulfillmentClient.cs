using AllegroApi.Http;
using AllegroApi.Models.Fulfillment;

namespace AllegroApi.Clients;

/// <summary>
/// Provides methods for managing Allegro Fulfillment services.
/// Handles Advance Ship Notices (ASN), stock management, parcels, and warehouse operations.
/// </summary>
public class FulfillmentClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="FulfillmentClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making API requests.</param>
    public FulfillmentClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Advance Ship Notices

    /// <summary>
    /// Gets a list of Advance Ship Notices.
    /// The list is ordered by creation date and can be filtered by status.
    /// </summary>
    /// <param name="offset">The offset of elements in the response (default: 0).</param>
    /// <param name="limit">Maximum number of elements in response (1-200, default: 50).</param>
    /// <param name="statuses">Optional list of statuses to filter by (DRAFT, IN_TRANSIT, UNPACKING, COMPLETED, CANCELLED).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of Advance Ship Notices.</returns>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<AdvanceShipNoticeList> GetAdvanceShipNoticesAsync(
        int offset = 0,
        int limit = 50,
        List<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };

        if (statuses != null && statuses.Count > 0)
        {
            foreach (var status in statuses)
            {
                queryParams[$"status"] = status;
            }
        }

        return _httpClient.GetAsync<AdvanceShipNoticeList>(
            "/fulfillment/advance-ship-notices",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new Advance Ship Notice.
    /// </summary>
    /// <param name="request">The request containing ASN details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created Advance Ship Notice response.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<AdvanceShipNoticeResponse> CreateAdvanceShipNoticeAsync(
        CreateAdvanceShipNoticeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PostAsync<CreateAdvanceShipNoticeRequest, AdvanceShipNoticeResponse>(
            "/fulfillment/advance-ship-notices",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a single Advance Ship Notice by ID.
    /// </summary>
    /// <param name="id">The UUID identifier of the Advance Ship Notice.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Advance Ship Notice details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Thrown when the ASN is not found.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<AdvanceShipNoticeResponse> GetAdvanceShipNoticeAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.GetAsync<AdvanceShipNoticeResponse>(
            $"/fulfillment/advance-ship-notices/{id}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates a submitted Advance Ship Notice.
    /// Update is allowed only when ASN is in IN_TRANSIT status.
    /// Note: If-Match header support requires custom implementation.
    /// </summary>
    /// <param name="id">The UUID identifier of the Advance Ship Notice.</param>
    /// <param name="request">The request containing updated ASN details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated Advance Ship Notice.</returns>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<AdvanceShipNoticeResponse> UpdateSubmittedAdvanceShipNoticeAsync(
        string id,
        UpdateSubmittedAdvanceShipNoticeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PutAsync<UpdateSubmittedAdvanceShipNoticeRequest, AdvanceShipNoticeResponse>(
            $"/fulfillment/advance-ship-notices/{id}/submitted",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Cancels an Advance Ship Notice.
    /// Can only cancel ASN in IN_TRANSIT status.
    /// </summary>
    /// <param name="id">The UUID identifier of the Advance Ship Notice to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails or ASN cannot be cancelled.</exception>
    public async System.Threading.Tasks.Task CancelAdvanceShipNoticeAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        await _httpClient.PutAsync<object, object>(
            $"/fulfillment/advance-ship-notices/{id}/cancel",
            new { },
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets labels for an Advance Ship Notice in PDF format.
    /// Returns the raw binary content.
    /// </summary>
    /// <param name="id">The UUID identifier of the Advance Ship Notice.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The label file as byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Thrown when the ASN is not found.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public async System.Threading.Tasks.Task<byte[]> GetAdvanceShipNoticeLabelsAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var response = await _httpClient.GetRawAsync(
            $"/fulfillment/advance-ship-notices/{id}/labels",
            null,
            cancellationToken);

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Submits an Advance Ship Notice using a submit command.
    /// After submission, updates are limited to selected properties only.
    /// </summary>
    /// <param name="commandId">The UUID identifier of the command.</param>
    /// <param name="command">The submit command with ASN ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The submit command result with status and possible errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<SubmitCommand> SubmitAdvanceShipNoticeAsync(
        string commandId,
        SubmitCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        ArgumentNullException.ThrowIfNull(command);

        return _httpClient.PutAsync<SubmitCommand, SubmitCommand>(
            $"/fulfillment/submit-commands/{commandId}",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the current receiving state and details of an Advance Ship Notice.
    /// Shows real-time progress and information about received items.
    /// </summary>
    /// <param name="id">The UUID identifier of the Advance Ship Notice.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The receiving state with progress and item details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Thrown when ASN is not found or in DRAFT/IN_TRANSIT state.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<ReceivingState> GetAdvanceShipNoticeReceivingStateAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.GetAsync<ReceivingState>(
            $"/fulfillment/advance-ship-notices/{id}/receiving-state",
            null,
            cancellationToken);
    }

    #endregion

    #region Stock Management

    /// <summary>
    /// Gets available stock from Allegro Warehouse.
    /// Supports filtering, sorting, and pagination.
    /// </summary>
    /// <param name="offset">The offset of elements (default: 0).</param>
    /// <param name="limit">Maximum number of elements (1-1000, default: 50).</param>
    /// <param name="phrase">Filter by product name (minimum 3 characters).</param>
    /// <param name="sort">Sort field (available, unfulfillable, name, etc.).</param>
    /// <param name="productId">Filter by product UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of stock products.</returns>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<StockProductList> GetFulfillmentStockAsync(
        int offset = 0,
        int limit = 50,
        string? phrase = null,
        string? sort = null,
        string? productId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };

        if (!string.IsNullOrWhiteSpace(phrase))
        {
            queryParams["phrase"] = phrase;
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            queryParams["sort"] = sort;
        }

        if (!string.IsNullOrWhiteSpace(productId))
        {
            queryParams["productId"] = productId;
        }

        return _httpClient.GetAsync<StockProductList>(
            "/fulfillment/stock",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Fulfillment Orders and Parcels

    /// <summary>
    /// Gets list of shipped parcels for a given order.
    /// Includes detailed information such as expiration dates and serial numbers.
    /// </summary>
    /// <param name="orderId">The UUID of the Allegro order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The fulfillment order with parcels.</returns>
    /// <exception cref="ArgumentNullException">Thrown when orderId is null.</exception>
    /// <exception cref="AllegroNotFoundException">Thrown when the order is not found.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<FulfillmentOrder> GetOrderParcelsAsync(
        string orderId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orderId);

        return _httpClient.GetAsync<FulfillmentOrder>(
            $"/fulfillment/orders/{orderId}/parcels",
            null,
            cancellationToken);
    }

    #endregion

    #region Available Products

    /// <summary>
    /// Gets list of products that can be added to Advance Ship Notice.
    /// Contains products for which the seller has created offers.
    /// </summary>
    /// <param name="offset">The offset of elements (default: 0).</param>
    /// <param name="limit">Maximum number of elements (1-100, default: 50).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of available products.</returns>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<AvailableProductsList> GetAvailableProductsAsync(
        int offset = 0,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };

        return _httpClient.GetAsync<AvailableProductsList>(
            "/fulfillment/available-products",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Tax Identification

    /// <summary>
    /// Adds a tax identification number.
    /// For international sellers only.
    /// </summary>
    /// <param name="request">The tax ID request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails or tax ID is invalid.</exception>
    public async System.Threading.Tasks.Task AddTaxIdAsync(
        TaxIdRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _httpClient.PostAsync<TaxIdRequest, object>(
            "/fulfillment/tax-id",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates a tax identification number.
    /// For international sellers only.
    /// </summary>
    /// <param name="request">The tax ID request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails or tax ID is invalid.</exception>
    public async System.Threading.Tasks.Task UpdateTaxIdAsync(
        TaxIdRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _httpClient.PutAsync<TaxIdRequest, object>(
            "/fulfillment/tax-id",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets tax identification number with verification status.
    /// After adding/updating, status will be NOT_VERIFIED until acceptance.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The tax ID with verification status.</returns>
    /// <exception cref="AllegroNotFoundException">Thrown when tax ID is not found.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<TaxIdResponse> GetTaxIdAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<TaxIdResponse>(
            "/fulfillment/tax-id",
            null,
            cancellationToken);
    }

    #endregion

    #region Removal Preferences

    /// <summary>
    /// Gets the current active removal preference.
    /// Removal preference determines how products are removed from the warehouse.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The current fulfillment removal preference.</returns>
    /// <exception cref="AllegroNotFoundException">Thrown when removal preferences were never set.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<FulfillmentRemovalPreference> GetRemovalPreferencesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<FulfillmentRemovalPreference>(
            "/fulfillment/removal/preferences",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new active fulfillment removal preference.
    /// Becomes active immediately for all new system removal orders.
    /// </summary>
    /// <param name="request">The removal preference request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created fulfillment removal preference.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="AllegroApiException">Thrown when the API request fails.</exception>
    public System.Threading.Tasks.Task<FulfillmentRemovalPreference> CreateRemovalPreferencesAsync(
        FulfillmentRemovalPreference request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PutAsync<FulfillmentRemovalPreference, FulfillmentRemovalPreference>(
            "/fulfillment/removal/preferences",
            request,
            null,
            cancellationToken);
    }

    #endregion
}
