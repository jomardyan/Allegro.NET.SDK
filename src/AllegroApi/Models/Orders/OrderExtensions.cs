using System.Text.Json.Serialization;

namespace AllegroApi.Models.Orders;

/// <summary>
/// Order event statistics containing the latest event information.
/// </summary>
public record OrderEventStats
{
    /// <summary>
    /// Latest event identifier.
    /// </summary>
    [JsonPropertyName("latestEvent")]
    public LatestEvent? LatestEvent { get; init; }
}

/// <summary>
/// Latest event details.
/// </summary>
public record LatestEvent
{
    /// <summary>
    /// Event ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// When the event occurred.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }
}

/// <summary>
/// Response containing list of carriers.
/// </summary>
public record CarriersResponse
{
    /// <summary>
    /// List of available carriers.
    /// </summary>
    [JsonPropertyName("carriers")]
    public List<Carrier>? Carriers { get; init; }
}

/// <summary>
/// Shipping carrier information.
/// </summary>
public record Carrier
{
    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Carrier name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Fulfillment update request.
/// </summary>
public record FulfillmentUpdateRequest
{
    /// <summary>
    /// New fulfillment status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>
/// Response containing order invoices.
/// </summary>
public record OrderInvoicesResponse
{
    /// <summary>
    /// List of invoices.
    /// </summary>
    [JsonPropertyName("invoices")]
    public List<OrderInvoice>? Invoices { get; init; }
}

/// <summary>
/// Order invoice information.
/// </summary>
public record OrderInvoice
{
    /// <summary>
    /// Invoice identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Invoice number.
    /// </summary>
    [JsonPropertyName("number")]
    public string? Number { get; init; }

    /// <summary>
    /// Invoice type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// When the invoice was issued.
    /// </summary>
    [JsonPropertyName("issuedAt")]
    public DateTime? IssuedAt { get; init; }
}

/// <summary>
/// Response containing list of refund claims.
/// </summary>
public record RefundClaimsResponse
{
    /// <summary>
    /// List of refund claims.
    /// </summary>
    [JsonPropertyName("refundClaims")]
    public List<RefundClaim>? RefundClaims { get; init; }

    /// <summary>
    /// Count of returned refund claims.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Refund claim (commission refund application).
/// </summary>
public record RefundClaim
{
    /// <summary>
    /// Refund claim identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Related order line item.
    /// </summary>
    [JsonPropertyName("lineItem")]
    public RefundLineItem? LineItem { get; init; }

    /// <summary>
    /// Buyer information.
    /// </summary>
    [JsonPropertyName("buyer")]
    public RefundBuyer? Buyer { get; init; }

    /// <summary>
    /// Refund amount.
    /// </summary>
    [JsonPropertyName("refund")]
    public RefundAmount? Refund { get; init; }

    /// <summary>
    /// Claim status (IN_PROGRESS, WAITING_FOR_PAYMENT_REFUND, GRANTED, REJECTED, etc.).
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// When the claim was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Line item related to refund claim.
/// </summary>
public record RefundLineItem
{
    /// <summary>
    /// Line item identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Related offer.
    /// </summary>
    [JsonPropertyName("offer")]
    public RefundOffer? Offer { get; init; }
}

/// <summary>
/// Offer related to refund claim.
/// </summary>
public record RefundOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Offer name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Buyer information for refund claim.
/// </summary>
public record RefundBuyer
{
    /// <summary>
    /// Buyer login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}

/// <summary>
/// Refund amount information.
/// </summary>
public record RefundAmount
{
    /// <summary>
    /// Amount value.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Request to create a refund claim.
/// </summary>
public record CreateRefundClaimRequest
{
    /// <summary>
    /// Order line item identifier.
    /// </summary>
    [JsonPropertyName("lineItemId")]
    public string? LineItemId { get; init; }

    /// <summary>
    /// Refund reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }
}
