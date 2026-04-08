using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Response containing available delivery services.
/// </summary>
public record DeliveryServicesDto
{
    /// <summary>
    /// List of available delivery services.
    /// </summary>
    [JsonPropertyName("deliveryServices")]
    public List<DeliveryServiceDto>? DeliveryServices { get; init; }
}

/// <summary>
/// Represents a single delivery service.
/// </summary>
public record DeliveryServiceDto
{
    /// <summary>
    /// Delivery service identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Delivery service name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Carrier providing the service.
    /// </summary>
    [JsonPropertyName("carrier")]
    public string? Carrier { get; init; }

    /// <summary>
    /// Service type (e.g., "PARCEL").
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
