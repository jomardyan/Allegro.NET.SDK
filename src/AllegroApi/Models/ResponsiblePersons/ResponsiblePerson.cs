using System.Text.Json.Serialization;

namespace AllegroApi.Models.ResponsiblePersons;

/// <summary>
/// Response containing a list of responsible persons for EU GPSR compliance.
/// </summary>
public record ResponsiblePersonsListResponse
{
    /// <summary>
    /// List of responsible persons.
    /// </summary>
    [JsonPropertyName("responsiblePersons")]
    public List<ResponsiblePersonResponse>? ResponsiblePersons { get; init; }

    /// <summary>
    /// Number of responsible persons returned in the current response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>
    /// Total number of available responsible persons.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}

/// <summary>
/// Responsible person details for EU GPSR compliance.
/// A responsible person ensures that the product complies with EU regulations.
/// </summary>
public record ResponsiblePersonResponse
{
    /// <summary>
    /// Responsible person identifier (UUID format).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Internal name of responsible person in dictionary (visible only to you).
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Responsible person personal data including name, address, and contact information.
    /// </summary>
    [JsonPropertyName("personalData")]
    public ResponsiblePersonPersonalData? PersonalData { get; init; }
}

/// <summary>
/// Request to create a new responsible person.
/// </summary>
public record CreateResponsiblePersonRequest
{
    /// <summary>
    /// Internal name of responsible person in dictionary (visible only to you).
    /// Cannot start or end with whitespace, cannot contain multiple spaces in a row.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Responsible person personal data including name, address, and contact information.
    /// </summary>
    [JsonPropertyName("personalData")]
    public ResponsiblePersonPersonalData PersonalData { get; init; } = null!;
}

/// <summary>
/// Request to update an existing responsible person.
/// </summary>
public record UpdateResponsiblePersonRequest
{
    /// <summary>
    /// Responsible person identifier (UUID format).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Internal name of responsible person in dictionary (visible only to you).
    /// Cannot start or end with whitespace, cannot contain multiple spaces in a row.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Responsible person personal data including name, address, and contact information.
    /// </summary>
    [JsonPropertyName("personalData")]
    public ResponsiblePersonPersonalData? PersonalData { get; init; }
}

/// <summary>
/// Responsible person personal data.
/// </summary>
public record ResponsiblePersonPersonalData
{
    /// <summary>
    /// Name of responsible person.
    /// Cannot start or end with whitespace, cannot contain multiple spaces in a row.
    /// Max length: 200 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Responsible person address.
    /// </summary>
    [JsonPropertyName("address")]
    public ResponsiblePersonAddress? Address { get; init; }

    /// <summary>
    /// Responsible person contact information.
    /// </summary>
    [JsonPropertyName("contact")]
    public ResponsiblePersonContact? Contact { get; init; }
}

/// <summary>
/// Responsible person address.
/// </summary>
public record ResponsiblePersonAddress
{
    /// <summary>
    /// Country code (ISO 3166). Must be an EU country.
    /// Example: PL, DE, FR, IT, etc.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }

    /// <summary>
    /// Street and number.
    /// Max length: 200 characters.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }

    /// <summary>
    /// Postal code.
    /// Max length: 20 characters.
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; init; }

    /// <summary>
    /// City name.
    /// Max length: 100 characters.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }
}

/// <summary>
/// Responsible person contact information.
/// At least one of email or formUrl is required.
/// </summary>
public record ResponsiblePersonContact
{
    /// <summary>
    /// Email address of responsible person.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Phone number of responsible person (optional).
    /// Max length: 30 characters.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// URL address to contact form.
    /// Max length: 80 characters.
    /// </summary>
    [JsonPropertyName("formUrl")]
    public string? FormUrl { get; init; }
}
