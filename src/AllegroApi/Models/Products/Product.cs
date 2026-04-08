using System.Text.Json.Serialization;

namespace AllegroApi.Models.Products;

/// <summary>
/// Response from product search
/// </summary>
public class ProductsSearchResult
{
    [JsonPropertyName("products")]
    public List<Product> Products { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

/// <summary>
/// Product information
/// </summary>
public class Product
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public ProductCategory? Category { get; set; }

    [JsonPropertyName("images")]
    public List<ProductImage> Images { get; set; } = new();

    [JsonPropertyName("parameters")]
    public List<ProductParameter> Parameters { get; set; } = new();

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("compatibility")]
    public List<CompatibilityItem>? Compatibility { get; set; }

    /// <summary>
    /// Offer requirements reference.
    /// </summary>
    [JsonPropertyName("offerRequirements")]
    public ProductOfferRequirements? OfferRequirements { get; set; }

    /// <summary>
    /// TecDoc specification reference.
    /// </summary>
    [JsonPropertyName("tecdocSpecification")]
    public string? TecdocSpecification { get; set; }

    /// <summary>
    /// AI co-created content indicator.
    /// </summary>
    [JsonPropertyName("aiCoCreatedContent")]
    public bool? AiCoCreatedContent { get; set; }

    /// <summary>
    /// Trusted content indicator.
    /// </summary>
    [JsonPropertyName("trustedContent")]
    public bool? TrustedContent { get; set; }

    /// <summary>
    /// Protected brand flag.
    /// </summary>
    [JsonPropertyName("hasProtectedBrand")]
    public bool? HasProtectedBrand { get; set; }

    /// <summary>
    /// Publication status information.
    /// </summary>
    [JsonPropertyName("publication")]
    public ProductPublication? Publication { get; set; }

    /// <summary>
    /// Product safety information for EU GPSR compliance.
    /// </summary>
    [JsonPropertyName("productSafety")]
    public ProductSafety? ProductSafety { get; set; }
}

public class ProductCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class ProductImage
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class ProductParameter
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("values")]
    public List<string> Values { get; set; } = new();

    [JsonPropertyName("valuesIds")]
    public List<string>? ValuesIds { get; set; }
}

public class CompatibilityItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Query parameters for product search
/// </summary>
public class ProductSearchParams
{
    public string? Ean { get; set; }
    public string? Phrase { get; set; }
    public string? Mode { get; set; } // "GTIN", "MPN"
    public string? Language { get; set; }
    public string? CategoryId { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }

    public Dictionary<string, string> ToQueryParams()
    {
        var parameters = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(Ean))
            parameters["ean"] = Ean;
        
        if (!string.IsNullOrEmpty(Phrase))
            parameters["phrase"] = Phrase;
        
        if (!string.IsNullOrEmpty(Mode))
            parameters["mode"] = Mode;
        
        if (!string.IsNullOrEmpty(Language))
            parameters["language"] = Language;
        
        if (!string.IsNullOrEmpty(CategoryId))
            parameters["category.id"] = CategoryId;
        
        if (Limit.HasValue)
            parameters["limit"] = Limit.Value.ToString();
        
        if (Offset.HasValue)
            parameters["offset"] = Offset.Value.ToString();
        
        return parameters;
    }
}

/// <summary>
/// Request to propose changes to a product.
/// </summary>
public record ProductChangeProposalRequest
{
    /// <summary>
    /// Product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Product description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Product parameters to change.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<ProductParameter>? Parameters { get; init; }

    /// <summary>
    /// Product images.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ProductImage>? Images { get; init; }
}

/// <summary>
/// Product change proposal response.
/// </summary>
public record ProductChangeProposalDto
{
    /// <summary>
    /// Proposal identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Product identifier.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; init; }

    /// <summary>
    /// Proposal status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Proposed changes.
    /// </summary>
    [JsonPropertyName("changes")]
    public ProductChangeProposalRequest? Changes { get; init; }

    /// <summary>
    /// When the proposal was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }
}

/// <summary>
/// Product offer requirements.
/// </summary>
public class ProductOfferRequirements
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

/// <summary>
/// Product publication status.
/// </summary>
public class ProductPublication
{
    /// <summary>
    /// Publication status: PROPOSED or LISTED.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

/// <summary>
/// Product safety information for EU GPSR compliance.
/// </summary>
public class ProductSafety
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("documents")]
    public List<ProductSafetyDocument>? Documents { get; set; }
}

/// <summary>
/// Product safety document.
/// </summary>
public class ProductSafetyDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

/// <summary>
/// Request to propose a new product.
/// Used when creating a new product in Allegro catalog.
/// </summary>
public record ProductProposalsRequest
{
    /// <summary>
    /// Suggested product name (max 75 characters).
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Product category.
    /// </summary>
    [JsonPropertyName("category")]
    public ProductCategory Category { get; init; } = null!;

    /// <summary>
    /// List of product images. At least one image is required.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageUrl> Images { get; init; } = new();

    /// <summary>
    /// List of product parameters.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<ProductParameter> Parameters { get; init; } = new();

    /// <summary>
    /// Product description.
    /// </summary>
    [JsonPropertyName("description")]
    public StandardizedDescription? Description { get; init; }

    /// <summary>
    /// Language of provided product data (name, description, parameters values).
    /// Format: BCP-47 language code (e.g., pl-PL, en-US).
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; init; } = string.Empty;
}

/// <summary>
/// Response from product proposal request.
/// Contains the created product details.
/// </summary>
public record ProductProposalsResponse
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Product category with similar categories.
    /// </summary>
    [JsonPropertyName("category")]
    public ProductCategoryWithSimilar? Category { get; init; }

    /// <summary>
    /// List of product images.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageUrl>? Images { get; init; }

    /// <summary>
    /// List of product parameters.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<NewProductParameterDto>? Parameters { get; init; }

    /// <summary>
    /// Product description.
    /// </summary>
    [JsonPropertyName("description")]
    public StandardizedDescription? Description { get; init; }

    /// <summary>
    /// Supplier information.
    /// </summary>
    [JsonPropertyName("supplier")]
    public SupplierDto? Supplier { get; init; }

    /// <summary>
    /// Language of product data (name, description, parameters values).
    /// Format: BCP-47 language code.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }

    /// <summary>
    /// Publication status.
    /// </summary>
    [JsonPropertyName("publication")]
    public ProductPublicationStatus? Publication { get; init; }
}

/// <summary>
/// Product category with similar categories.
/// </summary>
public class ProductCategoryWithSimilar
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// A list of similar categories in which you can sell this product.
    /// </summary>
    [JsonPropertyName("similar")]
    public List<CategoryInfo>? Similar { get; set; }
}

/// <summary>
/// Category information.
/// </summary>
public class CategoryInfo
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Parent category identifier.
    /// </summary>
    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    /// <summary>
    /// Indicates if the category is a leaf (has no subcategories).
    /// </summary>
    [JsonPropertyName("leaf")]
    public bool? Leaf { get; set; }
}

/// <summary>
/// Image URL reference.
/// </summary>
public class ImageUrl
{
    /// <summary>
    /// Image URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

/// <summary>
/// New product parameter DTO.
/// </summary>
public class NewProductParameterDto
{
    /// <summary>
    /// Parameter identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Parameter name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Parameter values.
    /// </summary>
    [JsonPropertyName("values")]
    public List<string>? Values { get; set; }

    /// <summary>
    /// Parameter value identifiers.
    /// </summary>
    [JsonPropertyName("valuesIds")]
    public List<string>? ValuesIds { get; set; }
}

/// <summary>
/// Standardized product description.
/// </summary>
public class StandardizedDescription
{
    /// <summary>
    /// Description sections.
    /// </summary>
    [JsonPropertyName("sections")]
    public List<DescriptionSection>? Sections { get; set; }
}

/// <summary>
/// Description section.
/// </summary>
public class DescriptionSection
{
    /// <summary>
    /// Section items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<DescriptionItem>? Items { get; set; }
}

/// <summary>
/// Description item.
/// </summary>
public class DescriptionItem
{
    /// <summary>
    /// Item type (e.g., TEXT, IMAGE).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Item content.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

/// <summary>
/// Supplier information.
/// </summary>
public class SupplierDto
{
    /// <summary>
    /// Supplier identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Supplier name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

/// <summary>
/// Product publication status.
/// </summary>
public class ProductPublicationStatus
{
    /// <summary>
    /// Publication status (e.g., PROPOSED, LISTED).
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
