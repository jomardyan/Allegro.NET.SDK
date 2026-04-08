using AllegroApi.Http;
using AllegroApi.Models.Marketplaces;

namespace AllegroApi.Clients;

/// <summary>
/// Client for marketplace information operations.
/// Provides access to details about all Allegro marketplaces including supported languages, currencies, and shipping countries.
/// </summary>
public class MarketplacesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the MarketplacesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public MarketplacesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets details for all marketplaces in Allegro.
    /// Use this resource to get information about all the marketplaces on the platform,
    /// including supported languages, currencies, and shipping countries.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all available Allegro marketplaces with their configuration.</returns>
    public System.Threading.Tasks.Task<AllegroMarketplaces> GetAllMarketplacesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AllegroMarketplaces>(
            "/marketplaces",
            null,
            cancellationToken);
    }
}
