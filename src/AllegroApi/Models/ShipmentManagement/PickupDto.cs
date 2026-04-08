using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Request for pickup date proposals.
/// </summary>
public record PickupProposalsRequestDto
{
    /// <summary>
    /// List of shipment identifiers for pickup.
    /// </summary>
    [JsonPropertyName("shipmentIds")]
    public List<string>? ShipmentIds { get; init; }

    /// <summary>
    /// Address where pickup should occur.
    /// </summary>
    [JsonPropertyName("address")]
    public ShipmentAddressDto? Address { get; init; }
}

/// <summary>
/// Response containing pickup date proposals.
/// </summary>
public record PickupProposalsResponseDto
{
    /// <summary>
    /// Proposed pickup date.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; init; }

    /// <summary>
    /// Time slot for pickup.
    /// </summary>
    [JsonPropertyName("timeSlot")]
    public TimeSlotDto? TimeSlot { get; init; }
}

/// <summary>
/// Time slot information.
/// </summary>
public record TimeSlotDto
{
    /// <summary>
    /// Start time of the time slot.
    /// </summary>
    [JsonPropertyName("from")]
    public string? From { get; init; }

    /// <summary>
    /// End time of the time slot.
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; init; }
}

/// <summary>
/// Command to create a pickup request.
/// </summary>
public record PickupCreateCommandDto
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// List of shipment identifiers to be picked up.
    /// </summary>
    [JsonPropertyName("shipmentIds")]
    public List<string>? ShipmentIds { get; init; }

    /// <summary>
    /// Requested pickup date.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; init; }

    /// <summary>
    /// Requested time slot.
    /// </summary>
    [JsonPropertyName("timeSlot")]
    public TimeSlotDto? TimeSlot { get; init; }

    /// <summary>
    /// Pickup address.
    /// </summary>
    [JsonPropertyName("address")]
    public ShipmentAddressDto? Address { get; init; }
}

/// <summary>
/// Status of pickup creation command.
/// </summary>
public record CreatePickupCommandStatusDto
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
    /// Created pickup identifier (available when status is SUCCESS).
    /// </summary>
    [JsonPropertyName("pickupId")]
    public string? PickupId { get; init; }

    /// <summary>
    /// Error details (if status is FAILED).
    /// </summary>
    [JsonPropertyName("errors")]
    public List<ErrorDto>? Errors { get; init; }
}

/// <summary>
/// Pickup details.
/// </summary>
public record PickupDto
{
    /// <summary>
    /// Pickup identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// List of shipment identifiers in this pickup.
    /// </summary>
    [JsonPropertyName("shipmentIds")]
    public List<string>? ShipmentIds { get; init; }

    /// <summary>
    /// Scheduled pickup date.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; init; }

    /// <summary>
    /// Time slot for pickup.
    /// </summary>
    [JsonPropertyName("timeSlot")]
    public TimeSlotDto? TimeSlot { get; init; }

    /// <summary>
    /// Pickup address.
    /// </summary>
    [JsonPropertyName("address")]
    public ShipmentAddressDto? Address { get; init; }

    /// <summary>
    /// Pickup status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Creation timestamp.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}
