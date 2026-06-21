using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllegroApi.Models.SaleExtensions;

/// <summary>
/// Paged listing of a seller's flexible bundles.
/// </summary>
public record FlexibleBundlesListingDTO
{
    /// <summary>
    /// Flexible bundles on the current page.
    /// </summary>
    [JsonPropertyName("bundles")]
    public List<FlexibleBundleListingDTO>? Bundles { get; init; }

    /// <summary>
    /// Reference to the next page, when present.
    /// </summary>
    [JsonPropertyName("nextPage")]
    public FlexibleBundleNextPage? NextPage { get; init; }
}

/// <summary>
/// Reference to the next page of flexible bundles.
/// </summary>
public record FlexibleBundleNextPage
{
    /// <summary>
    /// Identifier of the next page (pass as page.id).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Flexible bundle summary as returned by the listing endpoint.
/// </summary>
public record FlexibleBundleListingDTO
{
    /// <summary>
    /// Bundle identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Who created the bundle: USER or ALLEGRO.
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }

    /// <summary>
    /// Bundle creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Representative offer identifiers for each slot.
    /// </summary>
    [JsonPropertyName("slotsRepresentatives")]
    public List<string>? SlotsRepresentatives { get; init; }

    /// <summary>
    /// Discount configuration.
    /// </summary>
    [JsonPropertyName("discount")]
    public FlexibleBundleDiscountDTO? Discount { get; init; }
}

/// <summary>
/// Request body for creating a flexible bundle.
/// </summary>
public record FlexibleBundleCreateDTO
{
    /// <summary>
    /// Bundle slots.
    /// </summary>
    [JsonPropertyName("slots")]
    public List<FlexibleBundleSlotDTO>? Slots { get; init; }

    /// <summary>
    /// Discount configuration.
    /// </summary>
    [JsonPropertyName("discount")]
    public FlexibleBundleDiscountDTO? Discount { get; init; }
}

/// <summary>
/// Request body for updating a flexible bundle.
/// </summary>
public record FlexibleBundleUpdateDTO
{
    /// <summary>
    /// Bundle slots.
    /// </summary>
    [JsonPropertyName("slots")]
    public List<FlexibleBundleSlotDTO>? Slots { get; init; }

    /// <summary>
    /// Discount configuration.
    /// </summary>
    [JsonPropertyName("discount")]
    public FlexibleBundleDiscountDTO? Discount { get; init; }
}

/// <summary>
/// A slot within a flexible bundle definition.
/// </summary>
public record FlexibleBundleSlotDTO
{
    /// <summary>
    /// Slot identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Slot order within the bundle.
    /// </summary>
    [JsonPropertyName("order")]
    public int? Order { get; init; }

    /// <summary>
    /// Whether this slot is the bundle entry point.
    /// </summary>
    [JsonPropertyName("entryPoint")]
    public bool? EntryPoint { get; init; }

    /// <summary>
    /// Required quantity of offers from this slot.
    /// </summary>
    [JsonPropertyName("requiredQuantity")]
    public int? RequiredQuantity { get; init; }

    /// <summary>
    /// Offers assigned to this slot.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<FlexibleBundleOfferDTO>? Offers { get; init; }
}

/// <summary>
/// An offer assigned to a flexible bundle slot (create/update).
/// </summary>
public record FlexibleBundleOfferDTO
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Whether the offer is excluded from the bundle discount.
    /// </summary>
    [JsonPropertyName("excludedFromDiscount")]
    public bool? ExcludedFromDiscount { get; init; }
}

/// <summary>
/// Full flexible bundle representation as returned by GET/POST/PUT.
/// </summary>
public record FlexibleBundleGetDTO
{
    /// <summary>
    /// Bundle identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Who created the bundle: USER or ALLEGRO.
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }

    /// <summary>
    /// Bundle creation date (ISO 8601).
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Bundle slots with resolved offer details.
    /// </summary>
    [JsonPropertyName("slots")]
    public List<FlexibleBundleGetSlotDTO>? Slots { get; init; }

    /// <summary>
    /// Discount configuration.
    /// </summary>
    [JsonPropertyName("discount")]
    public FlexibleBundleDiscountDTO? Discount { get; init; }
}

/// <summary>
/// A slot within a flexible bundle, including resolved offers.
/// </summary>
public record FlexibleBundleGetSlotDTO
{
    /// <summary>
    /// Slot identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Slot order within the bundle.
    /// </summary>
    [JsonPropertyName("order")]
    public int? Order { get; init; }

    /// <summary>
    /// Whether this slot is the bundle entry point.
    /// </summary>
    [JsonPropertyName("entryPoint")]
    public bool? EntryPoint { get; init; }

    /// <summary>
    /// Required quantity of offers from this slot.
    /// </summary>
    [JsonPropertyName("requiredQuantity")]
    public int? RequiredQuantity { get; init; }

    /// <summary>
    /// Offers assigned to this slot.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<FlexibleBundleGetOfferDTO>? Offers { get; init; }
}

/// <summary>
/// An offer within a flexible bundle slot, including marketplace availability.
/// </summary>
public record FlexibleBundleGetOfferDTO
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Whether the offer is excluded from the bundle discount.
    /// </summary>
    [JsonPropertyName("excludedFromDiscount")]
    public bool? ExcludedFromDiscount { get; init; }

    /// <summary>
    /// Whether the offer is an entry point.
    /// </summary>
    [JsonPropertyName("entryPoint")]
    public bool? EntryPoint { get; init; }

    /// <summary>
    /// Per-marketplace details for the offer.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public List<FlexibleBundleOfferMarketplaceDetailsDTO>? Marketplaces { get; init; }
}

/// <summary>
/// Marketplace-specific details for a flexible bundle offer.
/// </summary>
public record FlexibleBundleOfferMarketplaceDetailsDTO
{
    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Availability of the offer on this marketplace.
    /// </summary>
    [JsonPropertyName("availability")]
    public FlexibleBundleOfferAvailabilityDTO? Availability { get; init; }
}

/// <summary>
/// Availability of a flexible bundle offer on a marketplace.
/// </summary>
public record FlexibleBundleOfferAvailabilityDTO
{
    /// <summary>
    /// Whether the offer is available.
    /// </summary>
    [JsonPropertyName("available")]
    public bool? Available { get; init; }

    /// <summary>
    /// Reasons for unavailability, when applicable.
    /// </summary>
    [JsonPropertyName("reasons")]
    public List<string>? Reasons { get; init; }
}

/// <summary>
/// Discount configuration for a flexible bundle.
/// </summary>
public record FlexibleBundleDiscountDTO
{
    /// <summary>
    /// Discount type: WHOLE_BUNDLE_DISCOUNT or SLOT_DISCOUNT.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Whole-bundle discount configuration (required when type is WHOLE_BUNDLE_DISCOUNT).
    /// </summary>
    [JsonPropertyName("bundle")]
    public JsonElement? Bundle { get; init; }

    /// <summary>
    /// Per-slot discount configuration (required when type is SLOT_DISCOUNT).
    /// </summary>
    [JsonPropertyName("slot")]
    public JsonElement? Slot { get; init; }
}
