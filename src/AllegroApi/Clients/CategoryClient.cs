using AllegroApi.Http;
using AllegroApi.Models.Categories;
using AllegroApi.Models.Tax;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.Clients;

/// <summary>
/// Client for category and parameter operations.
/// </summary>
public class CategoryClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the CategoryClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public CategoryClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Get IDs of Allegro categories. Use this to traverse the Allegro categories tree.
    /// Returns the list of the given category's children or a list of main Allegro categories.
    /// </summary>
    /// <param name="parentCategoryId">The ID of the parent category. If null, returns main categories.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of categories</returns>
    public async Task<CategoriesDto?> GetCategoriesAsync(
        string? parentCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        
        if (!string.IsNullOrWhiteSpace(parentCategoryId))
        {
            queryParams["parent.id"] = parentCategoryId;
        }

        return await _httpClient.GetAsync<CategoriesDto>(
            "/sale/categories",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Get a category by ID with full details including options and parent category information.
    /// </summary>
    /// <param name="categoryId">The category ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Category details</returns>
    public async Task<CategoryDto?> GetCategoryAsync(
        string categoryId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category ID cannot be null or empty.", nameof(categoryId));

        return await _httpClient.GetAsync<CategoryDto>(
            $"/sale/categories/{categoryId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Get parameters supported by a category. 
    /// Use this to get the list of parameters that must be provided when creating an offer in this category.
    /// </summary>
    /// <param name="categoryId">The category ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of parameters for the category</returns>
    public async Task<CategoryParameterList?> GetCategoryParametersAsync(
        string categoryId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category ID cannot be null or empty.", nameof(categoryId));

        return await _httpClient.GetAsync<CategoryParameterList>(
            $"/sale/categories/{categoryId}/parameters",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets information about changes in categories that occurred in the last 3 months.
    /// Supports events: CATEGORY_CREATED, CATEGORY_RENAMED, CATEGORY_MOVED, CATEGORY_DELETED.
    /// </summary>
    /// <param name="from">The ID of the last seen event. Returns changes after this event.</param>
    /// <param name="limit">Number of events to return (1-1000, default 100).</param>
    /// <param name="types">Types of events to filter (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of category change events.</returns>
    public System.Threading.Tasks.Task<CategoryEventsResponse> GetCategoryEventsAsync(
        string? from = null,
        int? limit = null,
        List<string>? types = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(from))
            queryParams["from"] = from;
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (types != null && types.Count > 0)
        {
            foreach (var type in types)
            {
                queryParams.Add("type", type);
            }
        }

        return _httpClient.GetAsync<CategoryEventsResponse>(
            "/sale/category-events",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets all tax settings for a category.
    /// Use this resource to receive tax settings for given category.
    /// Based on received settings you may set VAT tax settings for your offers.
    /// </summary>
    /// <param name="categoryId">An identifier of a category for which all available tax settings will be returned.</param>
    /// <param name="countryCodes">Country codes for which tax settings will be returned (PL, CZ, SK, HU). If not provided, settings for all countries will be returned.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tax settings for the given category.</returns>
    public System.Threading.Tasks.Task<CategoryTaxSettings> GetTaxSettingsForCategoryAsync(
        string categoryId,
        List<string>? countryCodes = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryId);

        var queryParams = new Dictionary<string, string>
        {
            ["category.id"] = categoryId
        };

        if (countryCodes != null && countryCodes.Count > 0)
        {
            queryParams["countryCode"] = string.Join(",", countryCodes);
        }

        return _httpClient.GetAsync<CategoryTaxSettings>(
            "/sale/tax-settings",
            queryParams,
            cancellationToken);
    }
}
