using System.Text.Json.Serialization;

namespace AllegroApi.Models.Pricing;

/// <summary>
/// Represents Allegro Prices consent response for an offer across marketplaces.
/// </summary>
public record AllegroPricesOfferConsentResponse
{
    /// <summary>
    /// Consent status for the base marketplace (ALLOWED, DENIED, or NOT_AVAILABLE).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Consent statuses for additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public Dictionary<string, MarketplaceConsent>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// Represents consent status for a specific marketplace.
/// </summary>
public record MarketplaceConsent
{
    /// <summary>
    /// Consent status (ALLOWED, DENIED, or NOT_AVAILABLE).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
}

/// <summary>
/// Request to update Allegro Prices consent for an offer.
/// </summary>
public record AllegroPricesOfferConsentRequest
{
    /// <summary>
    /// Consent status for the base marketplace (ALLOWED or DENIED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Consent statuses for additional marketplaces.
    /// </summary>
    [JsonPropertyName("additionalMarketplaces")]
    public Dictionary<string, MarketplaceConsent>? AdditionalMarketplaces { get; init; }
}

/// <summary>
/// Represents Allegro Prices account eligibility information.
/// </summary>
public record AllegroPricesAccountEligibility
{
    /// <summary>
    /// Whether the account is eligible for Allegro Prices program.
    /// </summary>
    [JsonPropertyName("eligible")]
    public bool Eligible { get; init; }

    /// <summary>
    /// Reason code if not eligible.
    /// </summary>
    [JsonPropertyName("reasonCode")]
    public string? ReasonCode { get; init; }

    /// <summary>
    /// Human-readable reason message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}

/// <summary>
/// Represents Allegro Prices account consent status.
/// </summary>
public record AllegroPricesAccountConsent
{
    /// <summary>
    /// Global consent status for the account (ALLOWED, DENIED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Consent statuses per marketplace.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public Dictionary<string, MarketplaceConsent>? Marketplaces { get; init; }
}

/// <summary>
/// Request to update Allegro Prices account consent.
/// </summary>
public record AllegroPricesAccountConsentRequest
{
    /// <summary>
    /// Global consent status (ALLOWED or DENIED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Consent statuses per marketplace.
    /// </summary>
    [JsonPropertyName("marketplaces")]
    public Dictionary<string, MarketplaceConsent>? Marketplaces { get; init; }
}

/// <summary>
/// Represents an Alle Discount campaign.
/// </summary>
public record AlleDiscountCampaign
{
    /// <summary>
    /// Campaign identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Campaign name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Campaign description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Campaign start date (ISO 8601).
    /// </summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; init; }

    /// <summary>
    /// Campaign end date (ISO 8601).
    /// </summary>
    [JsonPropertyName("endDate")]
    public string? EndDate { get; init; }

    /// <summary>
    /// Campaign status (ACTIVE, INACTIVE, ENDED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Marketplace identifier.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }
}

/// <summary>
/// List of Alle Discount campaigns.
/// </summary>
public record AlleDiscountCampaigns
{
    /// <summary>
    /// List of campaigns.
    /// </summary>
    [JsonPropertyName("campaigns")]
    public List<AlleDiscountCampaign> Campaigns { get; init; } = new();
}

/// <summary>
/// Request to submit offers to Alle Discount campaign.
/// </summary>
public record AlleDiscountSubmitOffersRequest
{
    /// <summary>
    /// Campaign identifier.
    /// </summary>
    [JsonPropertyName("campaignId")]
    public string CampaignId { get; init; } = string.Empty;

    /// <summary>
    /// List of offer identifiers to submit.
    /// </summary>
    [JsonPropertyName("offerIds")]
    public List<string> OfferIds { get; init; } = new();
}

/// <summary>
/// Request to withdraw offers from Alle Discount campaign.
/// </summary>
public record AlleDiscountWithdrawOffersRequest
{
    /// <summary>
    /// Campaign identifier.
    /// </summary>
    [JsonPropertyName("campaignId")]
    public string CampaignId { get; init; } = string.Empty;

    /// <summary>
    /// List of offer identifiers to withdraw.
    /// </summary>
    [JsonPropertyName("offerIds")]
    public List<string> OfferIds { get; init; } = new();
}

/// <summary>
/// Response from submit/withdraw command.
/// </summary>
public record AlleDiscountCommandResponse
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Command status (RUNNING, SUCCESS, FAILED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
}

/// <summary>
/// List of eligible offers for a campaign.
/// </summary>
public record AlleDiscountEligibleOffers
{
    /// <summary>
    /// List of offer identifiers.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<string> Offers { get; init; } = new();

    /// <summary>
    /// Total count of eligible offers.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }
}

/// <summary>
/// List of submitted offers in a campaign.
/// </summary>
public record AlleDiscountSubmittedOffers
{
    /// <summary>
    /// List of offer identifiers.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<string> Offers { get; init; } = new();

    /// <summary>
    /// Total count of submitted offers.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }
}
