using AllegroApi.Http;
using AllegroApi.Models.Shipping;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing shipping rates and delivery settings.
/// </summary>
public class ShippingClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ShippingClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public ShippingClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of seller's shipping rates.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of shipping rates.</returns>
    public System.Threading.Tasks.Task<ShippingRatesListResponse> GetShippingRatesAsync(CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<ShippingRatesListResponse>("/sale/shipping-rates", null, cancellationToken);
    }

    /// <summary>
    /// Creates a new shipping rates set.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="shippingRatesSet">The shipping rates set to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created shipping rates set.</returns>
    public System.Threading.Tasks.Task<ShippingRatesSet> CreateShippingRatesSetAsync(ShippingRatesSet shippingRatesSet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(shippingRatesSet);
        return _httpClient.PostAsync<ShippingRatesSet, ShippingRatesSet>("/sale/shipping-rates", shippingRatesSet, null, cancellationToken);
    }

    /// <summary>
    /// Gets a shipping rates set by ID.
    /// </summary>
    /// <param name="id">The shipping rates set ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The shipping rates set.</returns>
    public System.Threading.Tasks.Task<ShippingRatesSet> GetShippingRatesSetAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return _httpClient.GetAsync<ShippingRatesSet>($"/sale/shipping-rates/{id}", null, cancellationToken);
    }

    /// <summary>
    /// Updates a shipping rates set.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="id">The shipping rates set ID.</param>
    /// <param name="shippingRatesSet">The updated shipping rates set.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated shipping rates set.</returns>
    public System.Threading.Tasks.Task<ShippingRatesSet> UpdateShippingRatesSetAsync(string id, ShippingRatesSet shippingRatesSet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(shippingRatesSet);
        return _httpClient.PutAsync<ShippingRatesSet, ShippingRatesSet>($"/sale/shipping-rates/{id}", shippingRatesSet, null, cancellationToken);
    }

    /// <summary>
    /// Gets delivery settings for a marketplace.
    /// </summary>
    /// <param name="marketplaceId">The marketplace ID (e.g., "allegro-pl").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Delivery settings for the marketplace.</returns>
    public System.Threading.Tasks.Task<DeliverySettingsResponse> GetDeliverySettingsAsync(string marketplaceId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(marketplaceId);
        return _httpClient.GetAsync<DeliverySettingsResponse>($"/sale/delivery-settings/{marketplaceId}", null, cancellationToken);
    }

    /// <summary>
    /// Updates delivery settings for a marketplace.
    /// Rate limit: 200 requests per 60 seconds.
    /// </summary>
    /// <param name="marketplaceId">The marketplace ID (e.g., "allegro-pl").</param>
    /// <param name="settings">The updated delivery settings.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated delivery settings.</returns>
    public System.Threading.Tasks.Task<DeliverySettingsResponse> UpdateDeliverySettingsAsync(string marketplaceId, DeliverySettingsResponse settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(marketplaceId);
        ArgumentNullException.ThrowIfNull(settings);
        return _httpClient.PutAsync<DeliverySettingsResponse, DeliverySettingsResponse>($"/sale/delivery-settings/{marketplaceId}", settings, null, cancellationToken);
    }

    /// <summary>
    /// Gets available delivery methods.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available delivery methods.</returns>
    public System.Threading.Tasks.Task<DeliveryMethodsResponse> GetDeliveryMethodsAsync(CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<DeliveryMethodsResponse>("/sale/delivery-methods", null, cancellationToken);
    }
}
