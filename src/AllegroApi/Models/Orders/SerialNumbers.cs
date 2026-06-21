using System.Text.Json.Serialization;

namespace AllegroApi.Models.Orders;

/// <summary>
/// Request body for setting serial numbers on a checkout form's line items.
/// </summary>
public record CheckoutFormLineItemsSetSerialNumbersRequest
{
    /// <summary>
    /// Line items with their serial numbers.
    /// </summary>
    [JsonPropertyName("lineItems")]
    public List<CheckoutFormLineItemSetSerialNumbersRequest>? LineItems { get; init; }
}

/// <summary>
/// Serial numbers for a single line item.
/// </summary>
public record CheckoutFormLineItemSetSerialNumbersRequest
{
    /// <summary>
    /// Line item identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Serial number entries for the line item.
    /// </summary>
    [JsonPropertyName("serialNumbers")]
    public CheckoutFormLineItemSetSerialNumbersEntries? SerialNumbers { get; init; }
}

/// <summary>
/// Collection of serial number entries.
/// </summary>
public record CheckoutFormLineItemSetSerialNumbersEntries
{
    /// <summary>
    /// Serial number entries.
    /// </summary>
    [JsonPropertyName("entries")]
    public List<CheckoutFormLineItemSerialNumberEntry>? Entries { get; init; }
}

/// <summary>
/// A single serial number value.
/// </summary>
public record CheckoutFormLineItemSerialNumberEntry
{
    /// <summary>
    /// Serial number value.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }
}
