using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Payments;

/// <summary>
/// List of payment operations.
/// </summary>
public record PaymentOperationsList
{
    /// <summary>
    /// Collection of payment operations.
    /// </summary>
    [JsonPropertyName("paymentOperations")]
    public List<PaymentOperation>? PaymentOperations { get; init; }
}

/// <summary>
/// Payment operation details.
/// </summary>
public record PaymentOperation
{
    /// <summary>
    /// Payment operation identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Order identifier.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Payment type (e.g., "PAYMENT", "REFUND").
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Payment status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Payment amount.
    /// </summary>
    [JsonPropertyName("value")]
    public Money? Value { get; init; }

    /// <summary>
    /// Payment creation date.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Payment provider.
    /// </summary>
    [JsonPropertyName("provider")]
    public string? Provider { get; init; }

    /// <summary>
    /// Payment method details.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public PaymentMethod? PaymentMethod { get; init; }
}

/// <summary>
/// Payment method information.
/// </summary>
public record PaymentMethod
{
    /// <summary>
    /// Payment method ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Payment method name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Payment method type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>
/// Payment details response.
/// </summary>
public record PaymentDetailsResponse
{
    /// <summary>
    /// Payment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Payment status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Total payment amount.
    /// </summary>
    [JsonPropertyName("totalValue")]
    public Money? TotalValue { get; init; }

    /// <summary>
    /// Payment creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Payment update date.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// List of payment operations.
    /// </summary>
    [JsonPropertyName("operations")]
    public List<PaymentOperation>? Operations { get; init; }
}

/// <summary>
/// Response containing payment operations history.
/// </summary>
public record PaymentOperationsHistory
{
    /// <summary>
    /// List of payment operations.
    /// </summary>
    [JsonPropertyName("paymentOperations")]
    public List<PaymentOperationHistory>? PaymentOperations { get; init; }

    /// <summary>
    /// Total count of operations.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total value summary.
    /// </summary>
    [JsonPropertyName("totalValue")]
    public Money? TotalValue { get; init; }
}

/// <summary>
/// Payment operation history entry.
/// </summary>
public record PaymentOperationHistory
{
    /// <summary>
    /// Operation identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// When the operation occurred.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Operation type (e.g., CONTRIBUTION, REFUND_CHARGE, PAYOUT).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Operation value.
    /// </summary>
    [JsonPropertyName("value")]
    public Money? Value { get; init; }

    /// <summary>
    /// Related payment information.
    /// </summary>
    [JsonPropertyName("payment")]
    public PaymentHistoryInfo? Payment { get; init; }

    /// <summary>
    /// Wallet information.
    /// </summary>
    [JsonPropertyName("wallet")]
    public WalletInfo? Wallet { get; init; }

    /// <summary>
    /// Participant information.
    /// </summary>
    [JsonPropertyName("participant")]
    public ParticipantInfo? Participant { get; init; }

    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }
}

/// <summary>
/// Payment information in operation history.
/// </summary>
public record PaymentHistoryInfo
{
    /// <summary>
    /// Payment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Wallet information.
/// </summary>
public record WalletInfo
{
    /// <summary>
    /// Wallet type (AVAILABLE, WAITING).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Payment operator (PAYU, P24, AF, etc.).
    /// </summary>
    [JsonPropertyName("paymentOperator")]
    public string? PaymentOperator { get; init; }
}

/// <summary>
/// Participant information.
/// </summary>
public record ParticipantInfo
{
    /// <summary>
    /// Participant login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}
