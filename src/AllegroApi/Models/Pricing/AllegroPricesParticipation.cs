using System.Text.Json.Serialization;

namespace AllegroApi.Models.Pricing;

/// <summary>
/// Account participation status in the Allegro Prices program across marketplaces.
/// </summary>
public record AllegroPricesAccountParticipationResponse
{
    /// <summary>
    /// Participation status per marketplace.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public List<AccountParticipationMarketplace>? Marketplaces { get; init; }
}

/// <summary>
/// Account participation status for a single marketplace.
/// </summary>
public record AccountParticipationMarketplace
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Participation status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>
/// Request body for updating account participation in the Allegro Prices program.
/// </summary>
public record AllegroPricesAccountParticipationRequest
{
    /// <summary>
    /// Participation status to set per marketplace.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public List<AccountParticipationMarketplace>? Marketplaces { get; init; }
}

/// <summary>
/// Request body for querying Allegro Prices offers status.
/// </summary>
public record OfferStatusQueryRequestDto
{
    /// <summary>
    /// Offer filter.
    /// </summary>
    [JsonPropertyName("offer")]
    public OfferStatusFilterDto? Offer { get; init; }

    /// <summary>
    /// Marketplace filter.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public OfferStatusMarketplaceFilterDto? Marketplace { get; init; }

    /// <summary>
    /// Maximum number of offers to return.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    /// <summary>
    /// Number of offers to skip.
    /// </summary>
    [JsonPropertyName("offset")]
    public int? Offset { get; init; }
}

/// <summary>
/// Offer filter used when querying Allegro Prices offers status.
/// </summary>
public record OfferStatusFilterDto
{
    /// <summary>
    /// Offer identifiers to filter by.
    /// </summary>
    [JsonPropertyName("ids")]
    public List<string>? Ids { get; init; }

    /// <summary>
    /// Scope filter: WITH_DECLARATION, DISCOUNTED or EXCLUDED.
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; init; }
}

/// <summary>
/// Marketplace filter used when querying Allegro Prices offers status.
/// </summary>
public record OfferStatusMarketplaceFilterDto
{
    /// <summary>
    /// Marketplace identifier (allegro-pl, allegro-cz, allegro-sk, allegro-hu).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Response when querying Allegro Prices offers status.
/// </summary>
public record OfferStatusQueryResponseDto
{
    /// <summary>
    /// Offers matching the query.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferStatusItemDto>? Offers { get; init; }

    /// <summary>
    /// Number of returned offers.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total number of matching offers.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; init; }
}

/// <summary>
/// Allegro Prices status for a single offer.
/// </summary>
public record OfferStatusItemDto
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
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public SubsidyMarketplace? Marketplace { get; init; }

    /// <summary>
    /// Base price.
    /// </summary>
    [JsonPropertyName("basePrice")]
    public SubsidyMoney? BasePrice { get; init; }

    /// <summary>
    /// Date the offer was discounted (ISO 8601).
    /// </summary>
    [JsonPropertyName("discountedAt")]
    public DateTime? DiscountedAt { get; init; }

    /// <summary>
    /// Date the offer was excluded (ISO 8601).
    /// </summary>
    [JsonPropertyName("excludedAt")]
    public DateTime? ExcludedAt { get; init; }
}

/// <summary>
/// Marketplace reference used by Allegro Prices subsidy operations.
/// </summary>
public record SubsidyMarketplace
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Monetary amount used by Allegro Prices subsidy operations.
/// </summary>
public record SubsidyMoney
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

/// <summary>
/// Seller discount declaration for an offer submitted to Allegro Prices.
/// </summary>
public record SellerDiscountDeclaration
{
    /// <summary>
    /// Maximum contribution percentage declared by the seller.
    /// </summary>
    [JsonPropertyName("maxContributionPercentage")]
    public string? MaxContributionPercentage { get; init; }
}

/// <summary>
/// Error related to a single offer in a subsidy command.
/// </summary>
public record SubsidyOfferError
{
    /// <summary>
    /// Error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}

/// <summary>
/// Result of submitting a manage-offers (submit/exclude) command.
/// </summary>
public record SubsidyManageOffersCommandResult
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Command status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Command creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Request body for excluding offers from Allegro Prices.
/// </summary>
public record SubsidyExcludeOffersCommand
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Offers to exclude.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<SubsidyOfferToExclude>? Offers { get; init; }
}

/// <summary>
/// An offer to exclude from Allegro Prices.
/// </summary>
public record SubsidyOfferToExclude
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Marketplace the exclusion applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public SubsidyMarketplace? Marketplace { get; init; }
}

/// <summary>
/// Request body for submitting offers to Allegro Prices.
/// </summary>
public record SubsidySubmitOffersCommand
{
    /// <summary>
    /// Command identifier (UUID).
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Offers to submit.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<SubsidyOfferToSubmit>? Offers { get; init; }
}

/// <summary>
/// An offer to submit to Allegro Prices.
/// </summary>
public record SubsidyOfferToSubmit
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Marketplace the submission applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public SubsidyMarketplace? Marketplace { get; init; }

    /// <summary>
    /// Seller discount declaration.
    /// </summary>
    [JsonPropertyName("sellerDiscountDeclaration")]
    public SellerDiscountDeclaration? SellerDiscountDeclaration { get; init; }
}

/// <summary>
/// Preview of an exclude-offers command status.
/// </summary>
public record SubsidyExcludeOffersCommandPreview
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Command creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Per-offer exclusion results.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<SubsidyExcludeOfferItem>? Offers { get; init; }
}

/// <summary>
/// Per-offer result within an exclude-offers command.
/// </summary>
public record SubsidyExcludeOfferItem
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Marketplace the exclusion applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public SubsidyMarketplace? Marketplace { get; init; }

    /// <summary>
    /// Date the operation completed (ISO 8601).
    /// </summary>
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; init; }

    /// <summary>
    /// Operation status: SUCCESS, IN_PROGRESS or FAILED.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Errors related to the offer.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<SubsidyOfferError>? Errors { get; init; }
}

/// <summary>
/// Preview of a submit-offers command status.
/// </summary>
public record SubsidySubmitOffersCommandPreview
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("commandId")]
    public string? CommandId { get; init; }

    /// <summary>
    /// Command creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Per-offer submission results.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<SubsidySubmitOfferItem>? Offers { get; init; }
}

/// <summary>
/// Per-offer result within a submit-offers command.
/// </summary>
public record SubsidySubmitOfferItem
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Marketplace the submission applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public SubsidyMarketplace? Marketplace { get; init; }

    /// <summary>
    /// Seller discount declaration.
    /// </summary>
    [JsonPropertyName("sellerDiscountDeclaration")]
    public SellerDiscountDeclaration? SellerDiscountDeclaration { get; init; }

    /// <summary>
    /// Date the operation completed (ISO 8601).
    /// </summary>
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; init; }

    /// <summary>
    /// Operation status: SUCCESS, IN_PROGRESS or FAILED.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Errors related to the offer.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<SubsidyOfferError>? Errors { get; init; }
}
