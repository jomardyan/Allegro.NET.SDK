using System.Text.Json.Serialization;

namespace AllegroApi.Models.SaleExtensions;

/// <summary>
/// List of offer bundles.
/// </summary>
public record BundlesList
{
    /// <summary>
    /// Collection of bundles.
    /// </summary>
    [JsonPropertyName("bundles")]
    public List<Bundle>? Bundles { get; init; }
}

/// <summary>
/// Offer bundle details (product bundles/sets).
/// </summary>
public record Bundle
{
    /// <summary>
    /// Bundle identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Bundle name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Offers included in the bundle.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<BundleOffer>? Offers { get; init; }

    /// <summary>
    /// Bundle discount information.
    /// </summary>
    [JsonPropertyName("discount")]
    public BundleDiscount? Discount { get; init; }
}

/// <summary>
/// Offer reference in a bundle.
/// </summary>
public record BundleOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Quantity of this offer in the bundle.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }
}

/// <summary>
/// Bundle discount information.
/// </summary>
public record BundleDiscount
{
    /// <summary>
    /// Discount percentage.
    /// </summary>
    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; init; }

    /// <summary>
    /// Fixed discount amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }
}

/// <summary>
/// List of loyalty promotions.
/// </summary>
public record LoyaltyPromotionsList
{
    /// <summary>
    /// Collection of loyalty promotions.
    /// </summary>
    [JsonPropertyName("promotions")]
    public List<LoyaltyPromotion>? Promotions { get; init; }
}

/// <summary>
/// Loyalty promotion details.
/// </summary>
public record LoyaltyPromotion
{
    /// <summary>
    /// Promotion identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Promotion name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Promotion type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Promotion benefit description.
    /// </summary>
    [JsonPropertyName("benefit")]
    public string? Benefit { get; init; }

    /// <summary>
    /// Promotion start date.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Promotion end date.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; init; }
}

/// <summary>
/// List of offer tags.
/// </summary>
public record OfferTagsList
{
    /// <summary>
    /// Collection of offer tags.
    /// </summary>
    [JsonPropertyName("tags")]
    public List<OfferTag>? Tags { get; init; }
}

/// <summary>
/// Offer tag details.
/// </summary>
public record OfferTag
{
    /// <summary>
    /// Tag identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Tag name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Number of offers with this tag.
    /// </summary>
    [JsonPropertyName("offersCount")]
    public int? OffersCount { get; init; }
}

/// <summary>
/// Request for creating an offer tag.
/// </summary>
public record CreateOfferTagRequest
{
    /// <summary>
    /// Tag name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Request for assigning a tag to an offer.
/// </summary>
public record AssignTagRequest
{
    /// <summary>
    /// Tag identifier.
    /// </summary>
    [JsonPropertyName("tagId")]
    public string? TagId { get; init; }
}

/// <summary>
/// Request body for creating or modifying a turnover discount.
/// </summary>
public record TurnoverDiscountRequest
{
    /// <summary>
    /// List of thresholds to apply to cumulated turnover.
    /// </summary>
    [JsonPropertyName("thresholds")]
    public List<TurnoverDiscountThresholdDto>? Thresholds { get; init; }
}

/// <summary>
/// A turnover discount threshold mapping minimum turnover to a discount percentage.
/// </summary>
public record TurnoverDiscountThresholdDto
{
    /// <summary>
    /// Minimum turnover the buyer must accumulate to receive a discount.
    /// </summary>
    [JsonPropertyName("minimumTurnover")]
    public TurnoverAmount? MinimumTurnover { get; init; }

    /// <summary>
    /// Discount obtained by the user after reaching the threshold.
    /// </summary>
    [JsonPropertyName("discount")]
    public TurnoverDiscountPercentage? Discount { get; init; }
}

/// <summary>
/// Amount with currency for turnover thresholds.
/// </summary>
public record TurnoverAmount
{
    /// <summary>
    /// Amount as string (fractional part must be absent or 0).
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
/// Discount percentage for a turnover threshold.
/// </summary>
public record TurnoverDiscountPercentage
{
    /// <summary>
    /// Discount percentage value (fractional part must be absent or 0).
    /// </summary>
    [JsonPropertyName("percentage")]
    public string? Percentage { get; init; }
}

/// <summary>
/// Turnover discount details for a marketplace.
/// </summary>
public record TurnoverDiscountDto
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Turnover discount status: ACTIVATING, ACTIVE, or DEACTIVATING.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Definitions currently active or active in the future.
    /// </summary>
    [JsonPropertyName("definitions")]
    public List<TurnoverDiscountDefinitionDto>? Definitions { get; init; }
}

/// <summary>
/// Turnover discount definition for a specific period.
/// </summary>
public record TurnoverDiscountDefinitionDto
{
    /// <summary>
    /// First day of cumulating turnover against this definition.
    /// </summary>
    [JsonPropertyName("cumulatingFromDate")]
    public string? CumulatingFromDate { get; init; }

    /// <summary>
    /// First day when cumulating turnover is no longer happening (null = indefinite).
    /// </summary>
    [JsonPropertyName("cumulatingToDate")]
    public string? CumulatingToDate { get; init; }

    /// <summary>
    /// First day of applying discount from this definition.
    /// </summary>
    [JsonPropertyName("spendingFromDate")]
    public string? SpendingFromDate { get; init; }

    /// <summary>
    /// First day when applying discount is no longer happening (null = indefinite).
    /// </summary>
    [JsonPropertyName("spendingToDate")]
    public string? SpendingToDate { get; init; }

    /// <summary>
    /// Creation date of the definition.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Last update date of the definition.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Turnover discount thresholds.
    /// </summary>
    [JsonPropertyName("thresholds")]
    public List<TurnoverDiscountThresholdDto>? Thresholds { get; init; }
}

/// <summary>
/// Available promotion packages for offers.
/// </summary>
public record AvailablePromotionPackages
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Available base promotion packages (only one can be set on an offer).
    /// </summary>
    [JsonPropertyName("basePackages")]
    public List<AvailablePromotionPackage>? BasePackages { get; init; }

    /// <summary>
    /// Available extra promotion packages (multiple can be set on an offer).
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<AvailablePromotionPackage>? ExtraPackages { get; init; }

    /// <summary>
    /// Available promotion packages on additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public List<MarketplaceAvailablePromotionPackages>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// Available promotion packages for a specific marketplace.
/// </summary>
public record MarketplaceAvailablePromotionPackages
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Available base promotion packages.
    /// </summary>
    [JsonPropertyName("basePackages")]
    public List<AvailablePromotionPackage>? BasePackages { get; init; }

    /// <summary>
    /// Available extra promotion packages.
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<AvailablePromotionPackage>? ExtraPackages { get; init; }
}

/// <summary>
/// A single available promotion package.
/// </summary>
public record AvailablePromotionPackage
{
    /// <summary>
    /// Promotion package identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Promotion package name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Promotion package cycle duration (ISO 8601).
    /// </summary>
    [JsonPropertyName("cycleDuration")]
    public string? CycleDuration { get; init; }
}

/// <summary>
/// Promotion options currently assigned to an offer.
/// </summary>
public record OfferPromoOptions
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Base promotion package assigned to the offer.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public OfferPromoOption? BasePackage { get; init; }

    /// <summary>
    /// Extra promotion packages assigned to the offer.
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<OfferPromoOption>? ExtraPackages { get; init; }

    /// <summary>
    /// Pending changes to promotion packages.
    /// </summary>
    [JsonPropertyName("pendingChanges")]
    public OfferPromoOptionsPendingChanges? PendingChanges { get; init; }

    /// <summary>
    /// Promotion packages on additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public List<MarketplaceOfferPromoOption>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// A single promotion option assigned to an offer with validity dates.
/// </summary>
public record OfferPromoOption
{
    /// <summary>
    /// Promotion package identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Date from which the promotion package is valid (ISO 8601).
    /// </summary>
    [JsonPropertyName("validFrom")]
    public DateTime? ValidFrom { get; init; }

    /// <summary>
    /// Date to which the promotion package is valid (ISO 8601).
    /// </summary>
    [JsonPropertyName("validTo")]
    public DateTime? ValidTo { get; init; }

    /// <summary>
    /// Date on which the promotion package will be renewed (ISO 8601).
    /// </summary>
    [JsonPropertyName("nextCycleDate")]
    public DateTime? NextCycleDate { get; init; }
}

/// <summary>
/// Pending changes to an offer's promotion packages.
/// </summary>
public record OfferPromoOptionsPendingChanges
{
    /// <summary>
    /// Pending base package change.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public OfferPromoOption? BasePackage { get; init; }
}

/// <summary>
/// Promotion packages for a specific additional marketplace.
/// </summary>
public record MarketplaceOfferPromoOption
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Base promotion package on this marketplace.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public OfferPromoOption? BasePackage { get; init; }

    /// <summary>
    /// Extra promotion packages on this marketplace.
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<OfferPromoOption>? ExtraPackages { get; init; }

    /// <summary>
    /// Pending changes on this marketplace.
    /// </summary>
    [JsonPropertyName("pendingChanges")]
    public OfferPromoOptionsPendingChanges? PendingChanges { get; init; }
}

/// <summary>
/// Promo options for multiple seller offers.
/// </summary>
public record OfferPromoOptionsForSeller
{
    /// <summary>
    /// Promo options for seller offers.
    /// </summary>
    [JsonPropertyName("promoOptions")]
    public List<OfferPromoOptions>? PromoOptions { get; init; }

    /// <summary>
    /// Number of returned elements.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Total number of available elements.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public long? TotalCount { get; init; }
}

/// <summary>
/// Request to modify promotion options on a single offer.
/// </summary>
public record PromoOptionsModifications
{
    /// <summary>
    /// Promotion package modifications to be applied.
    /// </summary>
    [JsonPropertyName("modifications")]
    public List<PromoOptionsModification>? Modifications { get; init; }

    /// <summary>
    /// Promotion package modifications on additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public List<AdditionalMarketplacePromoOptionsModification>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// A single promotion package modification.
/// </summary>
public record PromoOptionsModification
{
    /// <summary>
    /// Type of modification: CHANGE, REMOVE_WITH_END_OF_CYCLE, or REMOVE_NOW.
    /// </summary>
    [JsonPropertyName("modificationType")]
    public string? ModificationType { get; init; }

    /// <summary>
    /// Type of promotion package: BASE or EXTRA.
    /// </summary>
    [JsonPropertyName("packageType")]
    public string? PackageType { get; init; }

    /// <summary>
    /// Promotion package identifier.
    /// </summary>
    [JsonPropertyName("packageId")]
    public string? PackageId { get; init; }
}

/// <summary>
/// Promotion package modifications on a specific additional marketplace.
/// </summary>
public record AdditionalMarketplacePromoOptionsModification
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Promotion package modifications to be applied on this marketplace.
    /// </summary>
    [JsonPropertyName("modifications")]
    public List<PromoOptionsModification>? Modifications { get; init; }
}

/// <summary>
/// Command for batch modification of promotion packages on multiple offers.
/// </summary>
public record PromoOptionsCommand
{
    /// <summary>
    /// Offer choice criteria.
    /// </summary>
    [JsonPropertyName("offerCriteria")]
    public List<OfferCriterium>? OfferCriteria { get; init; }

    /// <summary>
    /// Modification to be applied.
    /// </summary>
    [JsonPropertyName("modification")]
    public PromoOptionsCommandModification? Modification { get; init; }

    /// <summary>
    /// Modifications for additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public List<AdditionalMarketplacePromoOptionsCommandModification>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// Offer criteria for batch promo options commands.
/// </summary>
public record OfferCriterium
{
    /// <summary>
    /// Type of offer selection criteria.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Offers to which the promotion package modifications will be applied.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferId>? Offers { get; init; }
}

/// <summary>
/// Represents an offer identifier used in offer criteria.
/// </summary>
public record OfferId
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Modification details for a batch promo options command.
/// </summary>
public record PromoOptionsCommandModification
{
    /// <summary>
    /// Base package to set. Available packages from GET /sale/offer-promotion-packages.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public PromoOptionsCommandModificationPackage? BasePackage { get; init; }

    /// <summary>
    /// Extra packages to set (omitting preserves existing packages).
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<PromoOptionsCommandModificationPackage>? ExtraPackages { get; init; }

    /// <summary>
    /// Time at which modification will be applied: NOW or END_OF_CYCLE.
    /// </summary>
    [JsonPropertyName("modificationTime")]
    public string? ModificationTime { get; init; }
}

/// <summary>
/// Modification for additional marketplace in batch promo options command.
/// </summary>
public record AdditionalMarketplacePromoOptionsCommandModification
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Modification to apply on this marketplace.
    /// </summary>
    [JsonPropertyName("modification")]
    public PromoOptionsCommandModification? Modification { get; init; }
}

/// <summary>
/// Package reference in a batch promo options command.
/// </summary>
public record PromoOptionsCommandModificationPackage
{
    /// <summary>
    /// Promotion package identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Summary report for a batch promo options command.
/// </summary>
public record PromoGeneralReport
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Task count breakdown.
    /// </summary>
    [JsonPropertyName("taskCount")]
    public TaskCountDto? TaskCount { get; init; }
}

/// <summary>
/// Task count summary for batch commands.
/// </summary>
public record TaskCountDto
{
    /// <summary>
    /// Total number of tasks.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// Number of successful tasks.
    /// </summary>
    [JsonPropertyName("success")]
    public int? Success { get; init; }

    /// <summary>
    /// Number of failed tasks.
    /// </summary>
    [JsonPropertyName("failed")]
    public int? Failed { get; init; }
}
