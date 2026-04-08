using AllegroApi.Http;
using AllegroApi.Models.Listing;

namespace AllegroApi.Clients;

/// <summary>
/// Provides methods for searching and browsing public Allegro offers.
/// Rate limited to 270 requests per second.
/// Requires verified application access.
/// </summary>
public class ListingClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListingClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making API requests.</param>
    public ListingClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Searches for offers based on the provided parameters.
    /// At least one of: phrase, seller ID, or category ID is required.
    /// Rate limit: 270 requests per second.
    /// </summary>
    /// <param name="searchParams">The search parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The listing response with offers and filters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when searchParams is null.</exception>
    /// <exception cref="ArgumentException">Thrown when no phrase, seller ID, or category ID is provided.</exception>
    public System.Threading.Tasks.Task<ListingResponse> GetListingAsync(
        ListingSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(searchParams);

        // Validate that at least one required parameter is provided
        if (string.IsNullOrWhiteSpace(searchParams.Phrase) &&
            string.IsNullOrWhiteSpace(searchParams.CategoryId) &&
            (searchParams.SellerIds == null || searchParams.SellerIds.Count == 0) &&
            (searchParams.SellerLogins == null || searchParams.SellerLogins.Count == 0))
        {
            throw new ArgumentException(
                "At least one of: phrase, categoryId, sellerIds, or sellerLogins must be provided.",
                nameof(searchParams));
        }

        var queryParams = new Dictionary<string, string>();

        // Add basic search parameters
        if (!string.IsNullOrWhiteSpace(searchParams.CategoryId))
        {
            queryParams["category.id"] = searchParams.CategoryId;
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Phrase))
        {
            queryParams["phrase"] = searchParams.Phrase;
        }

        // Add seller filters
        if (searchParams.SellerIds != null && searchParams.SellerIds.Count > 0)
        {
            foreach (var sellerId in searchParams.SellerIds)
            {
                queryParams["seller.id"] = sellerId;
            }
        }

        if (searchParams.SellerLogins != null && searchParams.SellerLogins.Count > 0)
        {
            foreach (var login in searchParams.SellerLogins)
            {
                queryParams["seller.login"] = login;
            }
        }

        // Add optional parameters
        if (!string.IsNullOrWhiteSpace(searchParams.MarketplaceId))
        {
            queryParams["marketplaceId"] = searchParams.MarketplaceId;
        }

        if (!string.IsNullOrWhiteSpace(searchParams.ShippingCountry))
        {
            queryParams["shipping.country"] = searchParams.ShippingCountry;
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Currency))
        {
            queryParams["currency"] = searchParams.Currency;
        }

        if (!string.IsNullOrWhiteSpace(searchParams.SearchMode))
        {
            queryParams["searchMode"] = searchParams.SearchMode;
        }

        if (searchParams.Offset.HasValue)
        {
            queryParams["offset"] = searchParams.Offset.Value.ToString();
        }

        if (searchParams.Limit.HasValue)
        {
            queryParams["limit"] = searchParams.Limit.Value.ToString();
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Sort))
        {
            queryParams["sort"] = searchParams.Sort;
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Include))
        {
            queryParams["include"] = searchParams.Include;
        }

        if (searchParams.Fallback.HasValue)
        {
            queryParams["fallback"] = searchParams.Fallback.Value.ToString().ToLowerInvariant();
        }

        // Add dynamic filters
        if (searchParams.DynamicFilters != null)
        {
            foreach (var filter in searchParams.DynamicFilters)
            {
                queryParams[filter.Key] = filter.Value;
            }
        }

        return _httpClient.GetAsync<ListingResponse>(
            "/offers/listing",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Searches for offers by phrase.
    /// Convenience method for simple text searches.
    /// </summary>
    /// <param name="phrase">The search phrase.</param>
    /// <param name="offset">The offset (0-599).</param>
    /// <param name="limit">The limit (1-60).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The listing response with offers.</returns>
    /// <exception cref="ArgumentException">Thrown when phrase is null or whitespace.</exception>
    public System.Threading.Tasks.Task<ListingResponse> SearchByPhraseAsync(
        string phrase,
        int offset = 0,
        int limit = 60,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            throw new ArgumentException("Phrase cannot be null or whitespace.", nameof(phrase));
        }

        var searchParams = new ListingSearchParams
        {
            Phrase = phrase,
            Offset = offset,
            Limit = limit
        };

        return GetListingAsync(searchParams, cancellationToken);
    }

    /// <summary>
    /// Searches for offers in a specific category.
    /// Convenience method for category browsing.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="offset">The offset (0-599).</param>
    /// <param name="limit">The limit (1-60).</param>
    /// <param name="sort">The sort order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The listing response with offers.</returns>
    /// <exception cref="ArgumentException">Thrown when categoryId is null or whitespace.</exception>
    public System.Threading.Tasks.Task<ListingResponse> SearchByCategoryAsync(
        string categoryId,
        int offset = 0,
        int limit = 60,
        string? sort = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            throw new ArgumentException("CategoryId cannot be null or whitespace.", nameof(categoryId));
        }

        var searchParams = new ListingSearchParams
        {
            CategoryId = categoryId,
            Offset = offset,
            Limit = limit,
            Sort = sort
        };

        return GetListingAsync(searchParams, cancellationToken);
    }

    /// <summary>
    /// Searches for offers from a specific seller.
    /// Convenience method for seller's offers listing.
    /// </summary>
    /// <param name="sellerId">The seller ID.</param>
    /// <param name="offset">The offset (0-599).</param>
    /// <param name="limit">The limit (1-60).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The listing response with offers.</returns>
    /// <exception cref="ArgumentException">Thrown when sellerId is null or whitespace.</exception>
    public System.Threading.Tasks.Task<ListingResponse> SearchBySellerAsync(
        string sellerId,
        int offset = 0,
        int limit = 60,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sellerId))
        {
            throw new ArgumentException("SellerId cannot be null or whitespace.", nameof(sellerId));
        }

        var searchParams = new ListingSearchParams
        {
            SellerIds = new List<string> { sellerId },
            Offset = offset,
            Limit = limit
        };

        return GetListingAsync(searchParams, cancellationToken);
    }
}
