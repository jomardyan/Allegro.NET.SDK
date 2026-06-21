using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllegroApi.Models.PriceAutomation;

/// <summary>
/// List of automatic pricing rules defined on the account.
/// </summary>
public record AutomaticPricingRulesResponse
{
    /// <summary>
    /// Automatic pricing rules.
    /// </summary>
    [JsonPropertyName("rules")]
    public List<AutomaticPricingRuleResponse>? Rules { get; init; }
}

/// <summary>
/// Automatic pricing rule details.
/// </summary>
public record AutomaticPricingRuleResponse
{
    /// <summary>
    /// Rule identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Rule type: EXCHANGE_RATE, FOLLOW_BY_ALLEGRO_MIN_PRICE, FOLLOW_BY_MARKET_MIN_PRICE or FOLLOW_BY_TOP_OFFER_PRICE.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Rule name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Whether this is the default rule for its type.
    /// </summary>
    [JsonPropertyName("default")]
    public bool? Default { get; init; }

    /// <summary>
    /// Date and time of the last update (ISO 8601).
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Rule configuration. The shape depends on the rule type.
    /// </summary>
    [JsonPropertyName("configuration")]
    public JsonElement? Configuration { get; init; }
}

/// <summary>
/// Request body for creating an automatic pricing rule.
/// </summary>
public record AutomaticPricingRulePostRequest
{
    /// <summary>
    /// Rule name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Rule type: EXCHANGE_RATE, FOLLOW_BY_ALLEGRO_MIN_PRICE, FOLLOW_BY_MARKET_MIN_PRICE or FOLLOW_BY_TOP_OFFER_PRICE.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Rule configuration. The shape depends on the rule type.
    /// </summary>
    [JsonPropertyName("configuration")]
    public JsonElement? Configuration { get; init; }
}

/// <summary>
/// Request body for updating an automatic pricing rule.
/// </summary>
public record AutomaticPricingRulePutRequest
{
    /// <summary>
    /// Rule name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Rule configuration. The shape depends on the rule type.
    /// </summary>
    [JsonPropertyName("configuration")]
    public JsonElement? Configuration { get; init; }
}

/// <summary>
/// Automatic pricing rules assigned to a single offer.
/// </summary>
public record OfferRules
{
    /// <summary>
    /// Rules assigned to the offer per marketplace.
    /// </summary>
    [JsonPropertyName("rules")]
    public List<OfferRuleAssignment>? Rules { get; init; }

    /// <summary>
    /// Date and time of the last update (ISO 8601).
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// A single automatic pricing rule assignment for an offer on a marketplace.
/// </summary>
public record OfferRuleAssignment
{
    /// <summary>
    /// Marketplace the assignment applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public OfferRuleMarketplace? Marketplace { get; init; }

    /// <summary>
    /// The rule reference.
    /// </summary>
    [JsonPropertyName("rule")]
    public OfferRuleReference? Rule { get; init; }

    /// <summary>
    /// Offer-level rule configuration (e.g. price range).
    /// </summary>
    [JsonPropertyName("configuration")]
    public AutomaticPricingOfferRuleConfiguration? Configuration { get; init; }

    /// <summary>
    /// Date and time of the last update (ISO 8601).
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Marketplace reference for an offer rule assignment.
/// </summary>
public record OfferRuleMarketplace
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Reference to an automatic pricing rule.
/// </summary>
public record OfferRuleReference
{
    /// <summary>
    /// Rule identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Offer-level automatic pricing rule configuration.
/// </summary>
public record AutomaticPricingOfferRuleConfiguration
{
    /// <summary>
    /// Price range that bounds the automatic pricing.
    /// </summary>
    [JsonPropertyName("priceRange")]
    public AutomaticPricingPriceRange? PriceRange { get; init; }
}

/// <summary>
/// Price range bounding an automatic pricing rule for an offer.
/// </summary>
public record AutomaticPricingPriceRange
{
    /// <summary>
    /// Range type: BASE_MARKETPLACE_CURRENCY or MARKETPLACE_CURRENCY.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Minimum allowed price.
    /// </summary>
    [JsonPropertyName("minPrice")]
    public AutomaticPricingPrice? MinPrice { get; init; }

    /// <summary>
    /// Maximum allowed price.
    /// </summary>
    [JsonPropertyName("maxPrice")]
    public AutomaticPricingPrice? MaxPrice { get; init; }
}

/// <summary>
/// A monetary amount with currency used by automatic pricing.
/// </summary>
public record AutomaticPricingPrice
{
    /// <summary>
    /// Amount as string.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// ISO 4217 currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
