using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Command to cancel a shipment.
/// </summary>
public record ShipmentCancelCommandDto
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Shipment identifier to cancel.
    /// </summary>
    [JsonPropertyName("shipmentId")]
    public string? ShipmentId { get; init; }
}

/// <summary>
/// Status of shipment cancellation command.
/// </summary>
public record CancelShipmentCommandStatusDto
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
    /// Error details (if status is FAILED).
    /// </summary>
    [JsonPropertyName("errors")]
    public List<ErrorDto>? Errors { get; init; }
}

/// <summary>
/// Full shipment details.
/// </summary>
public record ShipmentDetailsDto
{
    /// <summary>
    /// Shipment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Tracking number.
    /// </summary>
    [JsonPropertyName("trackingNumber")]
    public string? TrackingNumber { get; init; }

    /// <summary>
    /// Delivery service used.
    /// </summary>
    [JsonPropertyName("deliveryService")]
    public DeliveryServiceDto? DeliveryService { get; init; }

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
    /// Parcels in the shipment.
    /// </summary>
    [JsonPropertyName("parcels")]
    public List<ParcelDto>? Parcels { get; init; }

    /// <summary>
    /// Shipment status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Creation date and time.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}
