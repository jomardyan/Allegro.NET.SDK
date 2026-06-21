using System.Text.Json.Serialization;

namespace AllegroApi.Models.Fulfillment;

/// <summary>
/// Refund dispositions report for Fulfillment by Allegro returns.
/// </summary>
public record FulfillmentRefundDispositionsResponse
{
    /// <summary>
    /// Refund disposition entries.
    /// </summary>
    [JsonPropertyName("report")]
    public List<FulfillmentRefundDisposition>? Report { get; init; }
}

/// <summary>
/// A single refund disposition entry.
/// </summary>
public record FulfillmentRefundDisposition
{
    /// <summary>
    /// Disposition type: RETURN or BOUNCE.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Refund details.
    /// </summary>
    [JsonPropertyName("refund")]
    public FulfillmentRefundDispositionRefund? Refund { get; init; }

    /// <summary>
    /// Stock status: SELLABLE, NON_SELLABLE, MISSING or ITEM_MISMATCH.
    /// </summary>
    [JsonPropertyName("stockStatus")]
    public string? StockStatus { get; init; }

    /// <summary>
    /// Verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public string? VerificationStatus { get; init; }

    /// <summary>
    /// Who is accountable for non-sellability: WAREHOUSE, BUYER or NOT_APPLICABLE.
    /// </summary>
    [JsonPropertyName("accountableForNonSellability")]
    public string? AccountableForNonSellability { get; init; }

    /// <summary>
    /// Order identifier.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Product details.
    /// </summary>
    [JsonPropertyName("product")]
    public FulfillmentRefundDispositionProduct? Product { get; init; }

    /// <summary>
    /// Buyer details.
    /// </summary>
    [JsonPropertyName("buyer")]
    public FulfillmentRefundDispositionBuyer? Buyer { get; init; }

    /// <summary>
    /// Creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Refund details within a refund disposition.
/// </summary>
public record FulfillmentRefundDispositionRefund
{
    /// <summary>
    /// Refund status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Refund details: NO_ACTION_NEEDED, ACTION_NEEDED or IN_PROGRESS.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; init; }
}

/// <summary>
/// Product details within a refund disposition.
/// </summary>
public record FulfillmentRefundDispositionProduct
{
    /// <summary>
    /// Product GTINs.
    /// </summary>
    [JsonPropertyName("gtins")]
    public List<string>? Gtins { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }
}

/// <summary>
/// Buyer details within a refund disposition.
/// </summary>
public record FulfillmentRefundDispositionBuyer
{
    /// <summary>
    /// Buyer login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}
