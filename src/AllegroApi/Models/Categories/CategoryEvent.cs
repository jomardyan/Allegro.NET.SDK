using System.Text.Json.Serialization;

namespace AllegroApi.Models.Categories;

/// <summary>
/// Response containing a list of category change events.
/// </summary>
public record CategoryEventsResponse
{
    /// <summary>
    /// List of category events.
    /// </summary>
    [JsonPropertyName("events")]
    public List<CategoryEvent>? Events { get; init; }
}

/// <summary>
/// Represents a single category change event.
/// </summary>
public record CategoryEvent
{
    /// <summary>
    /// Event identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// When the event occurred.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Type of event (CATEGORY_CREATED, CATEGORY_RENAMED, CATEGORY_MOVED, CATEGORY_DELETED).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Category details.
    /// </summary>
    [JsonPropertyName("category")]
    public CategoryEventDetails? Category { get; init; }
}

/// <summary>
/// Details about the category in the event.
/// </summary>
public record CategoryEventDetails
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Parent category information.
    /// </summary>
    [JsonPropertyName("parent")]
    public CategoryEventParent? Parent { get; init; }

    /// <summary>
    /// Whether this is a leaf category.
    /// </summary>
    [JsonPropertyName("leaf")]
    public bool? Leaf { get; init; }

    /// <summary>
    /// Redirect category (for deleted categories).
    /// </summary>
    [JsonPropertyName("redirectCategory")]
    public CategoryEventRedirect? RedirectCategory { get; init; }
}

/// <summary>
/// Parent category information.
/// </summary>
public record CategoryEventParent
{
    /// <summary>
    /// Parent category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Redirect category for deleted categories.
/// </summary>
public record CategoryEventRedirect
{
    /// <summary>
    /// Redirect category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}
