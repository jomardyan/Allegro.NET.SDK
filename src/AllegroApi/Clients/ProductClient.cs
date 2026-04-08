using AllegroApi.Http;
using AllegroApi.Models.Products;
using Microsoft.Extensions.Logging;

namespace AllegroApi.Clients;

/// <summary>
/// Client for product-related operations in Allegro API
/// </summary>
public class ProductClient
{
    private readonly AllegroHttpClient _httpClient;
    private readonly ILogger<ProductClient>? _logger;

    /// <summary>
    /// Initializes a new instance of the ProductClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    /// <param name="logger">Optional logger for diagnostics.</param>
    public ProductClient(AllegroHttpClient httpClient, ILogger<ProductClient>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
    }

    /// <summary>
    /// Search for products
    /// </summary>
    public async Task<ProductsSearchResult> SearchProductsAsync(
        ProductSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Searching products");
        
        if (string.IsNullOrEmpty(searchParams.Ean) && string.IsNullOrEmpty(searchParams.Phrase))
        {
            throw new ArgumentException("Either Ean or Phrase parameter is required");
        }

        return await _httpClient.GetAsync<ProductsSearchResult>(
            "/sale/products",
            searchParams.ToQueryParams(),
            cancellationToken);
    }

    /// <summary>
    /// Get product details by ID
    /// </summary>
    public async Task<Product> GetProductAsync(
        string productId,
        CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation($"Getting product {productId}");
        return await _httpClient.GetAsync<Product>(
            $"/sale/products/{productId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Proposes changes to an existing product.
    /// Limited to 100 suggestions per day for a single user.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <param name="request">Proposed changes.</param>
    /// <param name="acceptLanguage">Expected language of messages (pl-PL, en-US, etc.).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product change proposal details.</returns>
    public async Task<ProductChangeProposalDto> ProposeProductChangesAsync(
        string productId,
        ProductChangeProposalRequest request,
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productId);
        ArgumentNullException.ThrowIfNull(request);

        var queryParams = acceptLanguage != null 
            ? new Dictionary<string, string> { ["Accept-Language"] = acceptLanguage }
            : null;

        return await _httpClient.PostAsync<ProductChangeProposalRequest, ProductChangeProposalDto>(
            $"/sale/products/{productId}/change-proposals",
            request,
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets details of a specific product change proposal.
    /// </summary>
    /// <param name="changeProposalId">Change proposal identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product change proposal details.</returns>
    public async Task<ProductChangeProposalDto> GetProductChangeProposalAsync(
        string changeProposalId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changeProposalId);
        return await _httpClient.GetAsync<ProductChangeProposalDto>(
            $"/sale/products/change-proposals/{changeProposalId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Proposes a new product to the Allegro catalog.
    /// Use this to create a new product that doesn't exist in the catalog yet.
    /// Rate limit: Standard API limits apply.
    /// </summary>
    /// <param name="request">Product proposal details including name, category, images, and parameters.</param>
    /// <param name="acceptLanguage">Expected language of messages (pl-PL, en-US, uk-UA, sk-SK, cs-CZ, hu-HU).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created product details with identifier and publication status.</returns>
    /// <exception cref="AllegroBadRequestException">Invalid request data.</exception>
    /// <exception cref="AllegroConflictException">Product already exists. Location header contains existing product URL.</exception>
    /// <exception cref="AllegroUnprocessableEntityException">Request data cannot be processed.</exception>
    public async System.Threading.Tasks.Task<ProductProposalsResponse> ProposeProductAsync(
        ProductProposalsRequest request,
        string? acceptLanguage = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger?.LogInformation("Proposing new product: {ProductName}", request.Name);

        var headers = acceptLanguage != null
            ? new Dictionary<string, string> { ["Accept-Language"] = acceptLanguage }
            : null;

        return await _httpClient.PostAsync<ProductProposalsRequest, ProductProposalsResponse>(
            "/sale/product-proposals",
            request,
            headers,
            cancellationToken);
    }

    /// <summary>
    /// Gets a list of categories matching the provided name or other criteria.
    /// Useful for finding appropriate categories when creating an offer.
    /// </summary>
    /// <param name="name">Category name to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of matching categories with scores.</returns>
    public async System.Threading.Tasks.Task<MatchingCategoriesResponse> GetMatchingCategoriesAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        _logger?.LogInformation("Getting matching categories for: {Name}", name);

        var queryParams = new Dictionary<string, string>
        {
            ["name"] = name
        };

        return await _httpClient.GetAsync<MatchingCategoriesResponse>(
            "/sale/matching-categories",
            queryParams,
            cancellationToken);
    }
}
