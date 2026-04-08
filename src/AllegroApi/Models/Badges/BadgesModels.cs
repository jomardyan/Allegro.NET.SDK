using System.Text.Json.Serialization;

namespace AllegroApi.Models.Badges;

/// <summary>
/// Represents a list of badge campaigns.
/// </summary>
public record GetBadgeCampaignsList
{
    /// <summary>
    /// Gets the list of badge campaigns.
    /// </summary>
    [JsonPropertyName("badgeCampaigns")]
    public List<BadgeCampaign> BadgeCampaigns { get; init; } = new();
}

/// <summary>
/// Represents a badge campaign.
/// </summary>
public record BadgeCampaign
{
    /// <summary>
    /// Gets the badge campaign ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the badge campaign name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the marketplace reference.
    /// </summary>
    [JsonPropertyName("marketplace")]
    public MarketplaceReference? Marketplace { get; init; }

    /// <summary>
    /// Gets the campaign type (DISCOUNT, STANDARD, SOURCING).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the eligibility information.
    /// </summary>
    [JsonPropertyName("eligibility")]
    public UserCampaignEligibility? Eligibility { get; init; }

    /// <summary>
    /// Gets the application time policy.
    /// </summary>
    [JsonPropertyName("application")]
    public ApplicationTimePolicy? Application { get; init; }

    /// <summary>
    /// Gets the visibility time policy.
    /// </summary>
    [JsonPropertyName("visibility")]
    public VisibilityTimePolicy? Visibility { get; init; }

    /// <summary>
    /// Gets the publication time policy.
    /// </summary>
    [JsonPropertyName("publication")]
    public PublicationTimePolicy? Publication { get; init; }

    /// <summary>
    /// Gets the link to campaign Terms and Conditions.
    /// </summary>
    [JsonPropertyName("regulationsLink")]
    public string? RegulationsLink { get; init; }
}

/// <summary>
/// Represents a marketplace reference.
/// </summary>
public record MarketplaceReference
{
    /// <summary>
    /// Gets the marketplace ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Represents user eligibility for campaign participation.
/// </summary>
public record UserCampaignEligibility
{
    /// <summary>
    /// Gets whether the user is eligible to participate.
    /// </summary>
    [JsonPropertyName("eligible")]
    public bool Eligible { get; init; }

    /// <summary>
    /// Gets the reasons why the user cannot participate.
    /// </summary>
    [JsonPropertyName("refusalReasons")]
    public List<CampaignRefusalReason>? RefusalReasons { get; init; }
}

/// <summary>
/// Represents a campaign refusal reason.
/// </summary>
public record CampaignRefusalReason
{
    /// <summary>
    /// Gets the reason code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of refusal messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<RefusalMessage>? Messages { get; init; }
}

/// <summary>
/// Represents a refusal message.
/// </summary>
public record RefusalMessage
{
    /// <summary>
    /// Gets the detailed message text.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Gets the optional link associated with the refusal reason.
    /// </summary>
    [JsonPropertyName("link")]
    public string? Link { get; init; }
}

/// <summary>
/// Represents a time policy for application.
/// </summary>
public record ApplicationTimePolicy
{
    /// <summary>
    /// Gets the policy type (ALWAYS, SINCE, WITHIN, UNTIL, NEVER).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the start date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Gets the end date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// Represents a time policy for visibility.
/// </summary>
public record VisibilityTimePolicy
{
    /// <summary>
    /// Gets the policy type (ALWAYS, SINCE, WITHIN, UNTIL, NEVER).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the start date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Gets the end date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// Represents a time policy for publication.
/// </summary>
public record PublicationTimePolicy
{
    /// <summary>
    /// Gets the policy type (ALWAYS, SINCE, WITHIN, UNTIL, NEVER).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the start date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("from")]
    public DateTime? From { get; init; }

    /// <summary>
    /// Gets the end date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("to")]
    public DateTime? To { get; init; }
}

/// <summary>
/// Represents a badge application request.
/// </summary>
public record BadgeApplicationRequest
{
    /// <summary>
    /// Gets or sets the campaign information.
    /// </summary>
    [JsonPropertyName("campaign")]
    public BadgeApplicationCampaign Campaign { get; init; } = new();

    /// <summary>
    /// Gets or sets the offer information.
    /// </summary>
    [JsonPropertyName("offer")]
    public BadgeApplicationOffer Offer { get; init; } = new();

    /// <summary>
    /// Gets or sets the prices (required for DISCOUNT and SOURCING campaigns).
    /// </summary>
    [JsonPropertyName("prices")]
    public BadgeApplicationPrices? Prices { get; init; }

    /// <summary>
    /// Gets or sets the purchase constraints.
    /// </summary>
    [JsonPropertyName("purchaseConstraints")]
    public BadgeApplicationPurchaseConstraints? PurchaseConstraints { get; init; }
}

/// <summary>
/// Represents campaign information in a badge application.
/// </summary>
public record BadgeApplicationCampaign
{
    /// <summary>
    /// Gets or sets the badge campaign ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}

/// <summary>
/// Represents offer information in a badge application.
/// </summary>
public record BadgeApplicationOffer
{
    /// <summary>
    /// Gets or sets the offer ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}

/// <summary>
/// Represents prices in a badge application.
/// </summary>
public record BadgeApplicationPrices
{
    /// <summary>
    /// Gets or sets the bargain price (required for DISCOUNT and SOURCING campaigns).
    /// </summary>
    [JsonPropertyName("bargain")]
    public BadgePrice? Bargain { get; init; }
}

/// <summary>
/// Represents a badge price.
/// </summary>
public record BadgePrice
{
    /// <summary>
    /// Gets or sets the amount (decimal string).
    /// </summary>
    [JsonPropertyName("amount")]
    public string Amount { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the currency (ISO 4217 code).
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;
}

/// <summary>
/// Represents purchase constraints for a badge application.
/// </summary>
public record BadgeApplicationPurchaseConstraints
{
    /// <summary>
    /// Gets or sets the purchase limit.
    /// </summary>
    [JsonPropertyName("limit")]
    public BadgeApplicationPurchaseConstraintsLimit? Limit { get; init; }
}

/// <summary>
/// Represents purchase constraint limits.
/// </summary>
public record BadgeApplicationPurchaseConstraintsLimit
{
    /// <summary>
    /// Gets or sets the per-user limit.
    /// </summary>
    [JsonPropertyName("perUser")]
    public BadgeApplicationPurchaseConstraintsLimitPerUser? PerUser { get; init; }
}

/// <summary>
/// Represents per-user purchase constraint limits.
/// </summary>
public record BadgeApplicationPurchaseConstraintsLimitPerUser
{
    /// <summary>
    /// Gets or sets the maximum number of items per user.
    /// </summary>
    [JsonPropertyName("maxItems")]
    public int MaxItems { get; init; }
}

/// <summary>
/// Represents a list of badge applications.
/// </summary>
public record BadgeApplications
{
    /// <summary>
    /// Gets the list of badge applications.
    /// </summary>
    [JsonPropertyName("badgeApplications")]
    public List<BadgeApplication> Applications { get; init; } = new();
}

/// <summary>
/// Represents a badge application.
/// </summary>
public record BadgeApplication
{
    /// <summary>
    /// Gets the badge application ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the creation date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the last update date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Gets the campaign information.
    /// </summary>
    [JsonPropertyName("campaign")]
    public BadgeApplicationCampaign Campaign { get; init; } = new();

    /// <summary>
    /// Gets the offer information.
    /// </summary>
    [JsonPropertyName("offer")]
    public BadgeApplicationOffer Offer { get; init; } = new();

    /// <summary>
    /// Gets the prices.
    /// </summary>
    [JsonPropertyName("prices")]
    public BadgeApplicationPrices? Prices { get; init; }

    /// <summary>
    /// Gets the application process information.
    /// </summary>
    [JsonPropertyName("process")]
    public BadgeApplicationProcess Process { get; init; } = new();

    /// <summary>
    /// Gets the purchase constraints.
    /// </summary>
    [JsonPropertyName("purchaseConstraints")]
    public BadgeApplicationPurchaseConstraints? PurchaseConstraints { get; init; }
}

/// <summary>
/// Represents the badge application process status.
/// </summary>
public record BadgeApplicationProcess
{
    /// <summary>
    /// Gets the status (REQUESTED, PROCESSED, DECLINED).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the rejection reasons (for DECLINED status).
    /// </summary>
    [JsonPropertyName("rejectionReasons")]
    public List<BadgeApplicationRejectionReason>? RejectionReasons { get; init; }
}

/// <summary>
/// Represents a badge application rejection reason.
/// </summary>
public record BadgeApplicationRejectionReason
{
    /// <summary>
    /// Gets the reason code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets the rejection reason messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<RefusalMessage>? Messages { get; init; }
}

/// <summary>
/// Represents a badge operation.
/// </summary>
public record BadgeOperation
{
    /// <summary>
    /// Gets the badge operation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the operation type (FINISH, UPDATE).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the creation date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the last update date in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Gets the campaign information.
    /// </summary>
    [JsonPropertyName("campaign")]
    public BadgeApplicationCampaign Campaign { get; init; } = new();

    /// <summary>
    /// Gets the offer information.
    /// </summary>
    [JsonPropertyName("offer")]
    public BadgeApplicationOffer Offer { get; init; } = new();

    /// <summary>
    /// Gets the prices.
    /// </summary>
    [JsonPropertyName("prices")]
    public BadgeApplicationPrices? Prices { get; init; }

    /// <summary>
    /// Gets the operation process information.
    /// </summary>
    [JsonPropertyName("process")]
    public BadgeApplicationProcess Process { get; init; } = new();
}

/// <summary>
/// Represents a badge patch request for updating or finishing a badge.
/// </summary>
public record BadgePatchRequest
{
    /// <summary>
    /// Gets or sets the process update (for finishing a badge).
    /// </summary>
    [JsonPropertyName("process")]
    public BadgePatchProcess? Process { get; init; }

    /// <summary>
    /// Gets or sets the price update.
    /// </summary>
    [JsonPropertyName("prices")]
    public BadgePatchPrices? Prices { get; init; }
}

/// <summary>
/// Represents a process update for a badge patch.
/// </summary>
public record BadgePatchProcess
{
    /// <summary>
    /// Gets or sets the status (FINISHED to remove badge).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
}

/// <summary>
/// Represents price updates for a badge patch.
/// </summary>
public record BadgePatchPrices
{
    /// <summary>
    /// Gets or sets the bargain price update.
    /// </summary>
    [JsonPropertyName("bargain")]
    public BadgePatchPriceBargain? Bargain { get; init; }
}

/// <summary>
/// Represents bargain price value for a badge patch.
/// </summary>
public record BadgePatchPriceBargain
{
    /// <summary>
    /// Gets or sets the price value.
    /// </summary>
    [JsonPropertyName("value")]
    public BadgePrice? Value { get; init; }
}

/// <summary>
/// Represents the response from a badge patch operation.
/// </summary>
public record BadgePatchResponse
{
    /// <summary>
    /// Gets the operation ID for tracking the update status.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}
