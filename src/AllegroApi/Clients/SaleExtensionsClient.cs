using AllegroApi.Http;
using AllegroApi.Models.SaleExtensions;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing additional sale features (bundles, promotions, tags).
/// </summary>
public class SaleExtensionsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the SaleExtensionsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public SaleExtensionsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Bundles

    /// <summary>
    /// Gets a list of offer bundles.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of bundles.</returns>
    public System.Threading.Tasks.Task<BundlesList> GetBundlesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<BundlesList>(
            "/sale/bundles",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new offer bundle.
    /// </summary>
    /// <param name="bundle">Bundle details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created bundle.</returns>
    public System.Threading.Tasks.Task<Bundle> CreateBundleAsync(
        Bundle bundle,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bundle);
        return _httpClient.PostAsync<Bundle, Bundle>(
            "/sale/bundles",
            bundle,
            null,
            cancellationToken);
    }

    #endregion

    #region Loyalty Promotions

    /// <summary>
    /// Gets a list of loyalty promotions.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of loyalty promotions.</returns>
    public System.Threading.Tasks.Task<LoyaltyPromotionsList> GetLoyaltyPromotionsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<LoyaltyPromotionsList>(
            "/sale/loyalty/promotions",
            null,
            cancellationToken);
    }

    #endregion

    #region Badge Campaigns

    /// <summary>
    /// Gets a list of available badge campaigns.
    /// Badge campaigns are another way to promote your offers.
    /// </summary>
    /// <param name="marketplaceId">Marketplace identifier (e.g., "allegro-pl").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of badge campaigns.</returns>
    public System.Threading.Tasks.Task<GetBadgeCampaignsList> GetBadgeCampaignsAsync(
        string? marketplaceId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(marketplaceId))
            queryParams["marketplace.id"] = marketplaceId;

        return _httpClient.GetAsync<GetBadgeCampaignsList>(
            "/sale/badge-campaigns",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    #endregion

    #region Offer Tags

    /// <summary>
    /// Gets a list of offer tags.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of offer tags.</returns>
    public System.Threading.Tasks.Task<OfferTagsList> GetOfferTagsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<OfferTagsList>(
            "/sale/offer-tags",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new offer tag.
    /// </summary>
    /// <param name="request">Tag details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created tag.</returns>
    public System.Threading.Tasks.Task<OfferTag> CreateOfferTagAsync(
        CreateOfferTagRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CreateOfferTagRequest, OfferTag>(
            "/sale/offer-tags",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Assigns a tag to an offer.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="request">Tag assignment details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task AssignTagToOfferAsync(
        string offerId,
        AssignTagRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<AssignTagRequest>(
            $"/sale/offers/{offerId}/tags",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Turnover Discount

    /// <summary>
    /// Gets the list of turnover discounts for all supported marketplaces.
    /// </summary>
    /// <param name="marketplaceIds">Optional list of marketplace identifiers to filter results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of turnover discounts per marketplace.</returns>
    public System.Threading.Tasks.Task<List<TurnoverDiscountDto>> GetTurnoverDiscountsAsync(
        IEnumerable<string>? marketplaceIds = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (marketplaceIds != null)
        {
            var ids = marketplaceIds.ToList();
            if (ids.Count > 0)
            {
                // Build URL manually to support repeated query keys (explode=true per swagger)
                var qs = string.Join("&", ids.Select(id => $"marketplaceId={Uri.EscapeDataString(id)}"));
                return _httpClient.GetAsync<List<TurnoverDiscountDto>>(
                    $"/sale/turnover-discount?{qs}",
                    null,
                    cancellationToken);
            }
        }

        return _httpClient.GetAsync<List<TurnoverDiscountDto>>(
            "/sale/turnover-discount",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates or modifies a turnover discount for a specific marketplace.
    /// </summary>
    /// <param name="marketplaceId">Marketplace identifier (currently only "allegro-business-cz" is supported).</param>
    /// <param name="request">Turnover discount configuration with thresholds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated turnover discount details.</returns>
    public System.Threading.Tasks.Task<TurnoverDiscountDto> CreateOrModifyTurnoverDiscountAsync(
        string marketplaceId,
        TurnoverDiscountRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(marketplaceId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<TurnoverDiscountRequest, TurnoverDiscountDto>(
            $"/sale/turnover-discount/{marketplaceId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deactivates turnover discount for a marketplace.
    /// </summary>
    /// <param name="marketplaceId">Marketplace identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated turnover discount details after deactivation.</returns>
    public System.Threading.Tasks.Task<TurnoverDiscountDto> DeactivateTurnoverDiscountAsync(
        string marketplaceId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(marketplaceId);
        return _httpClient.PutAsync<object, TurnoverDiscountDto>(
            $"/sale/turnover-discount/{marketplaceId}/deactivate",
            new { },
            null,
            cancellationToken);
    }

    #endregion

    #region Promotion Packages

    /// <summary>
    /// Gets available offer promotion packages for all supported marketplaces.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Available promotion packages including base and extra packages.</returns>
    public System.Threading.Tasks.Task<AvailablePromotionPackages> GetPromotionPackagesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AvailablePromotionPackages>(
            "/sale/offer-promotion-packages",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the promo options (promotion packages) currently assigned to a single offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Promo options assigned to the offer.</returns>
    public System.Threading.Tasks.Task<OfferPromoOptions> GetOfferPromoOptionsAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<OfferPromoOptions>(
            $"/sale/offers/{offerId}/promo-options",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Modifies promotion packages on a single offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="modifications">Promotion package modifications to apply.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated promo options for the offer.</returns>
    public System.Threading.Tasks.Task<OfferPromoOptions> ModifyOfferPromoOptionsAsync(
        string offerId,
        PromoOptionsModifications modifications,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(modifications);
        return _httpClient.PostAsync<PromoOptionsModifications, OfferPromoOptions>(
            $"/sale/offers/{offerId}/promo-options-modification",
            modifications,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets promo options for all seller offers (paginated).
    /// </summary>
    /// <param name="limit">Limit of promo options per page (1-5000, default 5000).</param>
    /// <param name="offset">Offset for pagination (default 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Promo options for seller offers with pagination info.</returns>
    public System.Threading.Tasks.Task<OfferPromoOptionsForSeller> GetPromoOptionsForSellerOffersAsync(
        int? limit = null,
        long? offset = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<OfferPromoOptionsForSeller>(
            "/sale/offers/promo-options",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a batch command to modify promotion packages on multiple offers.
    /// </summary>
    /// <param name="commandId">Command identifier (must be a UUID).</param>
    /// <param name="command">Batch modification command with offer criteria and modifications.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Summary report for the batch command.</returns>
    public System.Threading.Tasks.Task<PromoGeneralReport> CreatePromoOptionsCommandAsync(
        string commandId,
        PromoOptionsCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PutAsync<PromoOptionsCommand, PromoGeneralReport>(
            $"/sale/offers/promo-options-commands/{commandId}",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the summary result of a batch promo options modification command.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Summary report showing number of successful and failed modifications.</returns>
    public System.Threading.Tasks.Task<PromoGeneralReport> GetPromoOptionsCommandResultAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<PromoGeneralReport>(
            $"/sale/offers/promo-options-commands/{commandId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Additional Services

    /// <summary>
    /// Gets the list of additional services groups for the user's account.
    /// Use this to retrieve groups with additional services that can be assigned to offers.
    /// </summary>
    /// <param name="offset">The offset of elements in the response (optional, default 0).</param>
    /// <param name="limit">The limit of elements in the response (optional, default 100, max 1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of additional services groups.</returns>
    public System.Threading.Tasks.Task<AdditionalServicesGroups> GetAdditionalServicesGroupsAsync(
        int? offset = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (offset.HasValue)
        {
            queryParams["offset"] = offset.Value.ToString();
        }
        if (limit.HasValue)
        {
            queryParams["limit"] = limit.Value.ToString();
        }

        return _httpClient.GetAsync<AdditionalServicesGroups>(
            "/sale/offer-additional-services/groups",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new additional services group.
    /// Use this to create a group of additional services that can be assigned to offers.
    /// </summary>
    /// <param name="request">The additional services group details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created additional services group.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    public System.Threading.Tasks.Task<AdditionalServicesGroupResponse> CreateAdditionalServicesGroupAsync(
        AdditionalServicesGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PostAsync<AdditionalServicesGroupRequest, AdditionalServicesGroupResponse>(
            "/sale/offer-additional-services/groups",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the additional services definitions grouped by categories.
    /// Use this to discover which additional services are available on the marketplace.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Categories with available additional service definitions.</returns>
    public System.Threading.Tasks.Task<CategoriesResponse> GetAdditionalServicesCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<CategoriesResponse>(
            "/sale/offer-additional-services/categories",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets translations for a specified additional services group.
    /// Use this to retrieve translations in all languages or filter by a specific language.
    /// </summary>
    /// <param name="groupId">The additional service group ID.</param>
    /// <param name="language">IETF language tag (optional, e.g., "en-US").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Translation information for the group.</returns>
    /// <exception cref="ArgumentNullException">Thrown when groupId is null.</exception>
    public System.Threading.Tasks.Task<AdditionalServiceGroupTranslationResponse> GetAdditionalServiceGroupTranslationsAsync(
        string groupId,
        string? language = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(groupId);

        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(language))
        {
            queryParams["language"] = language;
        }

        return _httpClient.GetAsync<AdditionalServiceGroupTranslationResponse>(
            $"/sale/offer-additional-services/groups/{groupId}/translations",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Creates or updates translations for a specified group and language.
    /// Use this to provide translations for additional service descriptions.
    /// It's allowed to provide an incomplete list of services that belong to the group.
    /// </summary>
    /// <param name="groupId">The additional service group ID.</param>
    /// <param name="language">IETF language tag (e.g., "pl-PL").</param>
    /// <param name="request">The translation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created/updated translation information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when groupId, language, or request is null.</exception>
    public System.Threading.Tasks.Task<AdditionalServiceGroupTranslationPatchResponse> UpdateAdditionalServiceGroupTranslationAsync(
        string groupId,
        string language,
        AdditionalServicesGroupTranslationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(groupId);
        ArgumentNullException.ThrowIfNull(language);
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PatchAsync<AdditionalServicesGroupTranslationRequest, AdditionalServiceGroupTranslationPatchResponse>(
            $"/sale/offer-additional-services/groups/{groupId}/translations/{language}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a translation for a specified group and language.
    /// Use this to remove translations that are no longer needed.
    /// </summary>
    /// <param name="groupId">The additional service group ID.</param>
    /// <param name="language">IETF language tag (e.g., "pl-PL").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when groupId or language is null.</exception>
    public async System.Threading.Tasks.Task DeleteAdditionalServiceGroupTranslationAsync(
        string groupId,
        string language,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(groupId);
        ArgumentNullException.ThrowIfNull(language);

        await _httpClient.DeleteAsync(
            $"/sale/offer-additional-services/groups/{groupId}/translations/{language}",
            null,
            cancellationToken);
    }

    #endregion
}
