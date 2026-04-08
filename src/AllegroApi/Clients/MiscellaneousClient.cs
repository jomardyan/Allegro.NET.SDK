using AllegroApi.Http;
using AllegroApi.Models.Miscellaneous;

namespace AllegroApi.Clients;

/// <summary>
/// Client for miscellaneous API endpoints (charity, bidding, affiliate, deposits, compatibility).
/// </summary>
public class MiscellaneousClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the MiscellaneousClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public MiscellaneousClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Charity

    /// <summary>
    /// Gets active charity campaigns.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of charity campaigns.</returns>
    public System.Threading.Tasks.Task<CharityCampaignsList> GetCharityCampaignsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<CharityCampaignsList>(
            "/charity/campaigns",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Searches for fundraising campaigns by name or organization prefix.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="phrase">Fundraising campaign name or organization name prefix to search for.</param>
    /// <param name="limit">Maximum number of returned results (1-100, default: 20).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of matching fundraising campaigns.</returns>
    /// <exception cref="AllegroBadRequestException">Invalid or missing query parameters.</exception>
    /// <exception cref="AllegroAuthenticationException">Unauthorized.</exception>
    /// <exception cref="AllegroAuthorizationException">Forbidden.</exception>
    public System.Threading.Tasks.Task<FundraisingCampaigns> SearchFundraisingCampaignsAsync(
        string phrase,
        int limit = 20,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(phrase);
        var queryParams = new Dictionary<string, string>
        {
            ["phrase"] = phrase,
            ["limit"] = limit.ToString()
        };
        return _httpClient.GetAsync<FundraisingCampaigns>(
            "/charity/fundraising-campaigns",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Bidding

    /// <summary>
    /// Gets bidding offers (auction-style offers).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of bidding offers.</returns>
    public System.Threading.Tasks.Task<BiddingOffersList> GetBiddingOffersAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<BiddingOffersList>(
            "/sale/offers/bidding",
            null,
            cancellationToken);
    }

    #endregion

    #region Affiliate

    /// <summary>
    /// Gets affiliate program information.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Affiliate program details.</returns>
    public System.Threading.Tasks.Task<AffiliateProgramInfo> GetAffiliateProgramInfoAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AffiliateProgramInfo>(
            "/affiliate/program",
            null,
            cancellationToken);
    }

    #endregion

    #region Deposits

    /// <summary>
    /// Gets current deposit information for the seller account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deposit information.</returns>
    public System.Threading.Tasks.Task<DepositInfo> GetDepositInfoAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<DepositInfo>(
            "/payments/deposit",
            null,
            cancellationToken);
    }

    #endregion

    #region Product Compatibility

    /// <summary>
    /// Gets product compatibility information.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product compatibility details.</returns>
    public System.Threading.Tasks.Task<ProductCompatibility> GetProductCompatibilityAsync(
        string productId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productId);
        return _httpClient.GetAsync<ProductCompatibility>(
            $"/sale/products/{productId}/compatibility",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets list of categories where compatibility list is supported.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of categories that support compatibility lists.</returns>
    public System.Threading.Tasks.Task<CompatibilityListSupportedCategoriesDto> GetCompatibilityListSupportedCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<CompatibilityListSupportedCategoriesDto>(
            "/sale/compatibility-list/supported-categories",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets suggested compatibility list for given offer or product.
    /// </summary>
    /// <param name="offerId">Offer identifier (optional).</param>
    /// <param name="productId">Product identifier (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Suggested compatibility list.</returns>
    public System.Threading.Tasks.Task<CompatibilityList> GetCompatibilityListSuggestionsAsync(
        string? offerId = null,
        string? productId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(offerId))
            queryParams["offerId"] = offerId;
        if (!string.IsNullOrEmpty(productId))
            queryParams["productId"] = productId;

        return _httpClient.GetAsync<CompatibilityList>(
            "/sale/compatibility-list-suggestions",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets list of compatible product groups.
    /// </summary>
    /// <param name="type">Type of compatible products (optional).</param>
    /// <param name="offset">Number of elements to skip (default: 0).</param>
    /// <param name="limit">Number of elements to return (default: 60, max: 1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of compatible product groups.</returns>
    public System.Threading.Tasks.Task<CompatibleProductsGroupsDto> GetCompatibleProductGroupsAsync(
        string? type = null,
        int offset = 0,
        int limit = 60,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };
        if (!string.IsNullOrEmpty(type))
            queryParams["type"] = type;

        return _httpClient.GetAsync<CompatibleProductsGroupsDto>(
            "/sale/compatible-products/groups",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets list of compatible products.
    /// </summary>
    /// <param name="type">Type of compatible products (optional).</param>
    /// <param name="groupId">Group identifier (optional).</param>
    /// <param name="phrase">Search phrase (optional).</param>
    /// <param name="offset">Number of elements to skip (default: 0).</param>
    /// <param name="limit">Number of elements to return (default: 60, max: 1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of compatible products.</returns>
    public System.Threading.Tasks.Task<CompatibleProductsListDto> GetCompatibleProductsAsync(
        string? type = null,
        string? groupId = null,
        string? phrase = null,
        int offset = 0,
        int limit = 60,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };
        if (!string.IsNullOrEmpty(type))
            queryParams["type"] = type;
        if (!string.IsNullOrEmpty(groupId))
            queryParams["groupId"] = groupId;
        if (!string.IsNullOrEmpty(phrase))
            queryParams["phrase"] = phrase;

        return _httpClient.GetAsync<CompatibleProductsListDto>(
            "/sale/compatible-products",
            queryParams,
            cancellationToken);
    }

    #endregion

    #region Offer Events

    /// <summary>
    /// Gets events about the seller's offers from the last 24 hours.
    /// Supports events: OFFER_ACTIVATED, OFFER_CHANGED.
    /// Rate limit: 900 requests per 60 seconds.
    /// </summary>
    /// <param name="from">Event identifier to start from (optional).</param>
    /// <param name="limit">Number of events to return (max: 1000, default: 100).</param>
    /// <param name="type">Filter by event type (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of offer events.</returns>
    public System.Threading.Tasks.Task<SellerOfferEventsResponse> GetOfferEventsAsync(
        string? from = null,
        int? limit = null,
        string? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(from))
            queryParams["from"] = from;
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (!string.IsNullOrEmpty(type))
            queryParams["type"] = type;

        return _httpClient.GetAsync<SellerOfferEventsResponse>(
            "/sale/offer-events",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    #endregion

    #region Deposits

    /// <summary>
    /// Gets deposit types available when creating an offer.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available deposit types.</returns>
    public System.Threading.Tasks.Task<DepositTypeResponse> GetDepositTypesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<DepositTypeResponse>(
            "/deposit/types",
            null,
            cancellationToken);
    }

    #endregion

    #region CPS Conversions

    /// <summary>
    /// Lists CPS (Cost Per Sale) conversions for specific filters.
    /// Rate limit: Standard API limits apply.
    /// </summary>
    /// <param name="orderCreatedAtGte">Minimum order creation date (optional).</param>
    /// <param name="orderCreatedAtLte">Maximum order creation date (optional).</param>
    /// <param name="lastModifiedAtGte">Minimum last modification date (optional).</param>
    /// <param name="lastModifiedAtLte">Maximum last modification date (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of CPS conversions.</returns>
    public System.Threading.Tasks.Task<CpsConversionResponse> GetCpsConversionsAsync(
        DateTime? orderCreatedAtGte = null,
        DateTime? orderCreatedAtLte = null,
        DateTime? lastModifiedAtGte = null,
        DateTime? lastModifiedAtLte = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (orderCreatedAtGte.HasValue)
            queryParams["orderCreatedAt.gte"] = orderCreatedAtGte.Value.ToString("o");
        if (orderCreatedAtLte.HasValue)
            queryParams["orderCreatedAt.lte"] = orderCreatedAtLte.Value.ToString("o");
        if (lastModifiedAtGte.HasValue)
            queryParams["lastModifiedAt.gte"] = lastModifiedAtGte.Value.ToString("o");
        if (lastModifiedAtLte.HasValue)
            queryParams["lastModifiedAt.lte"] = lastModifiedAtLte.Value.ToString("o");

        return _httpClient.GetAsync<CpsConversionResponse>(
            "/affiliate/conversions/cps",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    #endregion

    #region Bidding

    /// <summary>
    /// Places a bid in an auction.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="request">Bid details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Bid placement result.</returns>
    public System.Threading.Tasks.Task<PlaceBidResponse> PlaceBidAsync(
        string offerId,
        PlaceBidRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<PlaceBidRequest, PlaceBidResponse>(
            $"/bidding/offers/{offerId}/bid",
            request,
            null,
            cancellationToken);
    }

    #endregion
}
