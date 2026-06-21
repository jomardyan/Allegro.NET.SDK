using AllegroApi.Http;
using AllegroApi.Models.Offers;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing offers in Allegro API
/// </summary>
public class OfferManagementClient
{
    private readonly AllegroHttpClient _httpClient;
    private readonly ILogger<OfferManagementClient>? _logger;

    /// <summary>
    /// Initializes a new instance of the OfferManagementClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    /// <param name="logger">Optional logger for diagnostics.</param>
    public OfferManagementClient(AllegroHttpClient httpClient, ILogger<OfferManagementClient>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
    }

    #region Product Offers

    /// <summary>
    /// Create offer based on product
    /// </summary>
    public async Task<SaleProductOfferResponseV1> CreateProductOfferAsync(
        SaleProductOfferRequestV1 request,
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Creating product offer");
        return await _httpClient.PostAsync<SaleProductOfferRequestV1, SaleProductOfferResponseV1>(
            "/sale/product-offers",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Edit an existing offer
    /// </summary>
    public async Task<SaleProductOfferResponseV1> EditProductOfferAsync(
        string offerId,
        SaleProductOfferPatchRequestV1 request,
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Editing product offer {offerId}");
        return await _httpClient.PatchAsync<SaleProductOfferPatchRequestV1, SaleProductOfferResponseV1>(
            $"/sale/product-offers/{offerId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Get all data of a particular product offer
    /// </summary>
    public async Task<SaleProductOfferResponseV1> GetProductOfferAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting product offer {offerId}");
        return await _httpClient.GetAsync<SaleProductOfferResponseV1>(
            $"/sale/product-offers/{offerId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Get selected data of a particular product offer
    /// </summary>
    public async Task<SalePartialProductOfferResponse> GetPartialProductOfferAsync(
        string offerId,
        List<string> include, // "stock", "price"
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting partial product offer {offerId}");
        var queryParams = new Dictionary<string, string>();
        foreach (var part in include)
        {
            queryParams.Add("include", part);
        }

        return await _httpClient.GetAsync<SalePartialProductOfferResponse>(
            $"/sale/product-offers/{offerId}/parts",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Check the processing status of a POST or PATCH request
    /// </summary>
    public async Task<SaleProductOfferStatusResponse> GetProductOfferProcessingStatusAsync(
        string offerId,
        string operationId,
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting product offer processing status for offer {offerId}, operation {operationId}");
        return await _httpClient.GetAsync<SaleProductOfferStatusResponse>(
            $"/sale/product-offers/{offerId}/operations/{operationId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Offer Search and Listing

    /// <summary>
    /// Get seller's offers
    /// </summary>
    public async Task<OffersSearchResultDto> SearchOffersAsync(
        OfferSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Searching offers");
        return await _httpClient.GetAsync<OffersSearchResultDto>(
            "/sale/offers",
            searchParams.ToQueryParams(),
            cancellationToken);
    }

    /// <summary>
    /// Delete a draft offer
    /// </summary>
    public async Task DeleteOfferAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Deleting offer {offerId}");
        await _httpClient.DeleteAsync(
            $"/sale/offers/{offerId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Price Management

    /// <summary>
    /// Modify the Buy Now price in an offer
    /// </summary>
    public async Task<ChangePrice> ChangePriceAsync(
        string offerId,
        string commandId,
        ChangePriceWithoutOutput request,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Changing price for offer {offerId}");
        return await _httpClient.PutAsync<ChangePriceWithoutOutput, ChangePrice>(
            $"/offers/{offerId}/change-price-commands/{commandId}",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Batch Operations

    /// <summary>
    /// Batch offer publish / unpublish
    /// </summary>
    public async Task<GeneralReport> ChangePublicationStatusAsync(
        string commandId,
        PublicationChangeCommandDto request,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Changing publication status with command {commandId}");
        return await _httpClient.PutAsync<PublicationChangeCommandDto, GeneralReport>(
            $"/sale/offer-publication-commands/{commandId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Get publish command summary
    /// </summary>
    public async Task<GeneralReport> GetPublicationReportAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting publication report for command {commandId}");
        return await _httpClient.GetAsync<GeneralReport>(
            $"/sale/offer-publication-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Get publish command detailed report
    /// </summary>
    public async Task<TaskReport> GetPublicationTasksAsync(
        string commandId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting publication tasks for command {commandId}");
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue) queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue) queryParams["offset"] = offset.Value.ToString();

        return await _httpClient.GetAsync<TaskReport>(
            $"/sale/offer-publication-commands/{commandId}/tasks",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Offer Translations

    /// <summary>
    /// Get offer translations
    /// </summary>
    public async Task<OfferTranslations> GetOfferTranslationsAsync(
        string offerId,
        string? language = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting translations for offer {offerId}");
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(language))
            queryParams["language"] = language;

        return await _httpClient.GetAsync<OfferTranslations>(
            $"/sale/offers/{offerId}/translations",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Update offer translation
    /// </summary>
    public async Task UpdateOfferTranslationAsync(
        string offerId,
        string language,
        ManualTranslationUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Updating translation for offer {offerId}, language {language}");
        await _httpClient.PatchAsync<ManualTranslationUpdateRequest, object>(
            $"/sale/offers/{offerId}/translations/{language}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Delete offer translation
    /// </summary>
    public async Task DeleteOfferTranslationAsync(
        string offerId,
        string language,
        string? element = null,
        string? productsId = null,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Deleting translation for offer {offerId}, language {language}");
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(element))
            queryParams["element"] = element;
        if (!string.IsNullOrEmpty(productsId))
            queryParams["products.id"] = productsId;

        await _httpClient.DeleteAsync(
            $"/sale/offers/{offerId}/translations/{language}",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Rating & Parameters

    /// <summary>
    /// Gets the aggregated rating for an offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Offer rating information.</returns>
    public Task<OfferRating> GetOfferRatingAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<OfferRating>(
            $"/sale/offers/{offerId}/rating",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets offers that have missing (unfilled) parameters.
    /// </summary>
    /// <param name="offerIds">Optional offer identifiers to filter by.</param>
    /// <param name="parameterType">Optional parameter type filter.</param>
    /// <param name="offset">Number of elements to skip.</param>
    /// <param name="limit">Maximum number of elements to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Offers with their unfilled parameters.</returns>
    public Task<UnfilledParametersResponse> GetOffersUnfilledParametersAsync(
        IEnumerable<string>? offerIds = null,
        string? parameterType = null,
        int? offset = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<string>();
        if (offerIds != null)
            query.AddRange(offerIds.Select(id => $"offer.id={Uri.EscapeDataString(id)}"));
        if (!string.IsNullOrEmpty(parameterType))
            query.Add($"parameterType={Uri.EscapeDataString(parameterType)}");
        if (offset.HasValue)
            query.Add($"offset={offset.Value}");
        if (limit.HasValue)
            query.Add($"limit={limit.Value}");

        var endpoint = query.Count > 0
            ? $"/sale/offers/unfilled-parameters?{string.Join("&", query)}"
            : "/sale/offers/unfilled-parameters";

        return _httpClient.GetAsync<UnfilledParametersResponse>(endpoint, null, cancellationToken);
    }

    #endregion
}
