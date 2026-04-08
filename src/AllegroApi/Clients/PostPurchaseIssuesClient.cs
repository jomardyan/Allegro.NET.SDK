using AllegroApi.Http;
using AllegroApi.Models.PostPurchaseIssues;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing post-purchase issues (disputes and claims).
/// Provides functionality for viewing and responding to buyer-initiated disputes and warranty claims.
/// Critical for dispute resolution before escalation.
/// </summary>
public class PostPurchaseIssuesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the PostPurchaseIssuesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public PostPurchaseIssuesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets the list of user's disputes and claims ordered by descending opened date.
    /// </summary>
    /// <param name="checkoutFormId">Filter by checkout form identifier.</param>
    /// <param name="limit">Maximum number of issues to return (1-100, default 10).</param>
    /// <param name="offset">Index of first returned issue (default 0).</param>
    /// <param name="statuses">Filter by issue statuses.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of post-purchase issues.</returns>
    public System.Threading.Tasks.Task<PostPurchaseIssueListResponse> GetIssuesAsync(
        string? checkoutFormId = null,
        int? limit = null,
        int? offset = null,
        List<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(checkoutFormId))
            queryParams["checkoutForm.id"] = checkoutFormId;
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();
        if (statuses != null && statuses.Count > 0)
        {
            foreach (var status in statuses)
            {
                queryParams.Add("status", status);
            }
        }

        return _httpClient.GetAsync<PostPurchaseIssueListResponse>(
            "/sale/issues",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a single dispute or claim by ID with full details.
    /// </summary>
    /// <param name="issueId">Dispute or claim identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Post-purchase issue details.</returns>
    public System.Threading.Tasks.Task<PostPurchaseIssue> GetIssueAsync(
        string issueId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(issueId);
        return _httpClient.GetAsync<PostPurchaseIssue>(
            $"/sale/issues/{issueId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the list of messages and state changes within a dispute or claim.
    /// </summary>
    /// <param name="issueId">Dispute or claim identifier.</param>
    /// <param name="limit">Maximum number of messages to return.</param>
    /// <param name="offset">Index of first returned message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Chat history with messages and state changes.</returns>
    public System.Threading.Tasks.Task<IssueChatResponse> GetIssueChatAsync(
        string issueId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(issueId);
        
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<IssueChatResponse>(
            $"/sale/issues/{issueId}/chat",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Adds a message to a dispute or claim.
    /// At least one of 'text' or 'attachment' must be provided.
    /// </summary>
    /// <param name="issueId">Dispute or claim identifier.</param>
    /// <param name="request">Message request with text and/or attachment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created message details.</returns>
    public System.Threading.Tasks.Task<IssueMessage> AddMessageToIssueAsync(
        string issueId,
        AddIssueMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(issueId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<AddIssueMessageRequest, IssueMessage>(
            $"/sale/issues/{issueId}/message",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates an attachment declaration to receive an upload URL.
    /// Use the URL from the Location response header to upload the file.
    /// </summary>
    /// <param name="declaration">Attachment declaration with file name and MIME type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Attachment ID and upload URL (in Location header).</returns>
    public System.Threading.Tasks.Task<IssueAttachmentId> CreateAttachmentDeclarationAsync(
        IssueAttachmentDeclaration declaration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(declaration);
        return _httpClient.PostAsync<IssueAttachmentDeclaration, IssueAttachmentId>(
            "/sale/issues/attachments",
            declaration,
            null,
            cancellationToken);
    }
}
