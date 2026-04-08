using System.Text.Json.Serialization;

namespace AllegroApi.Models.ShipmentManagement;

/// <summary>
/// Request for shipment labels.
/// </summary>
public record LabelRequestDto
{
    /// <summary>
    /// List of shipment identifiers to generate labels for.
    /// </summary>
    [JsonPropertyName("shipmentIds")]
    public List<string>? ShipmentIds { get; init; }

    /// <summary>
    /// Label format (e.g., "PDF", "ZPL").
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; init; }
}

/// <summary>
/// Request for shipment protocol containing multiple shipments.
/// </summary>
public record ShipmentIdsDto
{
    /// <summary>
    /// List of shipment identifiers to include in the protocol.
    /// </summary>
    [JsonPropertyName("shipmentIds")]
    public List<string>? ShipmentIds { get; init; }
}
