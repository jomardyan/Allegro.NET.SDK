using System.Text.Json.Serialization;

namespace AllegroApi.Models.Classifieds;

/// <summary>
/// Represents the classified packages response (packages assigned to an offer).
/// </summary>
public record ClassifiedResponse
{
    /// <summary>
    /// Gets the base package assigned to the classified offer.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public ClassifiedPackage BasePackage { get; init; } = new();

    /// <summary>
    /// Gets the list of extra packages assigned to the classified offer.
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<ClassifiedExtraPackage> ExtraPackages { get; init; } = new();
}

/// <summary>
/// Represents a classified package.
/// </summary>
public record ClassifiedPackage
{
    /// <summary>
    /// Gets the classifieds package ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Represents an extra classified package.
/// </summary>
public record ClassifiedExtraPackage
{
    /// <summary>
    /// Gets the classifieds extra package ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets whether this extra package will be recreated when the offer is republished.
    /// Extra package with this flag set to true will be recreated when offer is being republished.
    /// </summary>
    [JsonPropertyName("republish")]
    public bool Republish { get; init; }
}

/// <summary>
/// Represents a request to assign classified packages to an offer.
/// </summary>
public record ClassifiedPackages
{
    /// <summary>
    /// Gets or sets the base package to assign.
    /// </summary>
    [JsonPropertyName("basePackage")]
    public ClassifiedPackage BasePackage { get; init; } = new();

    /// <summary>
    /// Gets or sets the list of extra packages to assign.
    /// </summary>
    [JsonPropertyName("extraPackages")]
    public List<ClassifiedPackage>? ExtraPackages { get; init; }
}

/// <summary>
/// Represents a list of classified package configurations.
/// </summary>
public record ClassifiedPackageConfigs
{
    /// <summary>
    /// Gets the list of package configurations.
    /// </summary>
    [JsonPropertyName("packages")]
    public List<ClassifiedPackageConfig> Packages { get; init; } = new();
}

/// <summary>
/// Represents a classified package configuration.
/// </summary>
public record ClassifiedPackageConfig
{
    /// <summary>
    /// Gets the classifieds package ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the classifieds package name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the package type (BASE or EXTRA).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of extensions included in the package.
    /// </summary>
    [JsonPropertyName("extensions")]
    public List<ClassifiedExtension>? Extensions { get; init; }

    /// <summary>
    /// Gets the list of additional promotions included in the package.
    /// </summary>
    [JsonPropertyName("promotions")]
    public List<ClassifiedPromotion>? Promotions { get; init; }

    /// <summary>
    /// Gets the publication configuration.
    /// </summary>
    [JsonPropertyName("publication")]
    public ClassifiedPublication? Publication { get; init; }
}

/// <summary>
/// Represents a classified extension (e.g., external site export).
/// </summary>
public record ClassifiedExtension
{
    /// <summary>
    /// Gets the classified extension name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the classified extension description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Represents a classified promotion.
/// </summary>
public record ClassifiedPromotion
{
    /// <summary>
    /// Gets the name of the promotion.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the duration in ISO 8601 format (e.g., "PT240H").
    /// </summary>
    [JsonPropertyName("duration")]
    public string Duration { get; init; } = string.Empty;
}

/// <summary>
/// Represents classified publication configuration.
/// </summary>
public record ClassifiedPublication
{
    /// <summary>
    /// Gets the duration in ISO 8601 format (e.g., "PT240H").
    /// </summary>
    [JsonPropertyName("duration")]
    public string Duration { get; init; } = string.Empty;
}

/// <summary>
/// Response containing daily statistics for the seller's classified ads.
/// </summary>
public record SellerOfferStatsResponseDto
{
    /// <summary>
    /// Total event stats grouped by event type for the entire period.
    /// </summary>
    [JsonPropertyName("eventStatsTotal")]
    public List<ClassifiedEventStat>? EventStatsTotal { get; init; }

    /// <summary>
    /// Daily event stats grouped by date.
    /// </summary>
    [JsonPropertyName("eventsPerDay")]
    public List<ClassifiedDailyEventStatResponseDto>? EventsPerDay { get; init; }
}

/// <summary>
/// Response containing daily statistics for specific classified offer ads.
/// </summary>
public record OfferStatsResponseDto
{
    /// <summary>
    /// Statistics per offer.
    /// </summary>
    [JsonPropertyName("offerStats")]
    public List<OfferStatResponseDto>? OfferStats { get; init; }
}

/// <summary>
/// Statistics for a single classified offer.
/// </summary>
public record OfferStatResponseDto
{
    /// <summary>
    /// Offer reference.
    /// </summary>
    [JsonPropertyName("offer")]
    public OfferStatModelDto? Offer { get; init; }

    /// <summary>
    /// Total event stats grouped by event type for the entire period.
    /// </summary>
    [JsonPropertyName("eventStatsTotal")]
    public List<ClassifiedEventStat>? EventStatsTotal { get; init; }

    /// <summary>
    /// Daily event stats grouped by date.
    /// </summary>
    [JsonPropertyName("eventsPerDay")]
    public List<ClassifiedDailyEventStatResponseDto>? EventsPerDay { get; init; }
}

/// <summary>
/// Offer model containing offer identifier for statistics.
/// </summary>
public record OfferStatModelDto
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Classified event statistics by type.
/// </summary>
public record ClassifiedEventStat
{
    /// <summary>
    /// Number of occurrences of this event type.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Event type (e.g. SHOWED_PHONE_NUMBER, ASKED_QUESTION).
    /// </summary>
    [JsonPropertyName("eventType")]
    public string? EventType { get; init; }
}

/// <summary>
/// Classified event statistics for a single day.
/// </summary>
public record ClassifiedDailyEventStatResponseDto
{
    /// <summary>
    /// Date in format yyyy-MM-dd.
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// Event types with their occurrence counts for this day.
    /// </summary>
    [JsonPropertyName("eventStats")]
    public List<ClassifiedEventStat>? EventStats { get; init; }
}
