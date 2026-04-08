using System.Text.Json.Serialization;

namespace AllegroApi.Models.Offers;

/// <summary>
/// Request to publish/unpublish offers
/// </summary>
public class PublicationChangeCommandDto
{
    [JsonPropertyName("publication")]
    public PublicationCommand Publication { get; set; } = new();

    [JsonPropertyName("offerCriteria")]
    public List<OfferCriterion> OfferCriteria { get; set; } = new();
}

public class PublicationCommand
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty; // "ACTIVATE", "END"

    [JsonPropertyName("scheduledAt")]
    public DateTime? ScheduledAt { get; set; }
}

public class OfferCriterion
{
    [JsonPropertyName("offers")]
    public List<OfferIdReference>? Offers { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "CONTAINS_OFFERS";
}

public class OfferIdReference
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

/// <summary>
/// General report for batch operations
/// </summary>
public class GeneralReport
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public Summary? Summary { get; set; }
}

public class Summary
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    [JsonPropertyName("pending")]
    public int Pending { get; set; }
}

/// <summary>
/// Task report for individual operations
/// </summary>
public class TaskReport
{
    [JsonPropertyName("tasks")]
    public List<Task> Tasks { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

public class Task
{
    [JsonPropertyName("offerId")]
    public string OfferId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("errors")]
    public List<Common.Error> Errors { get; set; } = new();

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// Request to change price
/// </summary>
public class ChangePriceWithoutOutput
{
    [JsonPropertyName("input")]
    public ChangePriceInput Input { get; set; } = new();
}

public class ChangePriceInput
{
    [JsonPropertyName("buyNowPrice")]
    public Common.Money BuyNowPrice { get; set; } = new();
}

public class ChangePrice
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("input")]
    public ChangePriceInput Input { get; set; } = new();

    [JsonPropertyName("output")]
    public ChangePriceOutput? Output { get; set; }
}

public class ChangePriceOutput
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("errors")]
    public List<Common.Error> Errors { get; set; } = new();
}
