using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Available delivery options for an order, with a suggested shipment input.
/// </summary>
public record DeliveryProposalDto
{
    /// <summary>
    /// Order identifier.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Suggested shipment creation input pre-filled from the order.
    /// </summary>
    [JsonPropertyName("suggestedInput")]
    public ShipmentCreateRequestDto? SuggestedInput { get; init; }

    /// <summary>
    /// Available delivery options.
    /// </summary>
    [JsonPropertyName("deliveryOptions")]
    public List<DeliveryOptionDto>? DeliveryOptions { get; init; }
}

/// <summary>
/// Shipment creation request.
/// </summary>
public record ShipmentCreateRequestDto
{
    /// <summary>Delivery method identifier.</summary>
    [JsonPropertyName("deliveryMethodId")]
    public string? DeliveryMethodId { get; init; }

    /// <summary>Carrier credentials identifier.</summary>
    [JsonPropertyName("credentialsId")]
    public string? CredentialsId { get; init; }

    /// <summary>Sender address.</summary>
    [JsonPropertyName("sender")]
    public ShipmentProposalAddressDto? Sender { get; init; }

    /// <summary>Receiver address.</summary>
    [JsonPropertyName("receiver")]
    public ShipmentProposalAddressDto? Receiver { get; init; }

    /// <summary>Reference number.</summary>
    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; init; }

    /// <summary>Packages.</summary>
    [JsonPropertyName("packages")]
    public List<PackageRequestDto>? Packages { get; init; }

    /// <summary>Insurance.</summary>
    [JsonPropertyName("insurance")]
    public ShipmentMoneyDto? Insurance { get; init; }

    /// <summary>Cash on delivery.</summary>
    [JsonPropertyName("cashOnDelivery")]
    public CashOnDeliveryDto? CashOnDelivery { get; init; }

    /// <summary>Label format: PDF or ZPL.</summary>
    [JsonPropertyName("labelFormat")]
    public string? LabelFormat { get; init; }

    /// <summary>Additional service identifiers.</summary>
    [JsonPropertyName("additionalServices")]
    public List<string>? AdditionalServices { get; init; }
}

/// <summary>
/// Address used by shipment delivery proposals (with optional pickup point).
/// </summary>
public record ShipmentProposalAddressDto
{
    /// <summary>Name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Company.</summary>
    [JsonPropertyName("company")]
    public string? Company { get; init; }

    /// <summary>Street.</summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>Postal code.</summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; init; }

    /// <summary>City.</summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>State / region.</summary>
    [JsonPropertyName("state")]
    public string? State { get; init; }

    /// <summary>Country code.</summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }

    /// <summary>Email.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>Phone.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    /// <summary>Pickup point identifier, when applicable.</summary>
    [JsonPropertyName("point")]
    public string? Point { get; init; }
}

/// <summary>
/// A package within a shipment request.
/// </summary>
public record PackageRequestDto
{
    /// <summary>Package type: DOX, PACKAGE, PALLET or OTHER.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Length.</summary>
    [JsonPropertyName("length")]
    public DimensionValue? Length { get; init; }

    /// <summary>Width.</summary>
    [JsonPropertyName("width")]
    public DimensionValue? Width { get; init; }

    /// <summary>Height.</summary>
    [JsonPropertyName("height")]
    public DimensionValue? Height { get; init; }

    /// <summary>Weight.</summary>
    [JsonPropertyName("weight")]
    public WeightValue? Weight { get; init; }

    /// <summary>Text printed on the label.</summary>
    [JsonPropertyName("textOnLabel")]
    public string? TextOnLabel { get; init; }
}

/// <summary>A dimension value with unit.</summary>
public record DimensionValue
{
    /// <summary>Numeric value.</summary>
    [JsonPropertyName("value")]
    public decimal? Value { get; init; }

    /// <summary>Unit (CENTIMETER).</summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

/// <summary>A weight value with unit.</summary>
public record WeightValue
{
    /// <summary>Numeric value.</summary>
    [JsonPropertyName("value")]
    public decimal? Value { get; init; }

    /// <summary>Unit (KILOGRAMS).</summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

/// <summary>Cash-on-delivery configuration.</summary>
public record CashOnDeliveryDto
{
    /// <summary>Amount.</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>Currency.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>Account owner name.</summary>
    [JsonPropertyName("ownerName")]
    public string? OwnerName { get; init; }

    /// <summary>IBAN.</summary>
    [JsonPropertyName("iban")]
    public string? Iban { get; init; }
}

/// <summary>Monetary amount used by shipment delivery proposals.</summary>
public record ShipmentMoneyDto
{
    /// <summary>Amount.</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>Currency.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// An available delivery option for an order.
/// </summary>
public record DeliveryOptionDto
{
    /// <summary>Delivery type: DOOR, APM or PUDO.</summary>
    [JsonPropertyName("deliveryType")]
    public string? DeliveryType { get; init; }

    /// <summary>Payment type: PREPAID or POSTPAID.</summary>
    [JsonPropertyName("paymentType")]
    public string? PaymentType { get; init; }

    /// <summary>Package type: DOX, PACKAGE, PALLET or OTHER.</summary>
    [JsonPropertyName("packageType")]
    public string? PackageType { get; init; }

    /// <summary>Origin country code.</summary>
    [JsonPropertyName("originCountry")]
    public string? OriginCountry { get; init; }

    /// <summary>Destination country code.</summary>
    [JsonPropertyName("destinationCountry")]
    public string? DestinationCountry { get; init; }

    /// <summary>Limits for this delivery option.</summary>
    [JsonPropertyName("limits")]
    public DeliveryOptionLimitsDto? Limits { get; init; }

    /// <summary>Additional services available for this option.</summary>
    [JsonPropertyName("additionalServices")]
    public List<DeliveryAdditionalServiceDto>? AdditionalServices { get; init; }

    /// <summary>Additional properties for this option.</summary>
    [JsonPropertyName("additionalProperties")]
    public List<DeliveryAdditionalPropertyDto>? AdditionalProperties { get; init; }
}

/// <summary>Limits for a delivery option.</summary>
public record DeliveryOptionLimitsDto
{
    /// <summary>Maximum cash-on-delivery amount.</summary>
    [JsonPropertyName("cashOnDelivery")]
    public ShipmentMoneyDto? CashOnDelivery { get; init; }

    /// <summary>Maximum insured amount.</summary>
    [JsonPropertyName("insurance")]
    public ShipmentMoneyDto? Insurance { get; init; }

    /// <summary>Maximum weight.</summary>
    [JsonPropertyName("weight")]
    public WeightValue? Weight { get; init; }
}

/// <summary>An additional service available for a delivery option.</summary>
public record DeliveryAdditionalServiceDto
{
    /// <summary>Service identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Service name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Service description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>An additional property for a delivery option.</summary>
public record DeliveryAdditionalPropertyDto
{
    /// <summary>Property identifier.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Property name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Property description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Whether the property is required.</summary>
    [JsonPropertyName("required")]
    public bool? Required { get; init; }

    /// <summary>Whether the property is read-only.</summary>
    [JsonPropertyName("readOnly")]
    public bool? ReadOnly { get; init; }
}
