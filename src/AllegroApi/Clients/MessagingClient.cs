using AllegroApi.Http;
using AllegroApi.Models.Messaging;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing buyer-seller messaging.
/// </summary>
public class MessagingClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the MessagingClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public MessagingClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of message threads.
    /// </summary>
    /// <param name="limit">Maximum number of threads to return (default: 20).</param>
    /// <param name="offset">Number of threads to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of message threads.</returns>
    public System.Threading.Tasks.Task<MessageThreadsList> GetThreadsAsync(
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<MessageThreadsList>(
            $"/messaging/threads?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific message thread by ID.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Thread details.</returns>
    public System.Threading.Tasks.Task<MessageThread> GetThreadAsync(
        string threadId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(threadId);
        return _httpClient.GetAsync<MessageThread>(
            $"/messaging/threads/{threadId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets messages in a thread.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="limit">Maximum number of messages to return (default: 20).</param>
    /// <param name="offset">Number of messages to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of messages.</returns>
    public System.Threading.Tasks.Task<MessagesList> GetMessagesAsync(
        string threadId,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(threadId);
        return _httpClient.GetAsync<MessagesList>(
            $"/messaging/threads/{threadId}/messages?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Sends a message in a thread.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="request">Message content and attachments.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created message.</returns>
    public System.Threading.Tasks.Task<Message> SendMessageAsync(
        string threadId,
        SendMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(threadId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<SendMessageRequest, Message>(
            $"/messaging/threads/{threadId}/messages",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Writes a new message (creating a new thread if needed).
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="message">Message recipient, text and optional attachments/order reference.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created message.</returns>
    public System.Threading.Tasks.Task<Message> WriteNewMessageAsync(
        NewMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        return _httpClient.PostAsync<NewMessage, Message>(
            "/messaging/messages",
            message,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific message by ID.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Message details.</returns>
    public System.Threading.Tasks.Task<Message> GetMessageAsync(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messageId);
        return _httpClient.GetAsync<Message>(
            $"/messaging/messages/{messageId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a single message.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task DeleteMessageAsync(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messageId);
        return _httpClient.DeleteAsync(
            $"/messaging/messages/{messageId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Marks a particular thread as read or unread.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="read">Whether the thread should be marked as read (default true).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated thread information.</returns>
    public System.Threading.Tasks.Task<MessageThreadReadResult> MarkThreadReadAsync(
        string threadId,
        bool read = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(threadId);
        return _httpClient.PutAsync<ThreadReadFlag, MessageThreadReadResult>(
            $"/messaging/threads/{threadId}/read",
            new ThreadReadFlag { Read = read },
            null,
            cancellationToken);
    }

    /// <summary>
    /// Declares a new message attachment, reserving an identifier for binary upload.
    /// </summary>
    /// <param name="declaration">Attachment declaration (file name and size).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Identifier of the declared attachment.</returns>
    public System.Threading.Tasks.Task<MessageAttachmentId> CreateAttachmentDeclarationAsync(
        NewAttachmentDeclaration declaration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(declaration);
        return _httpClient.PostAsync<NewAttachmentDeclaration, MessageAttachmentId>(
            "/messaging/message-attachments",
            declaration,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Uploads the binary content of a previously declared message attachment.
    /// </summary>
    /// <param name="attachmentId">The declared attachment identifier.</param>
    /// <param name="fileBytes">File content as a byte array.</param>
    /// <param name="contentType">MIME type (image/png, image/gif, image/bmp, image/tiff, image/jpeg or application/pdf).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Identifier of the uploaded attachment.</returns>
    public async System.Threading.Tasks.Task<MessageAttachmentId> UploadAttachmentAsync(
        string attachmentId,
        byte[] fileBytes,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attachmentId);
        ArgumentNullException.ThrowIfNull(fileBytes);
        ArgumentNullException.ThrowIfNull(contentType);
        var response = await _httpClient.PutRawAsync(
            $"/messaging/message-attachments/{attachmentId}",
            fileBytes,
            contentType,
            cancellationToken);
        return await _httpClient.ReadJsonAsync<MessageAttachmentId>(response);
    }

    /// <summary>
    /// Downloads the binary content of a message attachment.
    /// </summary>
    /// <param name="attachmentId">The attachment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>File content as a byte array.</returns>
    public async System.Threading.Tasks.Task<byte[]> DownloadAttachmentAsync(
        string attachmentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attachmentId);
        var response = await _httpClient.GetRawAsync(
            $"/messaging/message-attachments/{attachmentId}",
            null,
            cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
