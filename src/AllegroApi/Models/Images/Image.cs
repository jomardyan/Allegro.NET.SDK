using System.Text.Json.Serialization;

namespace AllegroApi.Models.Images;

public record ImageUploadResponse
{
    [JsonPropertyName("location")]
    public string? Location { get; init; }

    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; init; }
}

public record ImageUploadByUrlRequest
{
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;
}
