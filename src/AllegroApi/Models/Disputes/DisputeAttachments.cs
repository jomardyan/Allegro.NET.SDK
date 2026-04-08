using System.Text.Json.Serialization;

namespace AllegroApi.Models.Disputes;

/// <summary>
/// Represents a declaration for a dispute attachment.
/// </summary>
public record AttachmentDeclaration
{
    /// <summary>
    /// Name of the file to be attached.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// Size of the file in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    public int Size { get; init; }
}

/// <summary>
/// Represents the identifier of a dispute attachment.
/// </summary>
public record DisputeAttachmentId
{
    /// <summary>
    /// Unique identifier of the attachment.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}
