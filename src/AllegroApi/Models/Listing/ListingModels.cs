using System.Text.Json.Serialization;

namespace AllegroApi.Models.Listing;

/// <summary>
/// Represents a listing search response.
/// </summary>
public record ListingResponse
{
    /// <summary>
    /// Gets the search result offers.
    /// </summary>
    [JsonPropertyName("items")]
    public ListingResponseOffers? Items { get; init; }

    /// <summary>
    /// Gets the category information.
    /// </summary>
    [JsonPropertyName("categories")]
    public ListingResponseCategories? Categories { get; init; }

    /// <summary>
    /// Gets the available filters for refining search results.
    /// </summary>
    [JsonPropertyName("filters")]
    public List<ListingResponseFilter>? Filters { get; init; }

    /// <summary>
    /// Gets the search metadata.
    /// </summary>
    [JsonPropertyName("searchMeta")]
    public ListingResponseSearchMeta? SearchMeta { get; init; }

    /// <summary>
    /// Gets the available sorting options.
    /// </summary>
    [JsonPropertyName("sort")]
    public List<ListingResponseSort>? Sort { get; init; }
}

/// <summary>
/// Represents the offers in the listing response.
/// </summary>
public record ListingResponseOffers
{
    /// <summary>
    /// Gets the list of promoted offers.
    /// </summary>
    [JsonPropertyName("promoted")]
    public List<ListingOffer>? Promoted { get; init; }

    /// <summary>
    /// Gets the list of regular (non-promoted) offers.
    /// </summary>
    [JsonPropertyName("regular")]
    public List<ListingOffer>? Regular { get; init; }
}

/// <summary>
/// Represents a single listing offer.
/// </summary>
public record ListingOffer
{
    /// <summary>
    /// Gets the offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the offer name/title.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the seller information.
    /// </summary>
    [JsonPropertyName("seller")]
    public OfferSeller? Seller { get; init; }

    /// <summary>
    /// Gets the selling mode and price information.
    /// </summary>
    [JsonPropertyName("sellingMode")]
    public OfferSellingMode? SellingMode { get; init; }

    /// <summary>
    /// Gets the delivery information.
    /// </summary>
    [JsonPropertyName("delivery")]
    public OfferDelivery? Delivery { get; init; }

    /// <summary>
    /// Gets the offer images.
    /// </summary>
    [JsonPropertyName("images")]
    public List<OfferImage>? Images { get; init; }

    /// <summary>
    /// Gets the stock information.
    /// </summary>
    [JsonPropertyName("stock")]
    public OfferStock? Stock { get; init; }

    /// <summary>
    /// Gets the promotion information.
    /// </summary>
    [JsonPropertyName("promotion")]
    public OfferPromotion? Promotion { get; init; }

    /// <summary>
    /// Gets the vendor information (for external services).
    /// </summary>
    [JsonPropertyName("vendor")]
    public OfferVendor? Vendor { get; init; }

    /// <summary>
    /// Gets the publication information.
    /// </summary>
    [JsonPropertyName("publication")]
    public OfferPublication? Publication { get; init; }

    /// <summary>
    /// Gets the category information.
    /// </summary>
    [JsonPropertyName("category")]
    public ListingCategory? Category { get; init; }
}

/// <summary>
/// Represents seller information in a listing offer.
/// </summary>
public record OfferSeller
{
    /// <summary>
    /// Gets the seller ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the seller login.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// Gets whether the seller represents a registered business.
    /// </summary>
    [JsonPropertyName("company")]
    public bool? Company { get; init; }

    /// <summary>
    /// Gets whether the seller has "Super Sprzedawca" status.
    /// </summary>
    [JsonPropertyName("superSeller")]
    public bool? SuperSeller { get; init; }
}

/// <summary>
/// Represents selling mode and price information.
/// </summary>
public record OfferSellingMode
{
    /// <summary>
    /// Gets the selling format (BUY_NOW, AUCTION, etc.).
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; init; }

    /// <summary>
    /// Gets the offer price.
    /// </summary>
    [JsonPropertyName("price")]
    public OfferPrice? Price { get; init; }

    /// <summary>
    /// Gets the fixed (buy now) price for auction format.
    /// </summary>
    [JsonPropertyName("fixedPrice")]
    public OfferPrice? FixedPrice { get; init; }

    /// <summary>
    /// Gets the popularity (lower bound of range).
    /// </summary>
    [JsonPropertyName("popularity")]
    public int? Popularity { get; init; }

    /// <summary>
    /// Gets the popularity range.
    /// </summary>
    [JsonPropertyName("popularityRange")]
    public string? PopularityRange { get; init; }

    /// <summary>
    /// Gets the number of bidders (for auction format).
    /// </summary>
    [JsonPropertyName("bidCount")]
    public int? BidCount { get; init; }
}

/// <summary>
/// Represents a price with amount and currency.
/// </summary>
public record OfferPrice
{
    /// <summary>
    /// Gets the amount as a string to avoid rounding errors.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Gets the currency code (ISO 4217).
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Represents delivery information for an offer.
/// </summary>
public record OfferDelivery
{
    /// <summary>
    /// Gets whether the offer has free shipping option.
    /// </summary>
    [JsonPropertyName("availableForFree")]
    public bool? AvailableForFree { get; init; }

    /// <summary>
    /// Gets the lowest shipping cost available.
    /// </summary>
    [JsonPropertyName("lowestPrice")]
    public OfferPrice? LowestPrice { get; init; }
}

/// <summary>
/// Represents an offer image.
/// </summary>
public record OfferImage
{
    /// <summary>
    /// Gets the URL of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Represents stock information.
/// </summary>
public record OfferStock
{
    /// <summary>
    /// Gets the unit type (UNIT, PAIR, SET).
    /// </summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }

    /// <summary>
    /// Gets the available stock value.
    /// </summary>
    [JsonPropertyName("available")]
    public int? Available { get; init; }
}

/// <summary>
/// Represents promotion options for an offer.
/// </summary>
public record OfferPromotion
{
    /// <summary>
    /// Gets whether the offer is promoted.
    /// </summary>
    [JsonPropertyName("emphasized")]
    public bool? Emphasized { get; init; }

    /// <summary>
    /// Gets whether the offer has bold title option.
    /// </summary>
    [JsonPropertyName("bold")]
    public bool? Bold { get; init; }

    /// <summary>
    /// Gets whether the offer has highlight option.
    /// </summary>
    [JsonPropertyName("highlight")]
    public bool? Highlight { get; init; }
}

/// <summary>
/// Represents vendor information for external services.
/// </summary>
public record OfferVendor
{
    /// <summary>
    /// Gets the identifier of the external service.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the URL to the web page of the offer.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Represents publication information.
/// </summary>
public record OfferPublication
{
    /// <summary>
    /// Gets the publication ending date and time in UTC (ISO 8601).
    /// </summary>
    [JsonPropertyName("endingAt")]
    public DateTime? EndingAt { get; init; }
}

/// <summary>
/// Represents category information in the listing response.
/// </summary>
public record ListingResponseCategories
{
    /// <summary>
    /// Gets the subcategories with counters for refining search.
    /// </summary>
    [JsonPropertyName("subcategories")]
    public List<ListingCategoryWithCount>? Subcategories { get; init; }

    /// <summary>
    /// Gets the category path (breadcrumbs).
    /// </summary>
    [JsonPropertyName("path")]
    public List<ListingCategory>? Path { get; init; }
}

/// <summary>
/// Represents a category with offer count.
/// </summary>
public record ListingCategoryWithCount
{
    /// <summary>
    /// Gets the category ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the number of offers in this category.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Represents a category.
/// </summary>
public record ListingCategory
{
    /// <summary>
    /// Gets the category ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Represents a search filter.
/// </summary>
public record ListingResponseFilter
{
    /// <summary>
    /// Gets the filter identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the filter type (MULTI, SINGLE, NUMERIC, TEXT).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the filter name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the filter values.
    /// </summary>
    [JsonPropertyName("values")]
    public List<ListingResponseFilterValue>? Values { get; init; }
}

/// <summary>
/// Represents a filter value option.
/// </summary>
public record ListingResponseFilterValue
{
    /// <summary>
    /// Gets the value name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the filter value to use in query.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>
    /// Gets the ID suffix (for numeric filters).
    /// </summary>
    [JsonPropertyName("idSuffix")]
    public string? IdSuffix { get; init; }

    /// <summary>
    /// Gets the number of matching results.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Gets whether this value was selected in the current request.
    /// </summary>
    [JsonPropertyName("selected")]
    public bool? Selected { get; init; }
}

/// <summary>
/// Represents search metadata.
/// </summary>
public record ListingResponseSearchMeta
{
    /// <summary>
    /// Gets the total count of offers.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; init; }

    /// <summary>
    /// Gets the available filters count.
    /// </summary>
    [JsonPropertyName("availableCount")]
    public int? AvailableCount { get; init; }
}

/// <summary>
/// Represents a sorting option.
/// </summary>
public record ListingResponseSort
{
    /// <summary>
    /// Gets the sort type/key.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the sort name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets whether this sort was selected.
    /// </summary>
    [JsonPropertyName("selected")]
    public bool? Selected { get; init; }
}

/// <summary>
/// Represents search parameters for listing offers.
/// </summary>
public record ListingSearchParams
{
    /// <summary>
    /// Gets or sets the category ID to search in.
    /// </summary>
    public string? CategoryId { get; init; }

    /// <summary>
    /// Gets or sets the search phrase.
    /// </summary>
    public string? Phrase { get; init; }

    /// <summary>
    /// Gets or sets the seller ID(s) to filter by.
    /// </summary>
    public List<string>? SellerIds { get; init; }

    /// <summary>
    /// Gets or sets the seller login(s) to filter by.
    /// </summary>
    public List<string>? SellerLogins { get; init; }

    /// <summary>
    /// Gets or sets the marketplace ID (default: allegro-pl).
    /// </summary>
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Gets or sets the shipping country (ISO 3166-1 alpha-2).
    /// </summary>
    public string? ShippingCountry { get; init; }

    /// <summary>
    /// Gets or sets the currency (ISO 4217).
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Gets or sets the search mode (REGULAR, CLOSED).
    /// </summary>
    public string? SearchMode { get; init; }

    /// <summary>
    /// Gets or sets the offset (0-599).
    /// </summary>
    public int? Offset { get; init; }

    /// <summary>
    /// Gets or sets the limit (1-60, default: 60).
    /// </summary>
    public int? Limit { get; init; }

    /// <summary>
    /// Gets or sets the sort order (relevance, +price, -price, etc.).
    /// </summary>
    public string? Sort { get; init; }

    /// <summary>
    /// Gets or sets the included response parts.
    /// </summary>
    public string? Include { get; init; }

    /// <summary>
    /// Gets or sets whether to use fallback for no exact results (default: true).
    /// </summary>
    public bool? Fallback { get; init; }

    /// <summary>
    /// Gets or sets additional dynamic filters (parameter.XXX=value).
    /// </summary>
    public Dictionary<string, string>? DynamicFilters { get; init; }
}
