using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Command to create a new shipment.
/// </summary>
public record ShipmentCreateCommandDto
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Checkout form identifier associated with the shipment.
    /// </summary>
    [JsonPropertyName("checkoutFormId")]
    public string? CheckoutFormId { get; init; }

    /// <summary>
    /// Delivery service identifier.
    /// </summary>
    [JsonPropertyName("deliveryServiceId")]
    public string? DeliveryServiceId { get; init; }

    /// <summary>
    /// Sender information.
    /// </summary>
    [JsonPropertyName("sender")]
    public ShipmentAddressDto? Sender { get; init; }

    /// <summary>
    /// Receiver information.
    /// </summary>
    [JsonPropertyName("receiver")]
    public ShipmentAddressDto? Receiver { get; init; }

    /// <summary>
    /// Parcels included in the shipment.
    /// </summary>
    [JsonPropertyName("parcels")]
    public List<ParcelDto>? Parcels { get; init; }
}

/// <summary>
/// Address information for shipment sender or receiver.
/// </summary>
public record ShipmentAddressDto
{
    /// <summary>
    /// First name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    /// <summary>
    /// Company name (optional).
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Street name.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// Building number.
    /// </summary>
    [JsonPropertyName("buildingNumber")]
    public string? BuildingNumber { get; init; }

    /// <summary>
    /// Apartment number (optional).
    /// </summary>
    [JsonPropertyName("apartmentNumber")]
    public string? ApartmentNumber { get; init; }

    /// <summary>
    /// City name.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Postal code.
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; init; }

    /// <summary>
    /// Country code (ISO 3166-1 alpha-2).
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}

/// <summary>
/// Parcel information.
/// </summary>
public record ParcelDto
{
    /// <summary>
    /// Parcel weight in kilograms.
    /// </summary>
    [JsonPropertyName("weight")]
    public decimal? Weight { get; init; }

    /// <summary>
    /// Parcel dimensions.
    /// </summary>
    [JsonPropertyName("dimensions")]
    public ParcelDimensionsDto? Dimensions { get; init; }
}

/// <summary>
/// Parcel dimensions.
/// </summary>
public record ParcelDimensionsDto
{
    /// <summary>
    /// Length in centimeters.
    /// </summary>
    [JsonPropertyName("length")]
    public decimal? Length { get; init; }

    /// <summary>
    /// Width in centimeters.
    /// </summary>
    [JsonPropertyName("width")]
    public decimal? Width { get; init; }

    /// <summary>
    /// Height in centimeters.
    /// </summary>
    [JsonPropertyName("height")]
    public decimal? Height { get; init; }
}

/// <summary>
/// Status of shipment creation command.
/// </summary>
public record CreateShipmentCommandStatusDto
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Command status (e.g., "PENDING", "SUCCESS", "FAILED").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Created shipment identifier (available when status is SUCCESS).
    /// </summary>
    [JsonPropertyName("shipmentId")]
    public string? ShipmentId { get; init; }

    /// <summary>
    /// Error details (if status is FAILED).
    /// </summary>
    [JsonPropertyName("errors")]
    public List<ErrorDto>? Errors { get; init; }
}

/// <summary>
/// Error information.
/// </summary>
public record ErrorDto
{
    /// <summary>
    /// Error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// Error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// Path to the field that caused the error (if applicable).
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; init; }
}
