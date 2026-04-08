using AllegroApi.Http;
using AllegroApi.Models.Pricing;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.Clients;

/// <summary>
/// Client for pricing operations - fee calculations and quotes
/// </summary>
public class PricingClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the PricingClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public PricingClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Calculate fee and commission for an offer before listing.
    /// Provides information about listing fees, commission fees, and other charges.
    /// Limited to 25 requests per second per user.
    /// </summary>
    /// <param name="request">Offer details for fee calculation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Fee preview with commissions and quotes</returns>
    public async Task<FeePreviewResponse?> GetOfferFeePreviewAsync(
        FeePreviewRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await _httpClient.PostAsync<FeePreviewRequest, FeePreviewResponse>(
            "/pricing/offer-fee-preview",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets current offer quotes (listing and promo fees cycles) for authenticated user and list of offers.
    /// Maximum 20 offer IDs can be provided at once.
    /// </summary>
    /// <param name="offerIds">List of offer identifiers (maximum 20).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current offer quotes with fee cycles.</returns>
    public async Task<OfferQuotesDto?> GetOfferQuotesAsync(
        List<string> offerIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerIds);

        if (offerIds.Count == 0 || offerIds.Count > 20)
            throw new ArgumentException("Offer IDs list must contain between 1 and 20 items.", nameof(offerIds));

        var queryParams = new Dictionary<string, string>();
        for (int i = 0; i < offerIds.Count; i++)
        {
            queryParams[$"offer.id"] = offerIds[i];
        }

        return await _httpClient.GetAsync<OfferQuotesDto>(
            "/pricing/offer-quotes",
            queryParams,
            cancellationToken);
    }
}
