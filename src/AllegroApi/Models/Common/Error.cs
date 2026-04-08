using System.Text.Json.Serialization;

namespace AllegroApi.Models.Common;

/// <summary>
/// Represents an error in the Allegro API response
/// </summary>
public class Error
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public string? Details { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("userMessage")]
    public string? UserMessage { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Container for errors returned by the API
/// </summary>
public class ErrorsHolder
{
    [JsonPropertyName("errors")]
    public List<Error> Errors { get; set; } = new();
}

/// <summary>
/// Authentication error response
/// </summary>
public class AuthError
{
    [JsonPropertyName("error")]
    public string ErrorType { get; set; } = string.Empty;

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;
}
