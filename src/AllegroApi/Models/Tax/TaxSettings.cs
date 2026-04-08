using System.Text.Json.Serialization;

namespace AllegroApi.Models.Tax;

/// <summary>
/// Represents tax settings for a category.
/// </summary>
public record CategoryTaxSettings
{
    /// <summary>
    /// A list of tax subjects.
    /// </summary>
    [JsonPropertyName("subjects")]
    public List<TaxSubject> Subjects { get; init; } = new();

    /// <summary>
    /// A list of tax rates.
    /// </summary>
    [JsonPropertyName("rates")]
    public List<TaxRate> Rates { get; init; } = new();

    /// <summary>
    /// A list of tax exemptions.
    /// </summary>
    [JsonPropertyName("exemptions")]
    public List<TaxExemption> Exemptions { get; init; } = new();
}

/// <summary>
/// Represents a tax subject (e.g., Goods, Services).
/// </summary>
public record TaxSubject
{
    /// <summary>
    /// Displayable tax subject label.
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Value of subject (e.g., GOODS, SERVICES).
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;
}

/// <summary>
/// Represents tax rates for a specific country.
/// </summary>
public record TaxRate
{
    /// <summary>
    /// A country code for which given VAT setting is defined.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    /// Values of tax rates for given country code.
    /// </summary>
    [JsonPropertyName("values")]
    public List<TaxRateValue> Values { get; init; } = new();
}

/// <summary>
/// Represents a specific tax rate value.
/// </summary>
public record TaxRateValue
{
    /// <summary>
    /// Displayable tax rate label.
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// A numeric value of VAT tax rate. In case of "OUT_OF_SCOPE_OF_VAT" it is set to 0.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Exemption field must be filled out if true, otherwise is optional.
    /// </summary>
    [JsonPropertyName("exemptionRequired")]
    public bool ExemptionRequired { get; init; }
}

/// <summary>
/// Represents a tax exemption option.
/// </summary>
public record TaxExemption
{
    /// <summary>
    /// Displayable exemption label.
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Value of exemption (e.g., MARGIN_SCHEME).
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;
}
