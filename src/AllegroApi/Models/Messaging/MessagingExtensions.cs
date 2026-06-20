using System.Text.Json.Serialization;

namespace AllegroApi.Models.Messaging;

/// <summary>
/// Request body for writing a new message (not tied to an existing thread).
/// </summary>
public record NewMessage
{
    /// <summary>
    /// Message recipient.
    /// </summary>
    [JsonPropertyName("recipient")]
    public MessageRecipient? Recipient { get; init; }

    /// <summary>
    /// Message text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Identifiers of attachments declared for this message.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<MessageAttachmentId>? Attachments { get; init; }

    /// <summary>
    /// Order the message relates to, when applicable.
    /// </summary>
    [JsonPropertyName("order")]
    public MessageOrderReference? Order { get; init; }
}

/// <summary>
/// Recipient of a new message.
/// </summary>
public record MessageRecipient
{
    /// <summary>
    /// Recipient login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}

/// <summary>
/// Reference to an order a message relates to.
/// </summary>
public record MessageOrderReference
{
    /// <summary>
    /// Order (checkout form) identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Identifier of a message attachment.
/// </summary>
public record MessageAttachmentId
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Declaration of a new message attachment (reserves an identifier for upload).
/// </summary>
public record NewAttachmentDeclaration
{
    /// <summary>
    /// Original file name.
    /// </summary>
    [JsonPropertyName("filename")]
    public string? Filename { get; init; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    public long? Size { get; init; }
}

/// <summary>
/// Request body for changing the read flag on a thread.
/// </summary>
public record ThreadReadFlag
{
    /// <summary>
    /// Whether the thread should be marked as read.
    /// </summary>
    [JsonPropertyName("read")]
    public bool Read { get; init; }
}

/// <summary>
/// Message thread summary returned when changing the read flag.
/// </summary>
public record MessageThreadReadResult
{
    /// <summary>
    /// Thread identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Whether the thread is read.
    /// </summary>
    [JsonPropertyName("read")]
    public bool? Read { get; init; }

    /// <summary>
    /// Date and time of the last message in the thread (ISO 8601).
    /// </summary>
    [JsonPropertyName("lastMessageDateTime")]
    public DateTime? LastMessageDateTime { get; init; }

    /// <summary>
    /// The other participant of the thread.
    /// </summary>
    [JsonPropertyName("interlocutor")]
    public Interlocutor? Interlocutor { get; init; }
}
