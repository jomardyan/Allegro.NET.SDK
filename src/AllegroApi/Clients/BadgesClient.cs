using AllegroApi.Http;
using AllegroApi.Models.Badges;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing badge campaigns and applications.
/// Badges are promotional markers (e.g., discount badges) that can be attached to offers.
/// </summary>
public class BadgesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BadgesClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making API requests.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    public BadgesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets the list of badge campaigns available for the user.
    /// Use this method to discover which badge campaigns you can participate in.
    /// </summary>
    /// <param name="marketplaceId">The marketplace ID (optional filter).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of badge campaigns with eligibility information.</returns>
    public System.Threading.Tasks.Task<GetBadgeCampaignsList> GetBadgeCampaignsAsync(
        string? marketplaceId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(marketplaceId))
        {
            queryParams["marketplace.id"] = marketplaceId;
        }

        return _httpClient.GetAsync<GetBadgeCampaignsList>(
            "/sale/badge-campaigns",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new badge application to add a badge to an offer.
    /// Required for DISCOUNT and SOURCING campaign types: prices with bargain amount.
    /// </summary>
    /// <param name="request">The badge application request with campaign, offer, and pricing details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created badge application with processing status.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    public System.Threading.Tasks.Task<BadgeApplication> CreateBadgeApplicationAsync(
        BadgeApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PostAsync<BadgeApplicationRequest, BadgeApplication>(
            "/sale/badges",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a single badge application by its ID.
    /// Use this to check the processing status and get rejection reasons if declined.
    /// </summary>
    /// <param name="applicationId">The badge application ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The badge application details including process status.</returns>
    /// <exception cref="ArgumentNullException">Thrown when applicationId is null.</exception>
    public System.Threading.Tasks.Task<BadgeApplication> GetBadgeApplicationAsync(
        string applicationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(applicationId);

        return _httpClient.GetAsync<BadgeApplication>(
            $"/sale/badge-applications/{applicationId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a list of badge applications with optional filtering.
    /// Use to track all applications, filter by campaign or offer.
    /// </summary>
    /// <param name="campaignId">Filter by badge campaign ID (optional).</param>
    /// <param name="offerId">Filter by offer ID (optional).</param>
    /// <param name="offset">Result offset (optional, default 0).</param>
    /// <param name="limit">Result limit (optional, max 1000, default 50).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of badge applications matching the filters.</returns>
    public System.Threading.Tasks.Task<BadgeApplications> GetBadgeApplicationsAsync(
        string? campaignId = null,
        string? offerId = null,
        int? offset = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(campaignId))
        {
            queryParams["campaign.id"] = campaignId;
        }
        if (!string.IsNullOrEmpty(offerId))
        {
            queryParams["offer.id"] = offerId;
        }
        if (offset.HasValue)
        {
            queryParams["offset"] = offset.Value.ToString();
        }
        if (limit.HasValue)
        {
            queryParams["limit"] = limit.Value.ToString();
        }

        return _httpClient.GetAsync<BadgeApplications>(
            "/sale/badge-applications",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a badge operation by its ID.
    /// Use this to track the status of badge update or finish operations.
    /// The operation ID is returned in the Location header after PATCH requests.
    /// </summary>
    /// <param name="operationId">The badge operation ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The badge operation details including process status.</returns>
    /// <exception cref="ArgumentNullException">Thrown when operationId is null.</exception>
    public System.Threading.Tasks.Task<BadgeOperation> GetBadgeOperationAsync(
        string operationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operationId);

        return _httpClient.GetAsync<BadgeOperation>(
            $"/sale/badge-operations/{operationId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates a badge campaign on an offer (updates price) or finishes the badge (removes it).
    /// Returns 202 Accepted with operation ID in Location header for tracking.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="campaignId">The campaign ID.</param>
    /// <param name="request">The patch request (update prices or set status to FINISHED).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the operation ID for tracking.</returns>
    /// <exception cref="ArgumentNullException">Thrown when offerId, campaignId, or request is null.</exception>
    public async System.Threading.Tasks.Task<BadgePatchResponse> UpdateOfferBadgeCampaignAsync(
        string offerId,
        string campaignId,
        BadgePatchRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(campaignId);
        ArgumentNullException.ThrowIfNull(request);

        var response = await _httpClient.PatchAsync<BadgePatchRequest, BadgePatchResponse>(
            $"/sale/badges/offers/{offerId}/campaigns/{campaignId}",
            request,
            null,
            cancellationToken);

        return response;
    }
}
