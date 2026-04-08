using System.Text.Json.Serialization;

namespace AllegroApi.Models.BatchOperations;

/// <summary>
/// Command to modify multiple offers at once.
/// </summary>
public record OfferChangeCommand
{
    /// <summary>
    /// List of offers to modify with their new values.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferModificationDto>? Offers { get; init; }
}

/// <summary>
/// Offer modification details.
/// </summary>
public record OfferModificationDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// New name for the offer (optional).
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// New description (optional).
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// New images (optional).
    /// </summary>
    [JsonPropertyName("images")]
    public List<string>? Images { get; init; }

    /// <summary>
    /// New stock quantity (optional).
    /// </summary>
    [JsonPropertyName("stock")]
    public StockDto? Stock { get; init; }
}

/// <summary>
/// Stock information.
/// </summary>
public record StockDto
{
    /// <summary>
    /// Available quantity.
    /// </summary>
    [JsonPropertyName("available")]
    public int? Available { get; init; }

    /// <summary>
    /// Unit (e.g., "PIECE").
    /// </summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

/// <summary>
/// Command to change prices of multiple offers.
/// </summary>
public record OfferPriceChangeCommand
{
    /// <summary>
    /// List of offers with new prices.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferPriceDto>? Offers { get; init; }
}

/// <summary>
/// Offer price change details.
/// </summary>
public record OfferPriceDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// New buyNow price.
    /// </summary>
    [JsonPropertyName("buyNowPrice")]
    public PriceDto? BuyNowPrice { get; init; }
}

/// <summary>
/// Price information with amount and currency.
/// </summary>
public record PriceDto
{
    /// <summary>
    /// Price amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    /// <summary>
    /// Currency code (e.g., "PLN").
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Command to change quantities of multiple offers.
/// </summary>
public record OfferQuantityChangeCommand
{
    /// <summary>
    /// List of offers with new quantities.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferQuantityDto>? Offers { get; init; }
}

/// <summary>
/// Offer quantity change details.
/// </summary>
public record OfferQuantityDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// New stock quantity.
    /// </summary>
    [JsonPropertyName("stock")]
    public StockDto? Stock { get; init; }
}

/// <summary>
/// Command to modify automatic pricing rules for multiple offers.
/// </summary>
public record OfferPriceAutomationCommand
{
    /// <summary>
    /// List of offers with new pricing rules.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<OfferPriceAutomationDto>? Offers { get; init; }
}

/// <summary>
/// Offer pricing automation details.
/// </summary>
public record OfferPriceAutomationDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Pricing rules configuration.
    /// </summary>
    [JsonPropertyName("priceAutomation")]
    public PriceAutomationDto? PriceAutomation { get; init; }
}

/// <summary>
/// Automatic pricing rules configuration.
/// </summary>
public record PriceAutomationDto
{
    /// <summary>
    /// Whether automatic pricing is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Minimum price threshold.
    /// </summary>
    [JsonPropertyName("minPrice")]
    public PriceDto? MinPrice { get; init; }

    /// <summary>
    /// Maximum price threshold.
    /// </summary>
    [JsonPropertyName("maxPrice")]
    public PriceDto? MaxPrice { get; init; }
}

/// <summary>
/// General report for batch command status.
/// </summary>
public record GeneralReport
{
    /// <summary>
    /// Command identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Command status (e.g., "NEW", "RUNNING", "ENDED").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Total number of offers in the command.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// Number of successfully processed offers.
    /// </summary>
    [JsonPropertyName("success")]
    public int? Success { get; init; }

    /// <summary>
    /// Number of failed offers.
    /// </summary>
    [JsonPropertyName("failed")]
    public int? Failed { get; init; }

    /// <summary>
    /// Number of offers currently being processed.
    /// </summary>
    [JsonPropertyName("processing")]
    public int? Processing { get; init; }
}

/// <summary>
/// Detailed task report for batch command.
/// </summary>
public record TaskReport
{
    /// <summary>
    /// List of individual task statuses.
    /// </summary>
    [JsonPropertyName("tasks")]
    public List<TaskStatusDto>? Tasks { get; init; }

    /// <summary>
    /// Total count of tasks.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }
}

/// <summary>
/// Individual task status within a batch command.
/// </summary>
public record TaskStatusDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Task status (e.g., "SUCCESS", "FAIL").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Error message (if task failed).
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// Error code (if task failed).
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }
}
