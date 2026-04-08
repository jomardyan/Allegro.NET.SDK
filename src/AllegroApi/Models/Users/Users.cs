using System.Text.Json.Serialization;

namespace AllegroApi.Models.Users;

/// <summary>
/// User rating summary.
/// </summary>
public record RatingSummary
{
    /// <summary>
    /// Average rating score.
    /// </summary>
    [JsonPropertyName("averageRating")]
    public decimal? AverageRating { get; init; }

    /// <summary>
    /// Total number of ratings.
    /// </summary>
    [JsonPropertyName("ratingCount")]
    public int? RatingCount { get; init; }

    /// <summary>
    /// Number of positive ratings.
    /// </summary>
    [JsonPropertyName("positiveRatingCount")]
    public int? PositiveRatingCount { get; init; }

    /// <summary>
    /// Number of neutral ratings.
    /// </summary>
    [JsonPropertyName("neutralRatingCount")]
    public int? NeutralRatingCount { get; init; }

    /// <summary>
    /// Number of negative ratings.
    /// </summary>
    [JsonPropertyName("negativeRatingCount")]
    public int? NegativeRatingCount { get; init; }

    /// <summary>
    /// Rating percentage.
    /// </summary>
    [JsonPropertyName("ratingPercentage")]
    public decimal? RatingPercentage { get; init; }
}

/// <summary>
/// List of user ratings.
/// </summary>
public record UserRatingsList
{
    /// <summary>
    /// Collection of ratings.
    /// </summary>
    [JsonPropertyName("ratings")]
    public List<UserRating>? Ratings { get; init; }

    /// <summary>
    /// Total count of ratings.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// User rating details.
/// </summary>
public record UserRating
{
    /// <summary>
    /// Rating identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Related order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Rating score (e.g., "POSITIVE", "NEUTRAL", "NEGATIVE").
    /// </summary>
    [JsonPropertyName("score")]
    public string? Score { get; init; }

    /// <summary>
    /// Rating comment.
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    /// <summary>
    /// Rating author information.
    /// </summary>
    [JsonPropertyName("author")]
    public RatingAuthor? Author { get; init; }

    /// <summary>
    /// Rating creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Indicates if rating can be answered.
    /// </summary>
    [JsonPropertyName("canAnswer")]
    public bool? CanAnswer { get; init; }

    /// <summary>
    /// Seller's answer to the rating.
    /// </summary>
    [JsonPropertyName("answer")]
    public RatingAnswer? Answer { get; init; }

    /// <summary>
    /// Indicates if removal can be requested.
    /// </summary>
    [JsonPropertyName("canRequestRemoval")]
    public bool? CanRequestRemoval { get; init; }
}

/// <summary>
/// Rating author information.
/// </summary>
public record RatingAuthor
{
    /// <summary>
    /// User identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// User login name.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}

/// <summary>
/// Rating answer (seller's response).
/// </summary>
public record RatingAnswer
{
    /// <summary>
    /// Answer text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Answer creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Request for answering a rating.
/// </summary>
public record AnswerRatingRequest
{
    /// <summary>
    /// Answer text (max 1000 characters).
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }
}

/// <summary>
/// Request for rating removal.
/// </summary>
public record RatingRemovalRequest
{
    /// <summary>
    /// Reason for removal request.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    /// <summary>
    /// Additional explanation.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
