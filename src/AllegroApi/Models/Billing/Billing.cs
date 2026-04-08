using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Billing;

/// <summary>
/// List of billing entries.
/// </summary>
public record BillingEntriesList
{
    /// <summary>
    /// Collection of billing entries.
    /// </summary>
    [JsonPropertyName("billingEntries")]
    public List<BillingEntry>? BillingEntries { get; init; }

    /// <summary>
    /// Total count of entries.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Billing entry details.
/// </summary>
public record BillingEntry
{
    /// <summary>
    /// Billing entry identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Entry type (e.g., "COMMISSION", "PROMOTION", "LISTING_FEE").
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Entry amount.
    /// </summary>
    [JsonPropertyName("value")]
    public Money? Value { get; init; }

    /// <summary>
    /// Balance after this entry.
    /// </summary>
    [JsonPropertyName("balance")]
    public Money? Balance { get; init; }

    /// <summary>
    /// Related offer ID.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Related order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Entry creation date.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Tax information.
    /// </summary>
    [JsonPropertyName("tax")]
    public TaxInfo? Tax { get; init; }
}

/// <summary>
/// Tax information for billing entry.
/// </summary>
public record TaxInfo
{
    /// <summary>
    /// Tax rate percentage.
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal? Rate { get; init; }

    /// <summary>
    /// Tax amount.
    /// </summary>
    [JsonPropertyName("value")]
    public Money? Value { get; init; }
}

/// <summary>
/// Invoice details.
/// </summary>
public record Invoice
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
    /// Total invoice amount.
    /// </summary>
    [JsonPropertyName("totalValue")]
    public Money? TotalValue { get; init; }

    /// <summary>
    /// Invoice issue date.
    /// </summary>
    [JsonPropertyName("issuedAt")]
    public DateTime? IssuedAt { get; init; }

    /// <summary>
    /// Invoice PDF URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// Billing period.
    /// </summary>
    [JsonPropertyName("period")]
    public BillingPeriod? Period { get; init; }
}

/// <summary>
/// Billing period information.
/// </summary>
public record BillingPeriod
{
    /// <summary>
    /// Period start date.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Period end date.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// List of invoices.
/// </summary>
public record InvoicesList
{
    /// <summary>
    /// Collection of invoices.
    /// </summary>
    [JsonPropertyName("invoices")]
    public List<Invoice>? Invoices { get; init; }

    /// <summary>
    /// Total count of invoices.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Response containing billing entries with enhanced details.
/// </summary>
public record BillingEntries
{
    /// <summary>
    /// List of billing entries.
    /// </summary>
    [JsonPropertyName("billingEntries")]
    public List<BillingEntry>? BillingEntriesList { get; init; }

    /// <summary>
    /// Total count of entries matching the filters.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total balance amount.
    /// </summary>
    [JsonPropertyName("totalValue")]
    public Money? TotalValue { get; init; }
}

/// <summary>
/// Response containing list of billing types.
/// </summary>
public record BillingTypesResponse
{
    /// <summary>
    /// List of billing types.
    /// </summary>
    [JsonPropertyName("billingTypes")]
    public List<BillingTypeInfo>? BillingTypes { get; init; }
}

/// <summary>
/// Billing type information.
/// </summary>
public record BillingTypeInfo
{
    /// <summary>
    /// Type identifier (e.g., "LIS" for listing fee, "SUC" for success fee).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
