using System.Text.Json.Serialization;

namespace AllegroApi.Models.AdvancedOffers;

/// <summary>
/// Offer variant set details.
/// </summary>
public record OfferVariantSet
{
    /// <summary>
    /// Variant set identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Variant set name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// List of offers in the variant set.
    /// </summary>
    [JsonPropertyName("offers")]
    public List<VariantOffer>? Offers { get; init; }

    /// <summary>
    /// Variant parameters (e.g., color, size).
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<VariantParameter>? Parameters { get; init; }
}

/// <summary>
/// Offer variant details.
/// </summary>
public record VariantOffer
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Variant parameter values.
    /// </summary>
    [JsonPropertyName("parameterValues")]
    public Dictionary<string, string>? ParameterValues { get; init; }
}

/// <summary>
/// Variant parameter definition.
/// </summary>
public record VariantParameter
{
    /// <summary>
    /// Parameter identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Parameter name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Available values for this parameter.
    /// </summary>
    [JsonPropertyName("values")]
    public List<string>? Values { get; init; }
}

/// <summary>
/// List of offer attachments.
/// </summary>
public record OfferAttachmentsList
{
    /// <summary>
    /// Collection of attachments.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<OfferAttachment>? Attachments { get; init; }
}

/// <summary>
/// Offer attachment details (additional files like manuals, certificates).
/// </summary>
public record OfferAttachment
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Attachment filename.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    /// <summary>
    /// Attachment type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Attachment URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// Attachment description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Request for creating an offer attachment.
/// </summary>
public record CreateOfferAttachmentRequest
{
    /// <summary>
    /// Attachment filename.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    /// <summary>
    /// Attachment type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Attachment description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Smart offer configuration (automatic pricing, stock management).
/// </summary>
public record SmartOfferConfig
{
    /// <summary>
    /// Offer identifier.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Indicates if smart features are enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Automatic pricing configuration.
    /// </summary>
    [JsonPropertyName("autoPricing")]
    public AutoPricingConfig? AutoPricing { get; init; }

    /// <summary>
    /// Automatic stock management configuration.
    /// </summary>
    [JsonPropertyName("autoStock")]
    public AutoStockConfig? AutoStock { get; init; }
}

/// <summary>
/// Automatic pricing configuration.
/// </summary>
public record AutoPricingConfig
{
    /// <summary>
    /// Indicates if automatic pricing is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Pricing strategy (e.g., "COMPETITIVE", "MARKET_LEADER").
    /// </summary>
    [JsonPropertyName("strategy")]
    public string? Strategy { get; init; }

    /// <summary>
    /// Minimum price threshold.
    /// </summary>
    [JsonPropertyName("minPrice")]
    public decimal? MinPrice { get; init; }

    /// <summary>
    /// Maximum price threshold.
    /// </summary>
    [JsonPropertyName("maxPrice")]
    public decimal? MaxPrice { get; init; }
}

/// <summary>
/// Automatic stock management configuration.
/// </summary>
public record AutoStockConfig
{
    /// <summary>
    /// Indicates if automatic stock management is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Minimum stock level threshold.
    /// </summary>
    [JsonPropertyName("minStock")]
    public int? MinStock { get; init; }

    /// <summary>
    /// Maximum stock level threshold.
    /// </summary>
    [JsonPropertyName("maxStock")]
    public int? MaxStock { get; init; }
}

/// <summary>
/// Request for creating an offer attachment (standalone flow).
/// </summary>
public record OfferAttachmentCreateRequest
{
    /// <summary>
    /// Gets or sets the attachment type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the file information.
    /// </summary>
    [JsonPropertyName("file")]
    public AttachmentFileRequest? File { get; init; }
}

/// <summary>
/// Attachment file request.
/// </summary>
public record AttachmentFileRequest
{
    /// <summary>
    /// Gets or sets the filename (max 200 characters).
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

/// <summary>
/// Attachment file information.
/// </summary>
public record AttachmentFile
{
    /// <summary>
    /// Gets the filename.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the file URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

/// <summary>
/// Offer attachment response from standalone creation endpoint.
/// </summary>
public record OfferAttachmentResponse
{
    /// <summary>
    /// Gets the attachment ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the attachment type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the file information.
    /// </summary>
    [JsonPropertyName("file")]
    public AttachmentFile? File { get; init; }
}
