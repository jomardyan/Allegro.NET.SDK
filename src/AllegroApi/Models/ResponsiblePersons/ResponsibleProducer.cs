using System.Text.Json.Serialization;

namespace AllegroApi.Models.ResponsiblePersons;

/// <summary>
/// Response containing a list of responsible producers for EU GPSR compliance.
/// </summary>
public record ResponsibleProducersListResponse
{
    /// <summary>
    /// List of responsible producers.
    /// </summary>
    [JsonPropertyName("responsibleProducers")]
    public List<ResponsibleProducerResponse>? ResponsibleProducers { get; init; }

    /// <summary>
    /// Number of responsible producers returned in the current response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }

    /// <summary>
    /// Total number of available responsible producers.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}

/// <summary>
/// Responsible producer details for EU GPSR compliance.
/// Contains contact information for the company responsible for producing the product.
/// </summary>
public record ResponsibleProducerResponse
{
    /// <summary>
    /// Responsible producer identifier (UUID format).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Internal name of responsible producer in dictionary (visible only to you).
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Responsible producer data including trade name, address, and contact information.
    /// </summary>
    [JsonPropertyName("producerData")]
    public ResponsibleProducerData? ProducerData { get; init; }
}

/// <summary>
/// Request to create a new responsible producer.
/// </summary>
public record CreateResponsibleProducerRequest
{
    /// <summary>
    /// Internal name of responsible producer in dictionary (visible only to you).
    /// Cannot start or end with whitespace, cannot contain multiple spaces in a row.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Responsible producer data including trade name, address, and contact information.
    /// </summary>
    [JsonPropertyName("producerData")]
    public ResponsibleProducerData ProducerData { get; init; } = null!;
}

/// <summary>
/// Request to update an existing responsible producer.
/// </summary>
public record UpdateResponsibleProducerRequest
{
    /// <summary>
    /// Responsible producer identifier (UUID format).
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Internal name of responsible producer in dictionary (visible only to you).
    /// Cannot start or end with whitespace, cannot contain multiple spaces in a row.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Responsible producer data including trade name, address, and contact information.
    /// </summary>
    [JsonPropertyName("producerData")]
    public ResponsibleProducerData? ProducerData { get; init; }
}

/// <summary>
/// Responsible producer data.
/// </summary>
public record ResponsibleProducerData
{
    /// <summary>
    /// Name of company, first name and last name, or trade name of company responsible for producing product.
    /// Max length: 200 characters.
    /// </summary>
    [JsonPropertyName("tradeName")]
    public string? TradeName { get; init; }

    /// <summary>
    /// Responsible producer address.
    /// </summary>
    [JsonPropertyName("address")]
    public ResponsibleProducerAddress? Address { get; init; }

    /// <summary>
    /// Responsible producer contact information.
    /// </summary>
    [JsonPropertyName("contact")]
    public ResponsibleProducerContact? Contact { get; init; }
}

/// <summary>
/// Responsible producer address.
/// </summary>
public record ResponsibleProducerAddress
{
    /// <summary>
    /// Country code (ISO 3166).
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
/// Responsible producer contact information.
/// At least one of email or formUrl is required.
/// </summary>
public record ResponsibleProducerContact
{
    /// <summary>
    /// Email address of responsible producer.
    /// Max length: 50 characters.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Phone number of responsible producer (optional).
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
