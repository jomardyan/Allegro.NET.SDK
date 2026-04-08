using System.Text.Json.Serialization;

namespace AllegroApi.Models.Common;

/// <summary>
/// Response containing list of contacts.
/// </summary>
public record ContactResponseList
{
    /// <summary>
    /// List of contacts.
    /// </summary>
    [JsonPropertyName("contacts")]
    public List<ContactResponse>? Contacts { get; init; }
}

/// <summary>
/// Contact information.
/// </summary>
public record ContactResponse
{
    /// <summary>
    /// Contact identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Contact name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Company name.
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// Contact address.
    /// </summary>
    [JsonPropertyName("address")]
    public ContactAddress? Address { get; init; }
}

/// <summary>
/// Request to create or update a contact.
/// </summary>
public record ContactRequest
{
    /// <summary>
    /// Contact name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Company name.
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// Contact address.
    /// </summary>
    [JsonPropertyName("address")]
    public ContactAddress? Address { get; init; }
}

/// <summary>
/// Contact address information.
/// </summary>
public record ContactAddress
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
    [JsonPropertyName("zipCode")]
    public string? ZipCode { get; init; }

    /// <summary>
    /// Country code.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}
