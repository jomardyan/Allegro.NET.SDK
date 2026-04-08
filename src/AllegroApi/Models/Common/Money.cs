using System.Text.Json.Serialization;

namespace AllegroApi.Models.Common;

/// <summary>
/// Represents a monetary amount with currency
/// </summary>
public class Money
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; } = "0";

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "PLN";

    public decimal AmountValue => decimal.TryParse(Amount, out var value) ? value : 0;
}
