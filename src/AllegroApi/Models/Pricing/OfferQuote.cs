using System.Text.Json.Serialization;

namespace AllegroApi.Models.Pricing;

/// <summary>
/// Response containing offer quotes (listing and promo fees).
/// </summary>
public record OfferQuotesDto
{
    /// <summary>
    /// List of offer quotes.
    /// </summary>
    [JsonPropertyName("offerQuotes")]
    public List<OfferQuote>? OfferQuotes { get; init; }
}

/// <summary>
/// Offer quote with listing and promo fees information.
/// </summary>
public record OfferQuote
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Listing fee details.
    /// </summary>
    [JsonPropertyName("listingFee")]
    public FeeQuote? ListingFee { get; init; }

    /// <summary>
    /// Promotional fee details.
    /// </summary>
    [JsonPropertyName("promoFee")]
    public FeeQuote? PromoFee { get; init; }
}

/// <summary>
/// Fee quote details with cycle and amount information.
/// </summary>
public record FeeQuote
{
    /// <summary>
    /// Fee cycle information.
    /// </summary>
    [JsonPropertyName("cycle")]
    public FeeCycle? Cycle { get; init; }

    /// <summary>
    /// Fee amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public FeeAmount? Amount { get; init; }
}

/// <summary>
/// Fee billing cycle information.
/// </summary>
public record FeeCycle
{
    /// <summary>
    /// Cycle start date and time.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Cycle end date and time.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// Fee amount with currency.
/// </summary>
public record FeeAmount
{
    /// <summary>
    /// Amount value.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
