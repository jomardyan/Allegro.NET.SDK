using System.Text.Json.Serialization;

namespace AllegroApi.Models.Disputes;

/// <summary>
/// List of disputes.
/// </summary>
public record DisputesList
{
    /// <summary>
    /// Collection of disputes.
    /// </summary>
    [JsonPropertyName("disputes")]
    public List<Dispute>? Disputes { get; init; }

    /// <summary>
    /// Total count of disputes.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Dispute details.
/// </summary>
public record Dispute
{
    /// <summary>
    /// Dispute identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Related checkout form (order) ID.
    /// </summary>
    [JsonPropertyName("checkoutFormId")]
    public string? CheckoutFormId { get; init; }

    /// <summary>
    /// Dispute type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Dispute status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Dispute reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    /// <summary>
    /// Dispute creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Dispute update date.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Buyer information.
    /// </summary>
    [JsonPropertyName("buyer")]
    public DisputeParty? Buyer { get; init; }

    /// <summary>
    /// Seller information.
    /// </summary>
    [JsonPropertyName("seller")]
    public DisputeParty? Seller { get; init; }
}

/// <summary>
/// Dispute party (buyer or seller) information.
/// </summary>
public record DisputeParty
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
/// List of dispute messages.
/// </summary>
public record DisputeMessagesList
{
    /// <summary>
    /// Collection of messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<DisputeMessage>? Messages { get; init; }
}

/// <summary>
/// Dispute message details.
/// </summary>
public record DisputeMessage
{
    /// <summary>
    /// Message identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Message text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Message author.
    /// </summary>
    [JsonPropertyName("author")]
    public DisputeParty? Author { get; init; }

    /// <summary>
    /// Message creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Message attachments.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<DisputeAttachment>? Attachments { get; init; }
}

/// <summary>
/// Dispute message attachment.
/// </summary>
public record DisputeAttachment
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Attachment filename.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    /// <summary>
    /// Attachment URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Request for sending a dispute message.
/// </summary>
public record SendDisputeMessageRequest
{
    /// <summary>
    /// Message text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }
}
