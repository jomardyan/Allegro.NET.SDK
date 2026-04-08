using System.Text.Json.Serialization;

namespace AllegroApi.Models.PointsOfService;

/// <summary>
/// List of points of service.
/// </summary>
public record PointsOfServiceList
{
    /// <summary>
    /// Collection of points of service.
    /// </summary>
    [JsonPropertyName("pointsOfService")]
    public List<PointOfService>? PointsOfService { get; init; }
}

/// <summary>
/// Point of service details (pickup location).
/// </summary>
public record PointOfService
{
    /// <summary>
    /// Point of service identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// External identifier from seller's system.
    /// </summary>
    [JsonPropertyName("externalId")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Point of service name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Seller identifier.
    /// </summary>
    [JsonPropertyName("seller")]
    public SellerReference? Seller { get; init; }

    /// <summary>
    /// Point of service type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Address details.
    /// </summary>
    [JsonPropertyName("address")]
    public Address? Address { get; init; }

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
    /// Opening hours schedule.
    /// </summary>
    [JsonPropertyName("openingHours")]
    public OpeningHours? OpeningHours { get; init; }

    /// <summary>
    /// Service time in minutes.
    /// </summary>
    [JsonPropertyName("serviceTime")]
    public int? ServiceTime { get; init; }

    /// <summary>
    /// Payment methods accepted.
    /// </summary>
    [JsonPropertyName("payments")]
    public List<string>? Payments { get; init; }

    /// <summary>
    /// Confirmation type required.
    /// </summary>
    [JsonPropertyName("confirmationRequired")]
    public bool? ConfirmationRequired { get; init; }
}

/// <summary>
/// Seller reference.
/// </summary>
public record SellerReference
{
    /// <summary>
    /// Seller identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Address information.
/// </summary>
public record Address
{
    /// <summary>
    /// Street name.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// City name.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Postal code.
    /// </summary>
    [JsonPropertyName("postCode")]
    public string? PostCode { get; init; }

    /// <summary>
    /// Country code (ISO 3166-1 alpha-2).
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }

    /// <summary>
    /// Province/state.
    /// </summary>
    [JsonPropertyName("province")]
    public string? Province { get; init; }

    /// <summary>
    /// Geographic coordinates.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public Coordinates? Coordinates { get; init; }
}

/// <summary>
/// Geographic coordinates.
/// </summary>
public record Coordinates
{
    /// <summary>
    /// Latitude.
    /// </summary>
    [JsonPropertyName("latitude")]
    public decimal? Latitude { get; init; }

    /// <summary>
    /// Longitude.
    /// </summary>
    [JsonPropertyName("longitude")]
    public decimal? Longitude { get; init; }
}

/// <summary>
/// Opening hours schedule.
/// </summary>
public record OpeningHours
{
    /// <summary>
    /// Monday hours.
    /// </summary>
    [JsonPropertyName("MONDAY")]
    public List<TimeRange>? Monday { get; init; }

    /// <summary>
    /// Tuesday hours.
    /// </summary>
    [JsonPropertyName("TUESDAY")]
    public List<TimeRange>? Tuesday { get; init; }

    /// <summary>
    /// Wednesday hours.
    /// </summary>
    [JsonPropertyName("WEDNESDAY")]
    public List<TimeRange>? Wednesday { get; init; }

    /// <summary>
    /// Thursday hours.
    /// </summary>
    [JsonPropertyName("THURSDAY")]
    public List<TimeRange>? Thursday { get; init; }

    /// <summary>
    /// Friday hours.
    /// </summary>
    [JsonPropertyName("FRIDAY")]
    public List<TimeRange>? Friday { get; init; }

    /// <summary>
    /// Saturday hours.
    /// </summary>
    [JsonPropertyName("SATURDAY")]
    public List<TimeRange>? Saturday { get; init; }

    /// <summary>
    /// Sunday hours.
    /// </summary>
    [JsonPropertyName("SUNDAY")]
    public List<TimeRange>? Sunday { get; init; }
}

/// <summary>
/// Time range for opening hours.
/// </summary>
public record TimeRange
{
    /// <summary>
    /// Start time (HH:mm format).
    /// </summary>
    [JsonPropertyName("from")]
    public string? From { get; init; }

    /// <summary>
    /// End time (HH:mm format).
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; init; }
}
