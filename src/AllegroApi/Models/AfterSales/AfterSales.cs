using System.Text.Json.Serialization;

namespace AllegroApi.Models.AfterSales;

/// <summary>
/// List of return policies.
/// </summary>
public record ReturnPoliciesList
{
    /// <summary>
    /// Collection of return policies.
    /// </summary>
    [JsonPropertyName("returnPolicies")]
    public List<ReturnPolicyResponse>? ReturnPolicies { get; init; }
}

/// <summary>
/// Return policy details.
/// </summary>
public record ReturnPolicyResponse
{
    /// <summary>
    /// Return policy identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Return policy name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Period in days during which buyer can return the product.
    /// </summary>
    [JsonPropertyName("returnPeriod")]
    public int? ReturnPeriod { get; init; }

    /// <summary>
    /// Information about who covers return shipping costs.
    /// </summary>
    [JsonPropertyName("shippingCosts")]
    public string? ShippingCosts { get; init; }

    /// <summary>
    /// Additional description of the return policy.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Request for creating or updating a return policy.
/// </summary>
public record ReturnPolicyRequest
{
    /// <summary>
    /// Return policy name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Period in days during which buyer can return the product.
    /// </summary>
    [JsonPropertyName("returnPeriod")]
    public int? ReturnPeriod { get; init; }

    /// <summary>
    /// Information about who covers return shipping costs.
    /// </summary>
    [JsonPropertyName("shippingCosts")]
    public string? ShippingCosts { get; init; }

    /// <summary>
    /// Additional description of the return policy.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// List of warranties.
/// </summary>
public record WarrantiesList
{
    /// <summary>
    /// Collection of warranties.
    /// </summary>
    [JsonPropertyName("warranties")]
    public List<WarrantyResponse>? Warranties { get; init; }
}

/// <summary>
/// Warranty details.
/// </summary>
public record WarrantyResponse
{
    /// <summary>
    /// Warranty identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Warranty name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Warranty period in months.
    /// </summary>
    [JsonPropertyName("period")]
    public int? Period { get; init; }

    /// <summary>
    /// Warranty description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Attachment reference for warranty documentation.
    /// </summary>
    [JsonPropertyName("attachment")]
    public WarrantyAttachment? Attachment { get; init; }
}

/// <summary>
/// Request for creating or updating a warranty.
/// </summary>
public record WarrantyRequest
{
    /// <summary>
    /// Warranty name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Warranty period in months.
    /// </summary>
    [JsonPropertyName("period")]
    public int? Period { get; init; }

    /// <summary>
    /// Warranty description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Attachment reference for warranty documentation.
    /// </summary>
    [JsonPropertyName("attachment")]
    public WarrantyAttachment? Attachment { get; init; }
}

/// <summary>
/// Warranty attachment reference.
/// </summary>
public record WarrantyAttachment
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// List of implied warranties.
/// </summary>
public record ImpliedWarrantiesList
{
    /// <summary>
    /// Collection of implied warranties.
    /// </summary>
    [JsonPropertyName("impliedWarranties")]
    public List<ImpliedWarrantyResponse>? ImpliedWarranties { get; init; }
}

/// <summary>
/// Implied warranty details.
/// </summary>
public record ImpliedWarrantyResponse
{
    /// <summary>
    /// Implied warranty identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Implied warranty name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Implied warranty period in months.
    /// </summary>
    [JsonPropertyName("period")]
    public int? Period { get; init; }

    /// <summary>
    /// Implied warranty description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Request for creating or updating an implied warranty.
/// </summary>
public record ImpliedWarrantyRequest
{
    /// <summary>
    /// Implied warranty name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Implied warranty period in months.
    /// </summary>
    [JsonPropertyName("period")]
    public int? Period { get; init; }

    /// <summary>
    /// Implied warranty description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// After-sales service attachment response.
/// </summary>
public record AfterSalesAttachmentResponse
{
    /// <summary>
    /// Attachment identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Upload URL for the attachment (returned in Location header).
    /// </summary>
    [JsonPropertyName("uploadUrl")]
    public string? UploadUrl { get; init; }
}
