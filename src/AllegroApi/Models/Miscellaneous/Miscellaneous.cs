using System.Text.Json.Serialization;

namespace AllegroApi.Models.Miscellaneous;

/// <summary>
/// List of charity campaigns.
/// </summary>
public record CharityCampaignsList
{
    /// <summary>
    /// Collection of charity campaigns.
    /// </summary>
    [JsonPropertyName("campaigns")]
    public List<CharityCampaign>? Campaigns { get; init; }
}

/// <summary>
/// Charity campaign details.
/// </summary>
public record CharityCampaign
{
    /// <summary>
    /// Campaign identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Campaign name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Campaign description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Charity organization name.
    /// </summary>
    [JsonPropertyName("organization")]
    public string? Organization { get; init; }

    /// <summary>
    /// Campaign start date.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Campaign end date.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; init; }
}

/// <summary>
/// List of bidding offers (auction-style offers).
/// </summary>
public record BiddingOffersList
{
    /// <summary>
    /// Collection of bidding offers.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<BiddingOffer>? Offers { get; init; }

    /// <summary>
    /// Total count of offers.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Bidding offer details (auction-style offer).
/// </summary>
public record BiddingOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Offer name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Current highest bid amount.
    /// </summary>
    [JsonPropertyName("currentBid")]
    public decimal? CurrentBid { get; init; }

    /// <summary>
    /// Starting bid amount.
    /// </summary>
    [JsonPropertyName("startingBid")]
    public decimal? StartingBid { get; init; }

    /// <summary>
    /// Number of bids placed.
    /// </summary>
    [JsonPropertyName("bidCount")]
    public int? BidCount { get; init; }

    /// <summary>
    /// Auction end time.
    /// </summary>
    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; init; }
}

/// <summary>
/// Affiliate program information.
/// </summary>
public record AffiliateProgramInfo
{
    /// <summary>
    /// Affiliate program identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Program name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Commission rate percentage.
    /// </summary>
    [JsonPropertyName("commissionRate")]
    public decimal? CommissionRate { get; init; }

    /// <summary>
    /// Indicates if the program is active.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; init; }

    /// <summary>
    /// Tracking link for affiliates.
    /// </summary>
    [JsonPropertyName("trackingLink")]
    public string? TrackingLink { get; init; }
}

/// <summary>
/// Seller deposit information.
/// </summary>
public record DepositInfo
{
    /// <summary>
    /// Current deposit balance amount.
    /// </summary>
    [JsonPropertyName("balance")]
    public decimal? Balance { get; init; }

    /// <summary>
    /// Currency code (e.g., PLN, EUR).
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>
    /// Minimum required deposit amount.
    /// </summary>
    [JsonPropertyName("minDeposit")]
    public decimal? MinDeposit { get; init; }

    /// <summary>
    /// Reserved amount from deposit.
    /// </summary>
    [JsonPropertyName("reserved")]
    public decimal? Reserved { get; init; }

    /// <summary>
    /// Available amount for use.
    /// </summary>
    [JsonPropertyName("available")]
    public decimal? Available { get; init; }
}

/// <summary>
/// Product compatibility information.
/// </summary>
public record ProductCompatibility
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; init; }

    /// <summary>
    /// List of compatible products.
    /// </summary>
    [JsonPropertyName("compatibleWith")]
    public List<string>? CompatibleWith { get; init; }

    /// <summary>
    /// List of incompatible products.
    /// </summary>
    [JsonPropertyName("incompatibleWith")]
    public List<string>? IncompatibleWith { get; init; }
}

/// <summary>
/// Response containing seller offer events.
/// </summary>
public record SellerOfferEventsResponse
{
    /// <summary>
    /// List of offer events.
    /// </summary>
    [JsonPropertyName("offerEvents")]
    public List<SellerOfferEvent>? OfferEvents { get; init; }
}

/// <summary>
/// Seller offer event details.
/// </summary>
public record SellerOfferEvent
{
    /// <summary>
    /// Event identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Event type (e.g., OFFER_ACTIVATED, OFFER_CHANGED).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Date and time when the event occurred.
    /// </summary>
    [JsonPropertyName("occurredAt")]
    public DateTime? OccurredAt { get; init; }

    /// <summary>
    /// Offer identifier related to the event.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }
}

/// <summary>
/// Response containing deposit types.
/// </summary>
public record DepositTypeResponse
{
    /// <summary>
    /// List of available deposit types.
    /// </summary>
    [JsonPropertyName("deposits")]
    public List<DepositType>? Deposits { get; init; }
}

/// <summary>
/// Deposit type details.
/// </summary>
public record DepositType
{
    /// <summary>
    /// Deposit type identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Deposit description/name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Marketplace identifier where deposit is applicable.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Deposit price information.
    /// </summary>
    [JsonPropertyName("price")]
    public DepositPrice? Price { get; init; }
}

/// <summary>
/// Deposit price information.
/// </summary>
public record DepositPrice
{
    /// <summary>
    /// Price amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    /// <summary>
    /// Currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Response containing CPS conversions.
/// </summary>
public record CpsConversionResponse
{
    /// <summary>
    /// List of CPS conversions.
    /// </summary>
    [JsonPropertyName("conversions")]
    public List<CpsConversion>? Conversions { get; init; }
}

/// <summary>
/// CPS (Cost Per Sale) conversion details.
/// </summary>
public record CpsConversion
{
    /// <summary>
    /// Conversion identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Conversion status (CREATED, REJECTED, CONFIRMED).
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Date and time when conversion was last modified.
    /// </summary>
    [JsonPropertyName("lastModifiedAt")]
    public DateTime? LastModifiedAt { get; init; }

    /// <summary>
    /// Date and time when the related order was created.
    /// </summary>
    [JsonPropertyName("orderCreatedAt")]
    public DateTime? OrderCreatedAt { get; init; }

    /// <summary>
    /// Number of ordered items.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }
}

/// <summary>
/// Response containing categories that support compatibility lists.
/// </summary>
public record CompatibilityListSupportedCategoriesDto
{
    /// <summary>
    /// List of supported categories.
    /// </summary>
    [JsonPropertyName("supportedCategories")]
    public List<CompatibilitySupportedCategory>? SupportedCategories { get; init; }
}

/// <summary>
/// Category that supports compatibility lists.
/// </summary>
public record CompatibilitySupportedCategory
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public string? CategoryId { get; init; }

    /// <summary>
    /// Category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Type of compatible items (e.g., CAR).
    /// </summary>
    [JsonPropertyName("itemsType")]
    public string? ItemsType { get; init; }

    /// <summary>
    /// Input type (TEXT or ID).
    /// </summary>
    [JsonPropertyName("inputType")]
    public string? InputType { get; init; }
}

/// <summary>
/// Compatibility list with items.
/// </summary>
public record CompatibilityList
{
    /// <summary>
    /// Type of compatibility list (MANUAL or PRODUCT_BASED).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// List of compatible items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<string>? Items { get; init; }
}

/// <summary>
/// Response containing compatible product groups.
/// </summary>
public record CompatibleProductsGroupsDto
{
    /// <summary>
    /// List of compatible product groups.
    /// </summary>
    [JsonPropertyName("groups")]
    public List<CompatibleProductGroup>? Groups { get; init; }

    /// <summary>
    /// Number of returned elements.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total number of available elements.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; init; }
}

/// <summary>
/// Compatible product group.
/// </summary>
public record CompatibleProductGroup
{
    /// <summary>
    /// Group identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Group name.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }
}

/// <summary>
/// Response containing compatible products list.
/// </summary>
public record CompatibleProductsListDto
{
    /// <summary>
    /// List of compatible products.
    /// </summary>
    [JsonPropertyName("compatibleProducts")]
    public List<CompatibleProductDto>? CompatibleProducts { get; init; }

    /// <summary>
    /// Number of returned elements.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total number of available elements.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; init; }
}

/// <summary>
/// Compatible product details.
/// </summary>
public record CompatibleProductDto
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Product text/description.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Product group information.
    /// </summary>
    [JsonPropertyName("group")]
    public CompatibleProductGroup? Group { get; init; }
}

/// <summary>
/// Bid placement request.
/// </summary>
public record PlaceBidRequest
{
    /// <summary>
    /// Bid amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }
}

/// <summary>
/// Bid placement response.
/// </summary>
public record PlaceBidResponse
{
    /// <summary>
    /// Bid identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Indicates if bid was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    /// <summary>
    /// Message about bid result.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}
