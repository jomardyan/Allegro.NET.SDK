using System.Text.Json.Serialization;

namespace AllegroApi.Models.Miscellaneous;

/// <summary>
/// Represents search results for fundraising campaigns.
/// </summary>
public record FundraisingCampaigns
{
    /// <summary>
    /// List of fundraising campaigns.
    /// </summary>
    [JsonPropertyName("campaigns")]
    public List<FundraisingCampaign>? Campaigns { get; init; }
}

/// <summary>
/// Represents a charity fundraising campaign.
/// </summary>
public record FundraisingCampaign
{
    /// <summary>
    /// Unique campaign identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Campaign name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Organization running the campaign.
    /// </summary>
    [JsonPropertyName("organization")]
    public CharityOrganization? Organization { get; init; }
}

/// <summary>
/// Represents a charity organization.
/// </summary>
public record CharityOrganization
{
    /// <summary>
    /// Organization name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}
