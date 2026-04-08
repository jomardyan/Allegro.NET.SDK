using AllegroApi.Http;
using AllegroApi.Models.Disputes;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing disputes and issues.
/// </summary>
public class DisputesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the DisputesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public DisputesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of disputes.
    /// </summary>
    /// <param name="limit">Maximum number of disputes to return (default: 20).</param>
    /// <param name="offset">Number of disputes to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of disputes.</returns>
    public System.Threading.Tasks.Task<DisputesList> GetDisputesAsync(
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<DisputesList>(
            $"/sale/disputes?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific dispute by ID.
    /// </summary>
    /// <param name="disputeId">The dispute identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Dispute details.</returns>
    public System.Threading.Tasks.Task<Dispute> GetDisputeAsync(
        string disputeId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(disputeId);
        return _httpClient.GetAsync<Dispute>(
            $"/sale/disputes/{disputeId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets messages in a dispute.
    /// </summary>
    /// <param name="disputeId">The dispute identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of dispute messages.</returns>
    public System.Threading.Tasks.Task<DisputeMessagesList> GetDisputeMessagesAsync(
        string disputeId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(disputeId);
        return _httpClient.GetAsync<DisputeMessagesList>(
            $"/sale/disputes/{disputeId}/messages",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Sends a message in a dispute.
    /// </summary>
    /// <param name="disputeId">The dispute identifier.</param>
    /// <param name="request">Message content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created message.</returns>
    public System.Threading.Tasks.Task<DisputeMessage> SendDisputeMessageAsync(
        string disputeId,
        SendDisputeMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(disputeId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<SendDisputeMessageRequest, DisputeMessage>(
            $"/sale/disputes/{disputeId}/messages",
            request,
            null,
            cancellationToken);
    }
}
