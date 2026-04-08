using AllegroApi.Http;
using AllegroApi.Models.Users;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing user ratings and reputation.
/// </summary>
public class UsersClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the UsersClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public UsersClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets any user's rating summary (public information).
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Rating summary with feedback statistics.</returns>
    public System.Threading.Tasks.Task<RatingSummary> GetUserRatingsSummaryAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return _httpClient.GetAsync<RatingSummary>(
            $"/users/{userId}/ratings-summary",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets public user information by ID.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User information.</returns>
    public System.Threading.Tasks.Task<UserInfo> GetUserInfoAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return _httpClient.GetAsync<UserInfo>(
            $"/users/{userId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets public user information by ID.
    /// Alias for GetUserInfoAsync.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User information.</returns>
    public System.Threading.Tasks.Task<UserInfo> GetUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return GetUserInfoAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Gets user's ratings (as a seller).
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="recommended">Filter by recommended (true/false).</param>
    /// <param name="limit">Maximum number of ratings to return (default: 20).</param>
    /// <param name="offset">Number of ratings to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of ratings.</returns>
    public System.Threading.Tasks.Task<UserRatingsList> GetUserRatingsListAsync(
        string userId,
        bool? recommended = null,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var queryParams = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString(),
            ["offset"] = offset.ToString()
        };

        if (recommended.HasValue)
            queryParams["recommended"] = recommended.Value.ToString().ToLower();

        return _httpClient.GetAsync<UserRatingsList>(
            $"/users/{userId}/ratings",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets a list of user ratings.
    /// </summary>
    /// <param name="limit">Maximum number of ratings to return (default: 20).</param>
    /// <param name="offset">Number of ratings to skip (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of ratings.</returns>
    public System.Threading.Tasks.Task<UserRatingsList> GetUserRatingsAsync(
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<UserRatingsList>(
            $"/sale/user-ratings?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific rating by ID.
    /// </summary>
    /// <param name="ratingId">The rating identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Rating details.</returns>
    public System.Threading.Tasks.Task<UserRating> GetUserRatingAsync(
        string ratingId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ratingId);
        return _httpClient.GetAsync<UserRating>(
            $"/sale/user-ratings/{ratingId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Answers a user rating (seller's response to buyer feedback).
    /// Rate limit: 900 requests per 60 seconds.
    /// </summary>
    /// <param name="ratingId">The rating identifier.</param>
    /// <param name="request">Answer text.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated rating with answer.</returns>
    public System.Threading.Tasks.Task<UserRating> AnswerUserRatingAsync(
        string ratingId,
        AnswerRatingRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ratingId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<AnswerRatingRequest, UserRating>(
            $"/sale/user-ratings/{ratingId}/answer",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Requests removal of a rating.
    /// </summary>
    /// <param name="ratingId">The rating identifier.</param>
    /// <param name="request">Removal request details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task RequestRatingRemovalAsync(
        string ratingId,
        RatingRemovalRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ratingId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<RatingRemovalRequest>(
            $"/sale/user-ratings/{ratingId}/removal-request",
            request,
            null,
            cancellationToken);
    }
}
