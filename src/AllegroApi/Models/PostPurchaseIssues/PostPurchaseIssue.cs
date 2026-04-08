using System.Text.Json.Serialization;

namespace AllegroApi.Models.PostPurchaseIssues;

/// <summary>
/// Response containing a list of post-purchase issues (disputes and claims).
/// </summary>
public record PostPurchaseIssueListResponse
{
    /// <summary>
    /// List of issues.
    /// </summary>
    [JsonPropertyName("issues")]
    public List<PostPurchaseIssue>? Issues { get; init; }

    /// <summary>
    /// Total count of issues.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Represents a post-purchase issue (dispute or claim).
/// </summary>
public record PostPurchaseIssue
{
    /// <summary>
    /// Issue identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Type of issue (DISPUTE or CLAIM).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Reference number for the issue.
    /// </summary>
    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Due date for decision.
    /// </summary>
    [JsonPropertyName("decisionDueDate")]
    public DateTime? DecisionDueDate { get; init; }

    /// <summary>
    /// When the issue was opened.
    /// </summary>
    [JsonPropertyName("openedDate")]
    public DateTime? OpenedDate { get; init; }

    /// <summary>
    /// Issue subject/title.
    /// </summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; init; }

    /// <summary>
    /// Buyer information.
    /// </summary>
    [JsonPropertyName("buyer")]
    public IssueParticipant? Buyer { get; init; }

    /// <summary>
    /// Related checkout form.
    /// </summary>
    [JsonPropertyName("checkoutForm")]
    public IssueCheckoutForm? CheckoutForm { get; init; }

    /// <summary>
    /// Current issue state.
    /// </summary>
    [JsonPropertyName("currentState")]
    public IssueState? CurrentState { get; init; }

    /// <summary>
    /// Chat/message information.
    /// </summary>
    [JsonPropertyName("chat")]
    public IssueChat? Chat { get; init; }

    /// <summary>
    /// Buyer expectations.
    /// </summary>
    [JsonPropertyName("expectations")]
    public List<IssueExpectation>? Expectations { get; init; }

    /// <summary>
    /// Issue description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Related offer.
    /// </summary>
    [JsonPropertyName("offer")]
    public IssueOffer? Offer { get; init; }

    /// <summary>
    /// Related product.
    /// </summary>
    [JsonPropertyName("product")]
    public IssueProduct? Product { get; init; }

    /// <summary>
    /// Issue reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public IssueReason? Reason { get; init; }

    /// <summary>
    /// Buyer's right being exercised (COMPLAINT, RETURN, etc).
    /// </summary>
    [JsonPropertyName("right")]
    public string? Right { get; init; }

    /// <summary>
    /// Issue attachments.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<IssueAttachment>? Attachments { get; init; }
}

/// <summary>
/// Participant in an issue (buyer or seller).
/// </summary>
public record IssueParticipant
{
    /// <summary>
    /// Participant ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Participant login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }
}

/// <summary>
/// Checkout form related to an issue.
/// </summary>
public record IssueCheckoutForm
{
    /// <summary>
    /// Checkout form ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// When the checkout form was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Current state of an issue.
/// </summary>
public record IssueState
{
    /// <summary>
    /// Current status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Due date for the current status.
    /// </summary>
    [JsonPropertyName("statusDueDate")]
    public DateTime? StatusDueDate { get; init; }
}

/// <summary>
/// Chat/messaging information for an issue.
/// </summary>
public record IssueChat
{
    /// <summary>
    /// Last message in the chat.
    /// </summary>
    [JsonPropertyName("lastMessage")]
    public IssueMessageSummary? LastMessage { get; init; }

    /// <summary>
    /// Total number of messages.
    /// </summary>
    [JsonPropertyName("messagesCount")]
    public int? MessagesCount { get; init; }

    /// <summary>
    /// Initial message that started the issue.
    /// </summary>
    [JsonPropertyName("initialMessage")]
    public IssueMessage? InitialMessage { get; init; }
}

/// <summary>
/// Summary of the last message.
/// </summary>
public record IssueMessageSummary
{
    /// <summary>
    /// Message status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// When the message was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// A message in an issue chat.
/// </summary>
public record IssueMessage
{
    /// <summary>
    /// Message ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Message text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Message attachments.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<IssueAttachment>? Attachments { get; init; }

    /// <summary>
    /// Message author.
    /// </summary>
    [JsonPropertyName("author")]
    public IssueMessageAuthor? Author { get; init; }

    /// <summary>
    /// When the message was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Author of a message.
/// </summary>
public record IssueMessageAuthor
{
    /// <summary>
    /// Author login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// Author role (BUYER or SELLER).
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; init; }
}

/// <summary>
/// Buyer's expectations for resolving the issue.
/// </summary>
public record IssueExpectation
{
    /// <summary>
    /// Expectation name (e.g., PARTIAL_REFUND, FULL_REFUND).
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Refund details if applicable.
    /// </summary>
    [JsonPropertyName("refund")]
    public IssueRefund? Refund { get; init; }
}

/// <summary>
/// Refund information.
/// </summary>
public record IssueRefund
{
    /// <summary>
    /// Refund amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Offer related to an issue.
/// </summary>
public record IssueOffer
{
    /// <summary>
    /// Offer ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Quantity of items.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }
}

/// <summary>
/// Product related to an issue.
/// </summary>
public record IssueProduct
{
    /// <summary>
    /// Product ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Reason for the issue.
/// </summary>
public record IssueReason
{
    /// <summary>
    /// Reason description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Reason type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>
/// Attachment in an issue.
/// </summary>
public record IssueAttachment
{
    /// <summary>
    /// File name.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    /// <summary>
    /// URL to download the attachment.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Response from chat history request.
/// </summary>
public record IssueChatResponse
{
    /// <summary>
    /// List of messages and events.
    /// </summary>
    [JsonPropertyName("events")]
    public List<IssueChatEvent>? Events { get; init; }
}

/// <summary>
/// Event in an issue chat (message or state change).
/// </summary>
public record IssueChatEvent
{
    /// <summary>
    /// Event type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Message data if this is a message event.
    /// </summary>
    [JsonPropertyName("message")]
    public IssueMessage? Message { get; init; }

    /// <summary>
    /// State change data if this is a state change event.
    /// </summary>
    [JsonPropertyName("stateChange")]
    public IssueStateChange? StateChange { get; init; }
}

/// <summary>
/// State change event.
/// </summary>
public record IssueStateChange
{
    /// <summary>
    /// New state.
    /// </summary>
    [JsonPropertyName("newState")]
    public IssueState? NewState { get; init; }

    /// <summary>
    /// When the state changed.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }
}

/// <summary>
/// Request to add a message to an issue.
/// </summary>
public record AddIssueMessageRequest
{
    /// <summary>
    /// Message text (at least one of text or attachment required).
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Attachment ID (from POST /sale/issues/attachments).
    /// </summary>
    [JsonPropertyName("attachment")]
    public string? Attachment { get; init; }
}

/// <summary>
/// Attachment declaration request.
/// </summary>
public record IssueAttachmentDeclaration
{
    /// <summary>
    /// File name.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    /// <summary>
    /// MIME type (e.g., image/jpeg, application/pdf).
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; init; }
}

/// <summary>
/// Response with attachment ID.
/// </summary>
public record IssueAttachmentId
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}
