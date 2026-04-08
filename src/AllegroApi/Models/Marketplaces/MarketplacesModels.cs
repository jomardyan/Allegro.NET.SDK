using System.Text.Json.Serialization;

namespace AllegroApi.Models.Marketplaces;

/// <summary>
/// Represents the list of all Allegro marketplaces.
/// </summary>
public record AllegroMarketplaces
{
    /// <summary>
    /// List of marketplaces in Allegro.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public List<MarketplaceItem> Marketplaces { get; init; } = new();
}

/// <summary>
/// Represents a single marketplace.
/// </summary>
public record MarketplaceItem
{
    /// <summary>
    /// Marketplace identifier (e.g., "allegro-pl", "allegro-cz").
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Languages available for that marketplace.
    /// </summary>
    [JsonPropertyName("languages")]
    public MarketplaceLanguages? Languages { get; init; }

    /// <summary>
    /// Currencies available for that marketplace.
    /// </summary>
    [JsonPropertyName("currencies")]
    public MarketplaceCurrencies? Currencies { get; init; }

    /// <summary>
    /// List of delivery countries for that marketplace.
    /// </summary>
    [JsonPropertyName("shippingCountries")]
    public List<MarketplaceShippingCountry> ShippingCountries { get; init; } = new();
}

/// <summary>
/// Represents marketplace language options.
/// </summary>
public record MarketplaceLanguages
{
    /// <summary>
    /// Languages in which you can create offer.
    /// </summary>
    [JsonPropertyName("offerCreation")]
    public List<MarketplaceLanguage> OfferCreation { get; init; } = new();

    /// <summary>
    /// Languages in which buyer can see the offer.
    /// </summary>
    [JsonPropertyName("offerDisplay")]
    public List<MarketplaceLanguage> OfferDisplay { get; init; } = new();
}

/// <summary>
/// Represents a marketplace language.
/// </summary>
public record MarketplaceLanguage
{
    /// <summary>
    /// BCP-47 language code (e.g., "pl-PL", "en-US").
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;
}

/// <summary>
/// Represents marketplace currency options.
/// </summary>
public record MarketplaceCurrencies
{
    /// <summary>
    /// Base currency for the marketplace.
    /// </summary>
    [JsonPropertyName("base")]
    public MarketplaceCurrency? Base { get; init; }

    /// <summary>
    /// List of other currencies available for that marketplace.
    /// </summary>
    [JsonPropertyName("additional")]
    public List<MarketplaceCurrency> Additional { get; init; } = new();
}

/// <summary>
/// Represents a marketplace currency.
/// </summary>
public record MarketplaceCurrency
{
    /// <summary>
    /// ISO 4217 currency code (e.g., "PLN", "EUR").
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;
}

/// <summary>
/// Represents a marketplace shipping country.
/// </summary>
public record MarketplaceShippingCountry
{
    /// <summary>
    /// ISO 3166 country code (e.g., "PL", "DE").
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;
}
