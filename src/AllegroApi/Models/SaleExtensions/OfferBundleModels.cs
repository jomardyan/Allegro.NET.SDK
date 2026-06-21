using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllegroApi.Models.SaleExtensions;

/// <summary>
/// Offer bundle details as returned by the bundle endpoints.
/// </summary>
public record OfferBundleDTO
{
    /// <summary>
    /// Bundle identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Offers included in the bundle.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<BundledOfferDTO>? Offers { get; init; }

    /// <summary>
    /// Bundle publication information.
    /// </summary>
    [JsonPropertyName("publication")]
    public BundlePublicationDTO? Publication { get; init; }

    /// <summary>
    /// Bundle discounts per marketplace.
    /// </summary>
    [JsonPropertyName("discounts")]
    public List<BundleDiscountDTO>? Discounts { get; init; }

    /// <summary>
    /// Bundle creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Who created the bundle: USER or ALLEGRO.
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }
}

/// <summary>
/// An offer participating in a bundle.
/// </summary>
public record BundledOfferDTO
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Required quantity of this offer in the bundle.
    /// </summary>
    [JsonPropertyName("requiredQuantity")]
    public int? RequiredQuantity { get; init; }

    /// <summary>
    /// Whether this offer is the entry point of the bundle.
    /// </summary>
    [JsonPropertyName("entryPoint")]
    public bool? EntryPoint { get; init; }
}

/// <summary>
/// Bundle publication state for a marketplace.
/// </summary>
public record BundlePublicationDTO
{
    /// <summary>
    /// Marketplace the bundle is published on.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public BundleMarketplaceDTO? Marketplace { get; init; }

    /// <summary>
    /// Publication status: ACTIVE or SUSPENDED.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>
/// Marketplace reference within a bundle.
/// </summary>
public record BundleMarketplaceDTO
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Discount associated with a bundle for a marketplace.
/// </summary>
public record BundleDiscountDTO
{
    /// <summary>
    /// Marketplace the discount applies to.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public BundleMarketplaceDTO? Marketplace { get; init; }

    /// <summary>
    /// Discount amount.
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
/// Request body for updating a bundle discount.
/// </summary>
public record UpdateOfferBundleDiscountDTO
{
    /// <summary>
    /// Discounts to apply per marketplace.
    /// </summary>
    [JsonPropertyName("discounts")]
    public List<BundleDiscountDTO>? Discounts { get; init; }
}

/// <summary>
/// Loyalty (seller) promotion details.
/// </summary>
public record SellerRebateDto
{
    /// <summary>
    /// Promotion identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Benefits granted by the promotion.
    /// </summary>
    [JsonPropertyName("benefits")]
    public List<SellerRebateBenefit>? Benefits { get; init; }

    /// <summary>
    /// Criteria selecting offers the promotion applies to.
    /// </summary>
    [JsonPropertyName("offerCriteria")]
    public List<SellerRebateOfferCriterion>? OfferCriteria { get; init; }

    /// <summary>
    /// Promotion status: ACTIVE, INACTIVE or SUSPENDED.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Promotion creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Request body for creating or modifying a loyalty (seller) promotion.
/// </summary>
public record SellerCreateRebateRequestDto
{
    /// <summary>
    /// Benefits granted by the promotion.
    /// </summary>
    [JsonPropertyName("benefits")]
    public List<SellerRebateBenefit>? Benefits { get; init; }

    /// <summary>
    /// Criteria selecting offers the promotion applies to.
    /// </summary>
    [JsonPropertyName("offerCriteria")]
    public List<SellerRebateOfferCriterion>? OfferCriteria { get; init; }
}

/// <summary>
/// A benefit granted by a loyalty promotion.
/// </summary>
public record SellerRebateBenefit
{
    /// <summary>
    /// Benefit specification. The shape depends on the benefit type.
    /// </summary>
    [JsonPropertyName("specification")]
    public JsonElement? Specification { get; init; }
}

/// <summary>
/// Offer selection criterion for a loyalty promotion.
/// </summary>
public record SellerRebateOfferCriterion
{
    /// <summary>
    /// Criterion type: CONTAINS_OFFERS, OFFERS_ASSIGNED_EXTERNALLY or ALL_OFFERS.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Offers the criterion applies to (for CONTAINS_OFFERS).
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferId>? Offers { get; init; }
}

/// <summary>
/// Request body for creating or modifying an offer tag.
/// </summary>
public record TagRequest
{
    /// <summary>
    /// Tag name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Whether the tag is hidden.
    /// </summary>
    [JsonPropertyName("hidden")]
    public bool? Hidden { get; init; }
}

/// <summary>
/// Detailed result of a batch promo options modification command.
/// </summary>
public record PromoModificationReport
{
    /// <summary>
    /// Individual modification tasks.
    /// </summary>
    [JsonPropertyName("tasks")]
    public List<PromoModificationTask>? Tasks { get; init; }

    /// <summary>
    /// Modification applied by the command.
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
/// A single task within a promo options modification command.
/// </summary>
public record PromoModificationTask
{
    /// <summary>
    /// Offer the task applies to.
    /// </summary>
    [JsonPropertyName("offer")]
    public OfferId? Offer { get; init; }

    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Date the task was scheduled (ISO 8601).
    /// </summary>
    [JsonPropertyName("scheduledAt")]
    public DateTime? ScheduledAt { get; init; }

    /// <summary>
    /// Date the task finished (ISO 8601).
    /// </summary>
    [JsonPropertyName("finishedAt")]
    public DateTime? FinishedAt { get; init; }

    /// <summary>
    /// Task status: DONE, ERROR or IN_PROGRESS.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
