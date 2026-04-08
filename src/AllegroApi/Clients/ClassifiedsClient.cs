using AllegroApi.Http;
using AllegroApi.Models.Classifieds;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing classified packages and promotions.
/// Classifieds are a special type of listing with additional promotion options.
/// </summary>
public class ClassifiedsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassifiedsClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making API requests.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    public ClassifiedsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets classified packages currently assigned to an offer.
    /// Use this to retrieve information about base and extra packages assigned to a classified ad.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The classified packages response with base and extra packages.</returns>
    /// <exception cref="ArgumentNullException">Thrown when offerId is null.</exception>
    public System.Threading.Tasks.Task<ClassifiedResponse> GetClassifiedPackagesAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);

        return _httpClient.GetAsync<ClassifiedResponse>(
            $"/sale/offer-classifieds-packages/{offerId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Assigns classified packages to an offer.
    /// Use this to set or update the base package and extra packages for a classified ad.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="packages">The packages to assign (base package and optional extra packages).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when offerId or packages is null.</exception>
    public async System.Threading.Tasks.Task AssignClassifiedPackagesAsync(
        string offerId,
        ClassifiedPackages packages,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(packages);

        await _httpClient.PutAsync<ClassifiedPackages, object>(
            $"/sale/offer-classifieds-packages/{offerId}",
            packages,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets configurations of classifieds packages available for a category.
    /// Use this to discover which packages and promotions are available for a specific category.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The list of package configurations for the category.</returns>
    /// <exception cref="ArgumentNullException">Thrown when categoryId is null.</exception>
    public System.Threading.Tasks.Task<ClassifiedPackageConfigs> GetClassifiedPackageConfigurationsAsync(
        string categoryId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryId);

        var queryParams = new Dictionary<string, string>
        {
            ["category.id"] = categoryId
        };

        return _httpClient.GetAsync<ClassifiedPackageConfigs>(
            "/sale/classifieds-packages",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets the configuration of a specific classifieds package.
    /// Use this to retrieve detailed information about a package including promotions, extensions, and publication settings.
    /// </summary>
    /// <param name="packageId">The classifieds package ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The package configuration details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when packageId is null.</exception>
    public System.Threading.Tasks.Task<ClassifiedPackageConfig> GetClassifiedPackageConfigurationAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(packageId);

        return _httpClient.GetAsync<ClassifiedPackageConfig>(
            $"/sale/classifieds-packages/{packageId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets daily statistics for the seller's classified advertisements.
    /// Returns statistics grouped by date and event type for the logged-in user.
    /// </summary>
    /// <param name="dateGte">Start of the date range (ISO 8601 format). Must be less than current datetime. Range must be less than 3 months.</param>
    /// <param name="dateLte">End of the date range (ISO 8601 format). Must be greater than dateGte. Range must be less than 3 months.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Seller classified ads statistics grouped by day and event type.</returns>
    /// <exception cref="AllegroApi.Exceptions.AllegroBadRequestException">Invalid query parameters.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthenticationException">Unauthorized.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthorizationException">Forbidden.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroNotFoundException">Not found.</exception>
    public System.Threading.Tasks.Task<SellerOfferStatsResponseDto> GetSellerClassifiedStatsAsync(
        DateTime? dateGte = null,
        DateTime? dateLte = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (dateGte.HasValue)
            queryParams["date.gte"] = dateGte.Value.ToString("o");
        if (dateLte.HasValue)
            queryParams["date.lte"] = dateLte.Value.ToString("o");

        return _httpClient.GetAsync<SellerOfferStatsResponseDto>(
            "/sale/classified-seller-stats",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets daily statistics for a list of specific classified advertisements.
    /// Returns statistics grouped by offer, date and event type.
    /// </summary>
    /// <param name="offerIds">List of offer identifiers (maximum 50).</param>
    /// <param name="dateGte">Start of the date range (ISO 8601 format). Range must be less than 3 months.</param>
    /// <param name="dateLte">End of the date range (ISO 8601 format). Range must be less than 3 months.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Classified ads statistics per offer grouped by day and event type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when offerIds is null.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroBadRequestException">Invalid query parameters.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthenticationException">Unauthorized.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthorizationException">Forbidden.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroNotFoundException">Not found.</exception>
    public System.Threading.Tasks.Task<OfferStatsResponseDto> GetClassifiedOffersStatsAsync(
        IEnumerable<string> offerIds,
        DateTime? dateGte = null,
        DateTime? dateLte = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerIds);

        var ids = offerIds.ToList();
        if (ids.Count < 1 || ids.Count > 50)
            throw new ArgumentOutOfRangeException(nameof(offerIds), "Between 1 and 50 offer IDs must be provided.");

        // Build URL with exploded offer.id params (repeated keys per swagger spec)
        var queryParts = ids.Select(id => $"offer.id={Uri.EscapeDataString(id)}").ToList();
        if (dateGte.HasValue)
            queryParts.Add($"date.gte={Uri.EscapeDataString(dateGte.Value.ToString("o"))}");
        if (dateLte.HasValue)
            queryParts.Add($"date.lte={Uri.EscapeDataString(dateLte.Value.ToString("o"))}");

        var qs = string.Join("&", queryParts);
        return _httpClient.GetAsync<OfferStatsResponseDto>(
            $"/sale/classified-offers-stats?{qs}",
            null,
            cancellationToken);
    }
}
