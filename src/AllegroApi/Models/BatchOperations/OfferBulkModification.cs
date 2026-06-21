using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllegroApi.Models.BatchOperations;

/// <summary>
/// Command for batch offer price and stock modification (beta).
/// </summary>
public record OfferBulkChangeCommand
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Per-offer modifications.
    /// </summary>
    [JsonPropertyName("modifications")]
    public List<OfferBulkModification>? Modifications { get; init; }
}

/// <summary>
/// A single offer's price and/or stock modification.
/// </summary>
public record OfferBulkModification
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Price modifications keyed by marketplace identifier (e.g. "allegro-pl").
    /// Each value follows the marketplace price modification schema (changeType-discriminated).
    /// </summary>
    [JsonPropertyName("prices")]
    public Dictionary<string, JsonElement>? Prices { get; init; }

    /// <summary>
    /// Stock modification (FIXED sets an exact value, GAIN increases/decreases the current stock).
    /// </summary>
    [JsonPropertyName("stock")]
    public OfferBulkStockModification? Stock { get; init; }
}

/// <summary>
/// Stock modification for a bulk offer change.
/// </summary>
public record OfferBulkStockModification
{
    /// <summary>
    /// Modification type: FIXED or GAIN.
    /// </summary>
    [JsonPropertyName("changeType")]
    public string? ChangeType { get; init; }

    /// <summary>
    /// Stock value (an absolute value for FIXED, a delta for GAIN).
    /// </summary>
    [JsonPropertyName("value")]
    public int? Value { get; init; }
}

/// <summary>
/// Summary report of a batch command (offer bulk modification).
/// </summary>
public record OfferBulkModificationReport
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Command creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Command completion date (ISO 8601).
    /// </summary>
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; init; }

    /// <summary>
    /// Task count breakdown.
    /// </summary>
    [JsonPropertyName("taskCount")]
    public OfferBulkTaskCount? TaskCount { get; init; }
}

/// <summary>
/// Task counts for a batch command.
/// </summary>
public record OfferBulkTaskCount
{
    /// <summary>
    /// Number of failed tasks.
    /// </summary>
    [JsonPropertyName("failed")]
    public int? Failed { get; init; }

    /// <summary>
    /// Number of successful tasks.
    /// </summary>
    [JsonPropertyName("success")]
    public int? Success { get; init; }

    /// <summary>
    /// Total number of tasks.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }
}

/// <summary>
/// Detailed per-task report for a batch offer modification command.
/// </summary>
public record OfferBulkModificationTaskReport
{
    /// <summary>
    /// Per-task results (each relates to an offer field - price or stock).
    /// </summary>
    [JsonPropertyName("tasks")]
    public List<OfferBulkModificationTask>? Tasks { get; init; }
}

/// <summary>
/// A single task result within a batch offer modification command.
/// </summary>
public record OfferBulkModificationTask
{
    /// <summary>
    /// Subject of the task (offer and modified field).
    /// </summary>
    [JsonPropertyName("subject")]
    public JsonElement? Subject { get; init; }

    /// <summary>
    /// Task message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// Task status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Task errors, when present.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<JsonElement>? Errors { get; init; }
}
