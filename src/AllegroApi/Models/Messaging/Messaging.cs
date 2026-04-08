using System.Text.Json.Serialization;

namespace AllegroApi.Models.Messaging;

/// <summary>
/// List of message threads.
/// </summary>
public record MessageThreadsList
{
    /// <summary>
    /// Collection of message threads.
    /// </summary>
    [JsonPropertyName("threads")]
    public List<MessageThread>? Threads { get; init; }

    /// <summary>
    /// Total count of threads.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Message thread details.
/// </summary>
public record MessageThread
{
    /// <summary>
    /// Thread identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Related order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Related offer ID.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Thread subject.
    /// </summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; init; }

    /// <summary>
    /// Interlocutor (other party) information.
    /// </summary>
    [JsonPropertyName("interlocutor")]
    public Interlocutor? Interlocutor { get; init; }

    /// <summary>
    /// Last message details.
    /// </summary>
    [JsonPropertyName("lastMessage")]
    public MessageSummary? LastMessage { get; init; }

    /// <summary>
    /// Indicates if thread has unread messages.
    /// </summary>
    [JsonPropertyName("unread")]
    public bool? Unread { get; init; }

    /// <summary>
    /// Thread creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Interlocutor (message participant) information.
/// </summary>
public record Interlocutor
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
/// Message summary information.
/// </summary>
public record MessageSummary
{
    /// <summary>
    /// Message text preview.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Message creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Full message details.
/// </summary>
public record Message
{
    /// <summary>
    /// Message identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Thread identifier.
    /// </summary>
    [JsonPropertyName("threadId")]
    public string? ThreadId { get; init; }

    /// <summary>
    /// Message text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Message author information.
    /// </summary>
    [JsonPropertyName("author")]
    public Interlocutor? Author { get; init; }

    /// <summary>
    /// Message creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Indicates if message was read.
    /// </summary>
    [JsonPropertyName("read")]
    public bool? Read { get; init; }

    /// <summary>
    /// Message attachments.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<MessageAttachment>? Attachments { get; init; }
}

/// <summary>
/// Message attachment information.
/// </summary>
public record MessageAttachment
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
    /// Attachment MIME type.
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; init; }

    /// <summary>
    /// Attachment URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Request for sending a message.
/// </summary>
public record SendMessageRequest
{
    /// <summary>
    /// Message text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// List of attachment IDs.
    /// </summary>
    [JsonPropertyName("attachmentIds")]
    public List<string>? AttachmentIds { get; init; }
}

/// <summary>
/// List of messages in a thread.
/// </summary>
public record MessagesList
{
    /// <summary>
    /// Collection of messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<Message>? Messages { get; init; }

    /// <summary>
    /// Total count of messages.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}
