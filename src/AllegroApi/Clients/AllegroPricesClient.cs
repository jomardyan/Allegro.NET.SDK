using AllegroApi.Http;
using AllegroApi.Models.Pricing;

namespace AllegroApi.Clients;

/// <summary>
/// Client for Allegro Prices and Alle Discount operations.
/// Manages automated pricing features and discount campaigns.
/// </summary>
public class AllegroPricesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AllegroPricesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public AllegroPricesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Allegro Prices - Consent Management

    /// <summary>
    /// Gets the current Allegro Prices consent state for an offer across all marketplaces.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Consent status for the offer on each marketplace.</returns>
    public System.Threading.Tasks.Task<AllegroPricesOfferConsentResponse> GetOfferConsentAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<AllegroPricesOfferConsentResponse>(
            $"/sale/allegro-prices-offer-consents/{offerId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates the Allegro Prices consent for an offer on chosen marketplaces.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="request">Consent update request with status per marketplace.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated consent status.</returns>
    public System.Threading.Tasks.Task<AllegroPricesOfferConsentResponse> UpdateOfferConsentAsync(
        string offerId,
        AllegroPricesOfferConsentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<AllegroPricesOfferConsentRequest, AllegroPricesOfferConsentResponse>(
            $"/sale/allegro-prices-offer-consents/{offerId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Checks if the account is eligible for the Allegro Prices program.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Eligibility status with reason if not eligible.</returns>
    public System.Threading.Tasks.Task<AllegroPricesAccountEligibility> GetAccountEligibilityAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AllegroPricesAccountEligibility>(
            "/sale/allegro-prices-account-eligibility",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the global Allegro Prices consent status for the account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account-level consent status per marketplace.</returns>
    public System.Threading.Tasks.Task<AllegroPricesAccountConsent> GetAccountConsentAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AllegroPricesAccountConsent>(
            "/sale/allegro-prices-account-consent",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates the global Allegro Prices consent for the account.
    /// </summary>
    /// <param name="request">Consent update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated account consent status.</returns>
    public System.Threading.Tasks.Task<AllegroPricesAccountConsent> UpdateAccountConsentAsync(
        AllegroPricesAccountConsentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<AllegroPricesAccountConsentRequest, AllegroPricesAccountConsent>(
            "/sale/allegro-prices-account-consent",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Alle Discount - Campaign Management

    /// <summary>
    /// Gets the list of available Alle Discount campaigns.
    /// </summary>
    /// <param name="marketplaceId">Optional marketplace filter (e.g., "allegro-pl").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of discount campaigns.</returns>
    public System.Threading.Tasks.Task<AlleDiscountCampaigns> GetCampaignsAsync(
        string? marketplaceId = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = marketplaceId != null
            ? new Dictionary<string, string> { ["marketplaceId"] = marketplaceId }
            : null;

        return _httpClient.GetAsync<AlleDiscountCampaigns>(
            "/sale/alle-discount/campaigns",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets the list of offers eligible for a specific campaign.
    /// </summary>
    /// <param name="campaignId">Campaign identifier.</param>
    /// <param name="limit">Maximum number of results (1-1000).</param>
    /// <param name="offset">Offset for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of eligible offer identifiers.</returns>
    public System.Threading.Tasks.Task<AlleDiscountEligibleOffers> GetEligibleOffersAsync(
        string campaignId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(campaignId);

        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue) queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue) queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<AlleDiscountEligibleOffers>(
            $"/sale/alle-discount/{campaignId}/eligible-offers",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the list of offers already submitted to a specific campaign.
    /// </summary>
    /// <param name="campaignId">Campaign identifier.</param>
    /// <param name="limit">Maximum number of results (1-1000).</param>
    /// <param name="offset">Offset for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of submitted offer identifiers.</returns>
    public System.Threading.Tasks.Task<AlleDiscountSubmittedOffers> GetSubmittedOffersAsync(
        string campaignId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(campaignId);

        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue) queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue) queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<AlleDiscountSubmittedOffers>(
            $"/sale/alle-discount/{campaignId}/submitted-offers",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Submits offers to an Alle Discount campaign. Creates an asynchronous command.
    /// </summary>
    /// <param name="request">Submit request with campaign ID and offer IDs.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command response with command ID for status tracking.</returns>
    public System.Threading.Tasks.Task<AlleDiscountCommandResponse> SubmitOffersAsync(
        AlleDiscountSubmitOffersRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<AlleDiscountSubmitOffersRequest, AlleDiscountCommandResponse>(
            "/sale/alle-discount/submit-offer-commands",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status of a submit offers command.
    /// </summary>
    /// <param name="commandId">Command identifier returned from SubmitOffersAsync.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command status (RUNNING, SUCCESS, FAILED).</returns>
    public System.Threading.Tasks.Task<AlleDiscountCommandResponse> GetSubmitCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<AlleDiscountCommandResponse>(
            $"/sale/alle-discount/submit-offer-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Withdraws offers from an Alle Discount campaign. Creates an asynchronous command.
    /// </summary>
    /// <param name="request">Withdraw request with campaign ID and offer IDs.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command response with command ID for status tracking.</returns>
    public System.Threading.Tasks.Task<AlleDiscountCommandResponse> WithdrawOffersAsync(
        AlleDiscountWithdrawOffersRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<AlleDiscountWithdrawOffersRequest, AlleDiscountCommandResponse>(
            "/sale/alle-discount/withdraw-offer-commands",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status of a withdraw offers command.
    /// </summary>
    /// <param name="commandId">Command identifier returned from WithdrawOffersAsync.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command status (RUNNING, SUCCESS, FAILED).</returns>
    public System.Threading.Tasks.Task<AlleDiscountCommandResponse> GetWithdrawCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<AlleDiscountCommandResponse>(
            $"/sale/alle-discount/withdraw-offer-commands/{commandId}",
            null,
            cancellationToken);
    }

    #endregion
}
