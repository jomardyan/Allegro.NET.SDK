using System.Text.Json.Serialization;

namespace AllegroApi.Models.Account;

/// <summary>
/// Response containing list of additional email addresses.
/// </summary>
public record AdditionalEmailsResponse
{
    /// <summary>
    /// List of additional email addresses.
    /// </summary>
    [JsonPropertyName("emails")]
    public List<AdditionalEmail>? Emails { get; init; }
}

/// <summary>
/// Additional email address information.
/// </summary>
public record AdditionalEmail
{
    /// <summary>
    /// Email identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Whether the email is verified.
    /// </summary>
    [JsonPropertyName("verified")]
    public bool? Verified { get; init; }

    /// <summary>
    /// When the email was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Request to add a new additional email address.
/// </summary>
public record AdditionalEmailRequest
{
    /// <summary>
    /// Email address to add.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }
}
