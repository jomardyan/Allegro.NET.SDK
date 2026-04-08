using System.Text.Json.Serialization;

namespace AllegroApi.Models.Users;

/// <summary>
/// Public user information.
/// </summary>
public record UserInfo
{
    /// <summary>
    /// User identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// User login name.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// User email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// User's company name (if business account).
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// When the user was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// User address.
    /// </summary>
    [JsonPropertyName("address")]
    public UserAddress? Address { get; init; }
}

/// <summary>
/// User address information.
/// </summary>
public record UserAddress
{
    /// <summary>
    /// Street address.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// City name.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Postal code.
    /// </summary>
    [JsonPropertyName("postCode")]
    public string? PostCode { get; init; }

    /// <summary>
    /// Country code.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}
