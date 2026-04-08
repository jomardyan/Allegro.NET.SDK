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
    /// Gets a specific message by ID.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Message details.</returns>
    public System.Threading.Tasks.Task<Message> GetMessageAsync(
        string threadId,
        string messageId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(threadId);
        ArgumentNullException.ThrowIfNull(messageId);
        return _httpClient.GetAsync<Message>(
            $"/messaging/threads/{threadId}/messages/{messageId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Marks a message as read.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    public System.Threading.Tasks.Task MarkMessageAsReadAsync(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messageId);
        return _httpClient.PutAsync<object, object>(
            $"/messaging/messages/{messageId}/mark-read",
            new { },
            null,
            cancellationToken);
    }
}
