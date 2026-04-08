using AllegroApi.Http;
using AllegroApi.Models.PointsOfService;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing points of service (pickup locations).
/// </summary>
public class PointsOfServiceClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the PointsOfServiceClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public PointsOfServiceClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of seller's points of service.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of points of service.</returns>
    public System.Threading.Tasks.Task<PointsOfServiceList> GetPointsOfServiceAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<PointsOfServiceList>("/points-of-service", null, cancellationToken);
    }

    /// <summary>
    /// Gets a specific point of service by ID.
    /// </summary>
    /// <param name="posId">The point of service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Point of service details.</returns>
    public System.Threading.Tasks.Task<PointOfService> GetPointOfServiceAsync(
        string posId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(posId);
        return _httpClient.GetAsync<PointOfService>($"/points-of-service/{posId}", null, cancellationToken);
    }

    /// <summary>
    /// Creates a new point of service.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="pointOfService">Point of service details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created point of service.</returns>
    public System.Threading.Tasks.Task<PointOfService> CreatePointOfServiceAsync(
        PointOfService pointOfService,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pointOfService);
        return _httpClient.PostAsync<PointOfService, PointOfService>(
            "/points-of-service",
            pointOfService,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing point of service.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="posId">The point of service identifier.</param>
    /// <param name="pointOfService">Updated point of service details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated point of service.</returns>
    public System.Threading.Tasks.Task<PointOfService> UpdatePointOfServiceAsync(
        string posId,
        PointOfService pointOfService,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(posId);
        ArgumentNullException.ThrowIfNull(pointOfService);
        return _httpClient.PutAsync<PointOfService, PointOfService>(
            $"/points-of-service/{posId}",
            pointOfService,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a point of service.
    /// </summary>
    /// <param name="posId">The point of service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task DeletePointOfServiceAsync(
        string posId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(posId);
        return _httpClient.DeleteAsync($"/points-of-service/{posId}", null, cancellationToken);
    }
}
