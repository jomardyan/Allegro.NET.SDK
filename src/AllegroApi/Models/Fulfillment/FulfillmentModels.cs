using System.Text.Json.Serialization;

namespace AllegroApi.Models.Fulfillment;

/// <summary>
/// Represents a list of Advance Ship Notices.
/// </summary>
public record AdvanceShipNoticeList
{
    /// <summary>
    /// Gets the list of Advance Ship Notices.
    /// </summary>
    [JsonPropertyName("advanceShipNotices")]
    public List<AdvanceShipNoticeListItem> AdvanceShipNotices { get; init; } = new();

    /// <summary>
    /// Gets the number of Advance Ship Notices in response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>
    /// Gets the total number of Advance Ship Notices.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}

/// <summary>
/// Represents an Advance Ship Notice list item.
/// </summary>
public record AdvanceShipNoticeListItem
{
    /// <summary>
    /// Gets the UUID identifier of ASN.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the human-friendly identifier of ASN.
    /// </summary>
    [JsonPropertyName("displayNumber")]
    public string DisplayNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the Advance Ship Notice status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date and time of Advance Ship Notice creation in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the date and time of last Advance Ship Notice update in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Gets the list of product items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ProductItem> Items { get; init; } = new();

    /// <summary>
    /// Gets the handling unit information.
    /// </summary>
    [JsonPropertyName("handlingUnit")]
    public HandlingUnit? HandlingUnit { get; init; }

    /// <summary>
    /// Gets the labels information.
    /// </summary>
    [JsonPropertyName("labels")]
    public Labels? Labels { get; init; }

    /// <summary>
    /// Gets the shipping information.
    /// </summary>
    [JsonPropertyName("shipping")]
    public ShippingExtended? Shipping { get; init; }

    /// <summary>
    /// Gets the date and time of Advance Ship Notice submission in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("submittedAt")]
    public DateTime? SubmittedAt { get; init; }
}

/// <summary>
/// Represents a full Advance Ship Notice response.
/// </summary>
public record AdvanceShipNoticeResponse
{
    /// <summary>
    /// Gets the UUID identifier of ASN.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the human-friendly identifier of ASN.
    /// </summary>
    [JsonPropertyName("displayNumber")]
    public string DisplayNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the Advance Ship Notice status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date and time of Advance Ship Notice creation in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the date and time of last Advance Ship Notice update in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Gets the list of product items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ProductItem> Items { get; init; } = new();

    /// <summary>
    /// Gets the handling unit information.
    /// </summary>
    [JsonPropertyName("handlingUnit")]
    public HandlingUnit? HandlingUnit { get; init; }

    /// <summary>
    /// Gets the labels information.
    /// </summary>
    [JsonPropertyName("labels")]
    public Labels? Labels { get; init; }

    /// <summary>
    /// Gets the shipping information.
    /// </summary>
    [JsonPropertyName("shipping")]
    public ShippingExtended? Shipping { get; init; }

    /// <summary>
    /// Gets the date and time of Advance Ship Notice submission in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("submittedAt")]
    public DateTime? SubmittedAt { get; init; }

    /// <summary>
    /// Gets the volume in cubic centimeters.
    /// </summary>
    [JsonPropertyName("volumeInCc")]
    public AsnVolume? VolumeInCc { get; init; }
}

/// <summary>
/// Represents a request to create an Advance Ship Notice.
/// </summary>
public record CreateAdvanceShipNoticeRequest
{
    /// <summary>
    /// Gets or sets the list of product items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ProductItem> Items { get; init; } = new();

    /// <summary>
    /// Gets or sets the handling unit information.
    /// </summary>
    [JsonPropertyName("handlingUnit")]
    public HandlingUnit? HandlingUnit { get; init; }

    /// <summary>
    /// Gets or sets the shipping information.
    /// </summary>
    [JsonPropertyName("shipping")]
    public ShippingExtended? Shipping { get; init; }
}

/// <summary>
/// Represents a request to update a submitted Advance Ship Notice.
/// </summary>
public record UpdateSubmittedAdvanceShipNoticeRequest
{
    /// <summary>
    /// Gets or sets the handling unit information.
    /// </summary>
    [JsonPropertyName("handlingUnit")]
    public HandlingUnit? HandlingUnit { get; init; }

    /// <summary>
    /// Gets or sets the shipping information.
    /// </summary>
    [JsonPropertyName("shipping")]
    public ShippingExtended? Shipping { get; init; }
}

/// <summary>
/// Represents a product item with quantity.
/// </summary>
public record ProductItem
{
    /// <summary>
    /// Gets or sets the product information.
    /// </summary>
    [JsonPropertyName("product")]
    public Product Product { get; init; } = new();

    /// <summary>
    /// Gets or sets the quantity of the product (1-1000000).
    /// </summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}

/// <summary>
/// Represents product data.
/// </summary>
public record Product
{
    /// <summary>
    /// Gets or sets the product identifier (UUID format).
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}

/// <summary>
/// Represents handling unit information.
/// </summary>
public record HandlingUnit
{
    /// <summary>
    /// Gets or sets the unit type (BOX, PALLET, CONTAINER).
    /// </summary>
    [JsonPropertyName("unitType")]
    public string? UnitType { get; init; }

    /// <summary>
    /// Gets or sets the amount of units.
    /// </summary>
    [JsonPropertyName("amount")]
    public int? Amount { get; init; }
}

/// <summary>
/// Represents labels information.
/// </summary>
public record Labels
{
    /// <summary>
    /// Gets the status of labels.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>
/// Represents extended shipping information.
/// </summary>
public record ShippingExtended
{
    /// <summary>
    /// Gets or sets the carrier name.
    /// </summary>
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; init; }

    /// <summary>
    /// Gets or sets the tracking number.
    /// </summary>
    [JsonPropertyName("trackingNumber")]
    public string? TrackingNumber { get; init; }

    /// <summary>
    /// Gets or sets the estimated delivery date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("estimatedDeliveryAt")]
    public DateTime? EstimatedDeliveryAt { get; init; }
}

/// <summary>
/// Represents ASN volume information.
/// </summary>
public record AsnVolume
{
    /// <summary>
    /// Gets the volume value in cubic centimeters.
    /// </summary>
    [JsonPropertyName("value")]
    public decimal? Value { get; init; }
}

/// <summary>
/// Represents a submit command for Advance Ship Notice.
/// </summary>
public record SubmitCommand
{
    /// <summary>
    /// Gets or sets the identifier of the command (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets or sets the command input.
    /// </summary>
    [JsonPropertyName("input")]
    public SubmitCommandInput Input { get; init; } = new();

    /// <summary>
    /// Gets the command output.
    /// </summary>
    [JsonPropertyName("output")]
    public SubmitCommandOutput? Output { get; init; }
}

/// <summary>
/// Represents input for the submit command.
/// </summary>
public record SubmitCommandInput
{
    /// <summary>
    /// Gets or sets the Advance Ship Notice identifier.
    /// </summary>
    [JsonPropertyName("advanceShipNoticeId")]
    public string AdvanceShipNoticeId { get; init; } = string.Empty;
}

/// <summary>
/// Represents output from the submit command.
/// </summary>
public record SubmitCommandOutput
{
    /// <summary>
    /// Gets the command status (RUNNING, SUCCESSFUL, FAILED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the errors if command failed.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<SubmitCommandError>? Errors { get; init; }
}

/// <summary>
/// Represents an error from the submit command.
/// </summary>
public record SubmitCommandError
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets the error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}

/// <summary>
/// Represents the receiving state of an Advance Ship Notice.
/// </summary>
public record ReceivingState
{
    /// <summary>
    /// Gets the receiving progress percentage.
    /// </summary>
    [JsonPropertyName("progress")]
    public int? Progress { get; init; }

    /// <summary>
    /// Gets the list of received items with details.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ReceivingStateItem>? Items { get; init; }
}

/// <summary>
/// Represents a received item in the receiving state.
/// </summary>
public record ReceivingStateItem
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; init; }

    /// <summary>
    /// Gets the quantity received.
    /// </summary>
    [JsonPropertyName("quantityReceived")]
    public int? QuantityReceived { get; init; }

    /// <summary>
    /// Gets the quantity expected.
    /// </summary>
    [JsonPropertyName("quantityExpected")]
    public int? QuantityExpected { get; init; }

    /// <summary>
    /// Gets the condition of the item.
    /// </summary>
    [JsonPropertyName("condition")]
    public string? Condition { get; init; }
}

/// <summary>
/// Represents a list of stock products.
/// </summary>
public record StockProductList
{
    /// <summary>
    /// Gets the list of stock products.
    /// </summary>
    [JsonPropertyName("products")]
    public List<StockProduct> Products { get; init; } = new();

    /// <summary>
    /// Gets the number of products in response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>
    /// Gets the total number of products.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}

/// <summary>
/// Represents a stock product.
/// </summary>
public record StockProduct
{
    /// <summary>
    /// Gets the product identifier (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the GTIN (Global Trade Item Number).
    /// </summary>
    [JsonPropertyName("gtin")]
    public string? Gtin { get; init; }

    /// <summary>
    /// Gets the offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Gets the total quantity.
    /// </summary>
    [JsonPropertyName("totalQuantity")]
    public int? TotalQuantity { get; init; }

    /// <summary>
    /// Gets the available quantity.
    /// </summary>
    [JsonPropertyName("available")]
    public int? Available { get; init; }

    /// <summary>
    /// Gets the reserved quantity.
    /// </summary>
    [JsonPropertyName("reserved")]
    public int? Reserved { get; init; }

    /// <summary>
    /// Gets the unfulfillable quantity.
    /// </summary>
    [JsonPropertyName("unfulfillable")]
    public int? Unfulfillable { get; init; }

    /// <summary>
    /// Gets the sales statistics.
    /// </summary>
    [JsonPropertyName("salesStats")]
    public SalesStats? SalesStats { get; init; }

    /// <summary>
    /// Gets the reserve information.
    /// </summary>
    [JsonPropertyName("reserve")]
    public ReserveInfo? Reserve { get; init; }

    /// <summary>
    /// Gets the storage fee information.
    /// </summary>
    [JsonPropertyName("storageFee")]
    public StorageFee? StorageFee { get; init; }
}

/// <summary>
/// Represents sales statistics.
/// </summary>
public record SalesStats
{
    /// <summary>
    /// Gets the sales in last 30 days.
    /// </summary>
    [JsonPropertyName("lastThirtyDaysSalesSum")]
    public int? LastThirtyDaysSalesSum { get; init; }

    /// <summary>
    /// Gets the average daily sales.
    /// </summary>
    [JsonPropertyName("lastWeekSalesAverage")]
    public decimal? LastWeekSalesAverage { get; init; }
}

/// <summary>
/// Represents reserve information.
/// </summary>
public record ReserveInfo
{
    /// <summary>
    /// Gets the number of days until out of stock.
    /// </summary>
    [JsonPropertyName("outOfStockIn")]
    public int? OutOfStockIn { get; init; }
}

/// <summary>
/// Represents storage fee information.
/// </summary>
public record StorageFee
{
    /// <summary>
    /// Gets the fee status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Gets the net amount.
    /// </summary>
    [JsonPropertyName("amountNet")]
    public decimal? AmountNet { get; init; }

    /// <summary>
    /// Gets the gross amount.
    /// </summary>
    [JsonPropertyName("amountGross")]
    public decimal? AmountGross { get; init; }

    /// <summary>
    /// Gets the number of items with storage fee.
    /// </summary>
    [JsonPropertyName("itemCount")]
    public int? ItemCount { get; init; }

    /// <summary>
    /// Gets the fee charge date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("chargeDate")]
    public DateTime? ChargeDate { get; init; }
}

/// <summary>
/// Represents a fulfillment order with parcels.
/// </summary>
public record FulfillmentOrder
{
    /// <summary>
    /// Gets the order identifier.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string OrderId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of parcels.
    /// </summary>
    [JsonPropertyName("parcels")]
    public List<FulfillmentParcel> Parcels { get; init; } = new();
}

/// <summary>
/// Represents a fulfillment parcel.
/// </summary>
public record FulfillmentParcel
{
    /// <summary>
    /// Gets the parcel identifier.
    /// </summary>
    [JsonPropertyName("parcelId")]
    public string? ParcelId { get; init; }

    /// <summary>
    /// Gets the tracking number.
    /// </summary>
    [JsonPropertyName("trackingNumber")]
    public string? TrackingNumber { get; init; }

    /// <summary>
    /// Gets the carrier name.
    /// </summary>
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; init; }

    /// <summary>
    /// Gets the list of items in the parcel.
    /// </summary>
    [JsonPropertyName("items")]
    public List<FulfillmentParcelItem>? Items { get; init; }
}

/// <summary>
/// Represents an item in a fulfillment parcel.
/// </summary>
public record FulfillmentParcelItem
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; init; }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }

    /// <summary>
    /// Gets the expiration date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public DateTime? ExpirationDate { get; init; }

    /// <summary>
    /// Gets the serial number.
    /// </summary>
    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; init; }
}

/// <summary>
/// Represents a list of available products for ASN.
/// </summary>
public record AvailableProductsList
{
    /// <summary>
    /// Gets the list of available products.
    /// </summary>
    [JsonPropertyName("products")]
    public List<AvailableProduct> Products { get; init; } = new();

    /// <summary>
    /// Gets the number of products in response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>
    /// Gets the total number of products.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}

/// <summary>
/// Represents an available product.
/// </summary>
public record AvailableProduct
{
    /// <summary>
    /// Gets the product identifier (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the GTIN.
    /// </summary>
    [JsonPropertyName("gtin")]
    public string? Gtin { get; init; }
}

/// <summary>
/// Represents a tax identification number request.
/// </summary>
public record TaxIdRequest
{
    /// <summary>
    /// Gets or sets the tax identification number.
    /// </summary>
    [JsonPropertyName("taxId")]
    public string TaxId { get; init; } = string.Empty;
}

/// <summary>
/// Represents a tax identification number response.
/// </summary>
public record TaxIdResponse
{
    /// <summary>
    /// Gets the tax identification number.
    /// </summary>
    [JsonPropertyName("taxId")]
    public string TaxId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public string VerificationStatus { get; init; } = string.Empty;
}

/// <summary>
/// Represents fulfillment removal preference.
/// </summary>
public record FulfillmentRemovalPreference
{
    /// <summary>
    /// Gets or sets the removal type (e.g., RETURN, DESTROY).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the delivery address for returned items.
    /// </summary>
    [JsonPropertyName("deliveryAddress")]
    public RemovalDeliveryAddress? DeliveryAddress { get; init; }
}

/// <summary>
/// Represents a delivery address for removal.
/// </summary>
public record RemovalDeliveryAddress
{
    /// <summary>
    /// Gets or sets the street name.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    [JsonPropertyName("postCode")]
    public string? PostCode { get; init; }

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}
