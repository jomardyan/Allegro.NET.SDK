using System.Text.Json.Serialization;

namespace AllegroApi.Models.Orders;

/// <summary>
/// List of order events.
/// </summary>
public record OrderEventsList
{
    /// <summary>
    /// Collection of order events.
    /// </summary>
    [JsonPropertyName("events")]
    public List<OrderEvent>? Events { get; init; }
}

/// <summary>
/// A single order event.
/// </summary>
public record OrderEvent
{
    /// <summary>
    /// Event identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Order data associated with the event.
    /// </summary>
    [JsonPropertyName("order")]
    public OrderEventData? Order { get; init; }

    /// <summary>
    /// Event type: BOUGHT, FILLED_IN, READY_FOR_PROCESSING, BUYER_CANCELLED, FULFILLMENT_STATUS_CHANGED, BUYER_MODIFIED or AUTO_CANCELLED.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Date and time the event occurred (ISO 8601).
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }
}

/// <summary>
/// Order data carried by an order event.
/// </summary>
public record OrderEventData
{
    /// <summary>
    /// Checkout form reference.
    /// </summary>
    [JsonPropertyName("checkoutForm")]
    public OrderEventCheckoutForm? CheckoutForm { get; init; }

    /// <summary>
    /// Seller reference.
    /// </summary>
    [JsonPropertyName("seller")]
    public OrderEventSeller? Seller { get; init; }

    /// <summary>
    /// Buyer reference.
    /// </summary>
    [JsonPropertyName("buyer")]
    public OrderEventBuyer? Buyer { get; init; }

    /// <summary>
    /// Line items affected by the event.
    /// </summary>
    [JsonPropertyName("lineItems")]
    public List<OrderEventLineItem>? LineItems { get; init; }

    /// <summary>
    /// Marketplace the order belongs to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public OrderMarketplace? Marketplace { get; init; }
}

/// <summary>
/// Checkout form reference in an order event.
/// </summary>
public record OrderEventCheckoutForm
{
    /// <summary>
    /// Checkout form identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Checkout form revision.
    /// </summary>
    [JsonPropertyName("revision")]
    public string? Revision { get; init; }
}

/// <summary>
/// Seller reference in an order event.
/// </summary>
public record OrderEventSeller
{
    /// <summary>
    /// Seller identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Buyer reference in an order event.
/// </summary>
public record OrderEventBuyer
{
    /// <summary>
    /// Buyer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Buyer email.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Buyer login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// Whether the buyer is a guest.
    /// </summary>
    [JsonPropertyName("guest")]
    public bool? Guest { get; init; }
}

/// <summary>
/// Line item in an order event.
/// </summary>
public record OrderEventLineItem
{
    /// <summary>
    /// Line item identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Offer reference.
    /// </summary>
    [JsonPropertyName("offer")]
    public OrderEventOffer? Offer { get; init; }

    /// <summary>
    /// Quantity bought.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }

    /// <summary>
    /// Date and time the item was bought (ISO 8601).
    /// </summary>
    [JsonPropertyName("boughtAt")]
    public DateTime? BoughtAt { get; init; }
}

/// <summary>
/// Offer reference in an order event line item.
/// </summary>
public record OrderEventOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Offer name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Response listing parcel tracking numbers (waybills) attached to an order.
/// </summary>
public record CheckoutFormOrderWaybillResponse
{
    /// <summary>
    /// Shipments (waybills) attached to the order.
    /// </summary>
    [JsonPropertyName("shipments")]
    public List<CheckoutFormWaybill>? Shipments { get; init; }
}

/// <summary>
/// A parcel tracking number (waybill) attached to an order.
/// </summary>
public record CheckoutFormWaybill
{
    /// <summary>
    /// Shipment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Waybill (tracking) number.
    /// </summary>
    [JsonPropertyName("waybill")]
    public string? Waybill { get; init; }

    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("carrierId")]
    public string? CarrierId { get; init; }

    /// <summary>
    /// Carrier name (for OTHER carriers).
    /// </summary>
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; init; }

    /// <summary>
    /// Date the waybill was created (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Request body for attaching a parcel tracking number (waybill) to an order.
/// </summary>
public record CheckoutFormAddWaybillRequest
{
    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("carrierId")]
    public string? CarrierId { get; init; }

    /// <summary>
    /// Waybill (tracking) number.
    /// </summary>
    [JsonPropertyName("waybill")]
    public string? Waybill { get; init; }

    /// <summary>
    /// Carrier name (required when carrierId is OTHER).
    /// </summary>
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; init; }

    /// <summary>
    /// Line items covered by this waybill.
    /// </summary>
    [JsonPropertyName("lineItems")]
    public List<WaybillLineItem>? LineItems { get; init; }
}

/// <summary>
/// Line item reference within a waybill request.
/// </summary>
public record WaybillLineItem
{
    /// <summary>
    /// Line item identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Response after attaching a parcel tracking number to an order.
/// </summary>
public record CheckoutFormAddWaybillCreated
{
    /// <summary>
    /// Shipment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Waybill (tracking) number.
    /// </summary>
    [JsonPropertyName("waybill")]
    public string? Waybill { get; init; }

    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("carrierId")]
    public string? CarrierId { get; init; }

    /// <summary>
    /// Carrier name.
    /// </summary>
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; init; }

    /// <summary>
    /// Date the waybill was created (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Carrier parcel tracking history response.
/// </summary>
public record CarrierParcelTrackingResponse
{
    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("carrierId")]
    public string? CarrierId { get; init; }

    /// <summary>
    /// Tracking history per waybill.
    /// </summary>
    [JsonPropertyName("waybills")]
    public List<ParcelTrackingHistory>? Waybills { get; init; }
}

/// <summary>
/// Parcel tracking history for a single waybill.
/// </summary>
public record ParcelTrackingHistory
{
    /// <summary>
    /// Waybill (tracking) number.
    /// </summary>
    [JsonPropertyName("waybill")]
    public string? Waybill { get; init; }

    /// <summary>
    /// Tracking details.
    /// </summary>
    [JsonPropertyName("trackingDetails")]
    public ParcelTrackingDetails? TrackingDetails { get; init; }
}

/// <summary>
/// Detailed tracking information for a waybill.
/// </summary>
public record ParcelTrackingDetails
{
    /// <summary>
    /// Tracking statuses in chronological order.
    /// </summary>
    [JsonPropertyName("statuses")]
    public List<ParcelTrackingStatus>? Statuses { get; init; }

    /// <summary>
    /// Date the tracking record was created (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Date the tracking record was last updated (ISO 8601).
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// A single parcel tracking status entry.
/// </summary>
public record ParcelTrackingStatus
{
    /// <summary>
    /// Date and time the status occurred (ISO 8601).
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Status code (AVAILABLE_FOR_PICKUP, DELIVERED, IN_TRANSIT, ISSUE, NOTICE_LEFT, PENDING, RELEASED_FOR_DELIVERY, RETURNED).
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// Human readable status description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Response listing Allegro pickup/drop-off points.
/// </summary>
public record AllegroPickupDropOffPointsResponse
{
    /// <summary>
    /// Pickup/drop-off points.
    /// </summary>
    [JsonPropertyName("points")]
    public List<AllegroPickupDropOffPoint>? Points { get; init; }
}

/// <summary>
/// An Allegro pickup/drop-off point.
/// </summary>
public record AllegroPickupDropOffPoint
{
    /// <summary>
    /// Point identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Point name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Point type: PUDO or APM.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Point description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Point address.
    /// </summary>
    [JsonPropertyName("address")]
    public PickupPointAddress? Address { get; init; }
}

/// <summary>
/// Address of a pickup/drop-off point.
/// </summary>
public record PickupPointAddress
{
    /// <summary>
    /// Postal code.
    /// </summary>
    [JsonPropertyName("postCode")]
    public string? PostCode { get; init; }

    /// <summary>
    /// City.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Street.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// Country code.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }

    /// <summary>
    /// Geographic coordinates.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public PickupPointCoordinates? Coordinates { get; init; }
}

/// <summary>
/// Geographic coordinates of a pickup/drop-off point.
/// </summary>
public record PickupPointCoordinates
{
    /// <summary>
    /// Latitude.
    /// </summary>
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }

    /// <summary>
    /// Longitude.
    /// </summary>
    [JsonPropertyName("lon")]
    public double? Lon { get; init; }
}

/// <summary>
/// Request body for uploading a URL to an order billing document.
/// </summary>
public record NewOrderBillingDocumentLink
{
    /// <summary>
    /// URL to the billing document.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}
