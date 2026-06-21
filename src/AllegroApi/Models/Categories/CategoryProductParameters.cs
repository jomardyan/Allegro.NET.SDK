using System.Text.Json.Serialization;

namespace AllegroApi.Models.Categories;

/// <summary>
/// List of product parameters available in a category.
/// </summary>
public record CategoryProductParameterList
{
    /// <summary>
    /// Product parameters available in the category.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<CategoryProductParameter>? Parameters { get; init; }
}

/// <summary>
/// A product parameter available in a category.
/// </summary>
public record CategoryProductParameter
{
    /// <summary>
    /// Parameter identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Parameter name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Parameter type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Whether the parameter is required.
    /// </summary>
    [JsonPropertyName("required")]
    public bool? Required { get; init; }

    /// <summary>
    /// Unit of the parameter values, when applicable.
    /// </summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

/// <summary>
/// Planned changes in category parameters.
/// </summary>
public record CategoryParametersScheduledChangesResponse
{
    /// <summary>
    /// Scheduled category parameter changes.
    /// </summary>
    [JsonPropertyName("scheduledChanges")]
    public List<CategoryParametersScheduledChange>? ScheduledChanges { get; init; }
}

/// <summary>
/// A single scheduled change in category parameters.
/// </summary>
public record CategoryParametersScheduledChange
{
    /// <summary>
    /// Date the change was scheduled at (ISO 8601).
    /// </summary>
    [JsonPropertyName("scheduledAt")]
    public DateTime? ScheduledAt { get; init; }

    /// <summary>
    /// Date the change is scheduled for (ISO 8601).
    /// </summary>
    [JsonPropertyName("scheduledFor")]
    public DateTime? ScheduledFor { get; init; }

    /// <summary>
    /// Change type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
