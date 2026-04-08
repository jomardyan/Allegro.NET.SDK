using System.Text.Json.Serialization;

namespace AllegroApi.Models.Offers;

public class OfferTranslations
{
    [JsonPropertyName("translations")]
    public List<Translation> Translations { get; set; } = new();
}

public class Translation
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("safetyInformation")]
    public string? SafetyInformation { get; set; }
}

public class ManualTranslationUpdateRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("safetyInformation")]
    public List<SafetyInformationTranslation>? SafetyInformation { get; set; }
}

public class SafetyInformationTranslation
{
    [JsonPropertyName("productId")]
    public string ProductId { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}
