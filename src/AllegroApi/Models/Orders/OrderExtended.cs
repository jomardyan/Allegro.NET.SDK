using System.Text.Json.Serialization;

namespace AllegroApi.Models.Orders;

/// <summary>
/// Response containing order event statistics.
/// </summary>
public record OrderEventStatsResponse
{
    /// <summary>
    /// Latest event information.
    /// </summary>
    [JsonPropertyName("latestEvent")]
    public LatestOrderEvent? LatestEvent { get; init; }
}

/// <summary>
/// Latest order event details.
/// </summary>
public record LatestOrderEvent
{
    /// <summary>
    /// Event identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the event occurred.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime OccurredAt { get; init; }
}

/// <summary>
/// Response containing available shipping carriers.
/// </summary>
public record OrdersShippingCarriersResponse
{
    /// <summary>
    /// List of shipping carriers.
    /// </summary>
    [JsonPropertyName("carriers")]
    public List<OrdersShippingCarrier>? Carriers { get; init; }
}

/// <summary>
/// Shipping carrier information.
/// </summary>
public record OrdersShippingCarrier
{
    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Carrier name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Carrier tracking URL template.
    /// </summary>
    [JsonPropertyName("trackingUrl")]
    public string? TrackingUrl { get; init; }
}


