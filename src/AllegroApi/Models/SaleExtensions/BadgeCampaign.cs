using System.Text.Json.Serialization;

namespace AllegroApi.Models.SaleExtensions;

/// <summary>
/// Response containing list of badge campaigns.
/// </summary>
public record GetBadgeCampaignsList
{
    /// <summary>
    /// List of badge campaigns.
    /// </summary>
    [JsonPropertyName("badgeCampaigns")]
    public List<BadgeCampaign>? BadgeCampaigns { get; init; }
}

/// <summary>
/// Badge campaign information.
/// </summary>
public record BadgeCampaign
{
    /// <summary>
    /// Campaign identifier (e.g., "BARGAIN", "HIT").
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Campaign name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Marketplace information.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public BadgeCampaignMarketplace? Marketplace { get; init; }

    /// <summary>
    /// Campaign type (DISCOUNT, STANDARD, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Eligibility information.
    /// </summary>
    [JsonPropertyName("eligibility")]
    public BadgeCampaignEligibility? Eligibility { get; init; }

    /// <summary>
    /// Application settings.
    /// </summary>
    [JsonPropertyName("application")]
    public BadgeCampaignApplication? Application { get; init; }

    /// <summary>
    /// Visibility settings.
    /// </summary>
    [JsonPropertyName("visibility")]
    public BadgeCampaignVisibility? Visibility { get; init; }

    /// <summary>
    /// Publication settings.
    /// </summary>
    [JsonPropertyName("publication")]
    public BadgeCampaignPublication? Publication { get; init; }

    /// <summary>
    /// Link to campaign regulations.
    /// </summary>
    [JsonPropertyName("regulationsLink")]
    public string? RegulationsLink { get; init; }
}

/// <summary>
/// Badge campaign marketplace information.
/// </summary>
public record BadgeCampaignMarketplace
{
    /// <summary>
    /// Marketplace identifier (e.g., "allegro-pl").
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Badge campaign eligibility information.
/// </summary>
public record BadgeCampaignEligibility
{
    /// <summary>
    /// Whether the user is eligible for the campaign.
    /// </summary>
    [JsonPropertyName("eligible")]
    public bool? Eligible { get; init; }

    /// <summary>
    /// List of refusal reasons if not eligible.
    /// </summary>
    [JsonPropertyName("refusalReasons")]
    public List<BadgeCampaignRefusalReason>? RefusalReasons { get; init; }
}

/// <summary>
/// Refusal reason for badge campaign eligibility.
/// </summary>
public record BadgeCampaignRefusalReason
{
    /// <summary>
    /// Refusal reason code.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// List of refusal messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<BadgeCampaignMessage>? Messages { get; init; }
}

/// <summary>
/// Badge campaign message.
/// </summary>
public record BadgeCampaignMessage
{
    /// <summary>
    /// Message text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// Optional link for more information.
    /// </summary>
    [JsonPropertyName("link")]
    public string? Link { get; init; }
}

/// <summary>
/// Badge campaign application settings.
/// </summary>
public record BadgeCampaignApplication
{
    /// <summary>
    /// Application type (ALWAYS, NEVER, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>
/// Badge campaign visibility settings.
/// </summary>
public record BadgeCampaignVisibility
{
    /// <summary>
    /// Visibility type (ALWAYS, WITHIN, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Visibility start date.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Visibility end date.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// Badge campaign publication settings.
/// </summary>
public record BadgeCampaignPublication
{
    /// <summary>
    /// Publication type (UNTIL, SINCE, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Publication start date.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Publication end date.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}
