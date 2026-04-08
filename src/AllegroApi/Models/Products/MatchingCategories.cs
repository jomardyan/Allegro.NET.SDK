using System.Text.Json.Serialization;

namespace AllegroApi.Models.Products;

/// <summary>
/// Response containing matching categories.
/// </summary>
public record MatchingCategoriesResponse
{
    /// <summary>
    /// List of matching categories.
    /// </summary>
    [JsonPropertyName("categories")]
    public List<MatchingCategory>? Categories { get; init; }
}

/// <summary>
/// Matching category details.
/// </summary>
public record MatchingCategory
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Match score (0-1).
    /// </summary>
    [JsonPropertyName("score")]
    public double? Score { get; init; }
}
