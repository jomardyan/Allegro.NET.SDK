using System.Text.Json.Serialization;

namespace AllegroApi.Models.Common;

/// <summary>
/// Base class for paginated responses
/// </summary>
public class PagedResponse<T>
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("offset")]
    public int? Offset { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();
}

/// <summary>
/// Pagination parameters for API requests
/// </summary>
public class PaginationParams
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }

    public Dictionary<string, string> ToQueryParams()
    {
        var parameters = new Dictionary<string, string>();
        if (Offset.HasValue) parameters["offset"] = Offset.Value.ToString();
        if (Limit.HasValue) parameters["limit"] = Limit.Value.ToString();
        return parameters;
    }
}
