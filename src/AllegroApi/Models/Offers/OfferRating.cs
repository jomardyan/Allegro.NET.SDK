using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllegroApi.Models.Offers;

/// <summary>
/// Aggregated rating information for an offer.
/// </summary>
public record OfferRating
{
    /// <summary>
    /// Average score for the offer.
    /// </summary>
    [JsonPropertyName("averageScore")]
    public string? AverageScore { get; init; }

    /// <summary>
    /// Distribution of scores.
    /// </summary>
    [JsonPropertyName("scoreDistribution")]
    public List<OfferRatingScore>? ScoreDistribution { get; init; }

    /// <summary>
    /// Total number of responses.
    /// </summary>
    [JsonPropertyName("totalResponses")]
    public int? TotalResponses { get; init; }

    /// <summary>
    /// Feedback regarding product size.
    /// </summary>
    [JsonPropertyName("sizeFeedback")]
    public List<OfferRatingScore>? SizeFeedback { get; init; }
}

/// <summary>
/// A single score bucket within an offer rating.
/// </summary>
public record OfferRatingScore
{
    /// <summary>
    /// Score bucket name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Number of responses in this bucket.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Response listing offers with missing (unfilled) parameters.
/// </summary>
public record UnfilledParametersResponse
{
    /// <summary>
    /// Offers with unfilled parameters.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<UnfilledParametersOffer>? Offers { get; init; }

    /// <summary>
    /// Number of returned offers.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total number of matching offers.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; init; }
}

/// <summary>
/// An offer with unfilled parameters.
/// </summary>
public record UnfilledParametersOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Unfilled parameters of the offer.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<JsonElement>? Parameters { get; init; }

    /// <summary>
    /// Category the offer belongs to.
    /// </summary>
    [JsonPropertyName("category")]
    public JsonElement? Category { get; init; }
}
