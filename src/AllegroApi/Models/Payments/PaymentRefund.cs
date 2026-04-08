using System.Text.Json.Serialization;

namespace AllegroApi.Models.Payments;

/// <summary>
/// Request to create a payment refund.
/// </summary>
public record CreatePaymentRefundRequest
{
    /// <summary>
    /// Payment identifier.
    /// </summary>
    [JsonPropertyName("paymentId")]
    public string? PaymentId { get; init; }

    /// <summary>
    /// Refund amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public RefundAmount? Amount { get; init; }

    /// <summary>
    /// Refund reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }
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
    /// Currency code (e.g., PLN).
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Payment refund response.
/// </summary>
public record PaymentRefundResponse
{
    /// <summary>
    /// Refund identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Payment identifier.
    /// </summary>
    [JsonPropertyName("paymentId")]
    public string? PaymentId { get; init; }

    /// <summary>
    /// Refund amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public RefundAmount? Amount { get; init; }

    /// <summary>
    /// Refund reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    /// <summary>
    /// Refund status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// When the refund was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}
