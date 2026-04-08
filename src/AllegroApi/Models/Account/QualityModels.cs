using System.Text.Json.Serialization;

namespace AllegroApi.Models.Account;

/// <summary>
/// Represents sales quality history (at most 30 days).
/// If seller doesn't have sales quality for given day, it won't be present in quality array.
/// </summary>
public record SalesQualityHistoryResponse
{
    /// <summary>
    /// List of sales quality for each day (up to 30 days).
    /// </summary>
    [JsonPropertyName("quality")]
    public List<SalesQualityForDay> Quality { get; init; } = new();
}

/// <summary>
/// Represents sales quality metrics for a specific day.
/// </summary>
public record SalesQualityForDay
{
    /// <summary>
    /// Date for which the result is calculated (e.g., "2024-08-07").
    /// </summary>
    [JsonPropertyName("resultFor")]
    public string ResultFor { get; init; } = string.Empty;

    /// <summary>
    /// The total score for the given day.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; init; }

    /// <summary>
    /// The main sales quality level for the given day (e.g., "GOOD", "AVERAGE", "POOR").
    /// </summary>
    [JsonPropertyName("grade")]
    public string Grade { get; init; } = string.Empty;

    /// <summary>
    /// The maximum possible total score for the given day.
    /// </summary>
    [JsonPropertyName("maxScore")]
    public int MaxScore { get; init; }

    /// <summary>
    /// Individual quality metrics contributing to the total score.
    /// </summary>
    [JsonPropertyName("metrics")]
    public List<SalesQualityMetric> Metrics { get; init; } = new();
}

/// <summary>
/// Represents an individual sales quality metric.
/// </summary>
public record SalesQualityMetric
{
    /// <summary>
    /// Metric code identifier.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Display name of the metric.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Score achieved for this metric.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; init; }

    /// <summary>
    /// Maximum possible score for this metric.
    /// </summary>
    [JsonPropertyName("maxScore")]
    public int? MaxScore { get; init; }

    /// <summary>
    /// Additional description or details about the metric.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Represents Smart! seller classification report for a marketplace.
/// </summary>
public record SmartSellerClassificationReport
{
    /// <summary>
    /// Marketplace identifier (e.g., "allegro-pl").
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string MarketplaceId { get; init; } = string.Empty;

    /// <summary>
    /// Whether the seller is classified as Smart! seller.
    /// </summary>
    [JsonPropertyName("smart")]
    public bool Smart { get; init; }

    /// <summary>
    /// Date when the current Smart! status began (if applicable).
    /// </summary>
    [JsonPropertyName("smartSince")]
    public string? SmartSince { get; init; }

    /// <summary>
    /// Date when the Smart! status will end (if applicable).
    /// </summary>
    [JsonPropertyName("smartUntil")]
    public string? SmartUntil { get; init; }

    /// <summary>
    /// List of criteria evaluated for Smart! classification.
    /// </summary>
    [JsonPropertyName("criteria")]
    public List<SmartCriterion>? Criteria { get; init; }
}

/// <summary>
/// Represents a single Smart! classification criterion.
/// </summary>
public record SmartCriterion
{
    /// <summary>
    /// Criterion identifier code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Display name of the criterion.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Whether the criterion is met.
    /// </summary>
    [JsonPropertyName("met")]
    public bool Met { get; init; }

    /// <summary>
    /// Additional description or details about the criterion.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
