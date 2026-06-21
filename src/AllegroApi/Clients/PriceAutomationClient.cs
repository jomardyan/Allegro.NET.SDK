using AllegroApi.Http;
using AllegroApi.Models.PriceAutomation;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing automatic pricing (price automation) rules.
/// </summary>
public class PriceAutomationClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the PriceAutomationClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public PriceAutomationClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets all automatic pricing rules defined on the account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of automatic pricing rules.</returns>
    public System.Threading.Tasks.Task<AutomaticPricingRulesResponse> GetRulesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AutomaticPricingRulesResponse>(
            "/sale/price-automation/rules",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new automatic pricing rule.
    /// </summary>
    /// <param name="request">Rule definition.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created rule.</returns>
    public System.Threading.Tasks.Task<AutomaticPricingRuleResponse> CreateRuleAsync(
        AutomaticPricingRulePostRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<AutomaticPricingRulePostRequest, AutomaticPricingRuleResponse>(
            "/sale/price-automation/rules",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a single automatic pricing rule by identifier.
    /// </summary>
    /// <param name="ruleId">Rule identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rule details.</returns>
    public System.Threading.Tasks.Task<AutomaticPricingRuleResponse> GetRuleAsync(
        string ruleId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ruleId);
        return _httpClient.GetAsync<AutomaticPricingRuleResponse>(
            $"/sale/price-automation/rules/{ruleId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing automatic pricing rule.
    /// </summary>
    /// <param name="ruleId">Rule identifier.</param>
    /// <param name="request">Updated rule definition.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated rule.</returns>
    public System.Threading.Tasks.Task<AutomaticPricingRuleResponse> UpdateRuleAsync(
        string ruleId,
        AutomaticPricingRulePutRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ruleId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<AutomaticPricingRulePutRequest, AutomaticPricingRuleResponse>(
            $"/sale/price-automation/rules/{ruleId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes an automatic pricing rule.
    /// </summary>
    /// <param name="ruleId">Rule identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public System.Threading.Tasks.Task DeleteRuleAsync(
        string ruleId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ruleId);
        return _httpClient.DeleteAsync(
            $"/sale/price-automation/rules/{ruleId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the automatic pricing rules assigned to a specific offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Automatic pricing rules assigned to the offer.</returns>
    public System.Threading.Tasks.Task<OfferRules> GetOfferRulesAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<OfferRules>(
            $"/sale/price-automation/offers/{offerId}/rules",
            null,
            cancellationToken);
    }
}
