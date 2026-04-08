using System.Text.Json.Serialization;

namespace AllegroApi.Models.Categories;

public record CategoriesDto
{
    [JsonPropertyName("categories")]
    public List<CategoryDto>? Categories { get; init; }
}

public record CategoryDto
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("leaf")]
    public bool? Leaf { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("options")]
    public CategoryOptionsDto? Options { get; init; }

    [JsonPropertyName("parent")]
    public CategoryParent? Parent { get; init; }
}

public record CategoryParent
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record CategoryOptionsDto
{
    [JsonPropertyName("advertisement")]
    public bool? Advertisement { get; init; }

    [JsonPropertyName("advertisementPriceOptional")]
    public bool? AdvertisementPriceOptional { get; init; }

    [JsonPropertyName("variantsByColorPatternAllowed")]
    public bool? VariantsByColorPatternAllowed { get; init; }

    [JsonPropertyName("offersWithProductPublicationEnabled")]
    public bool? OffersWithProductPublicationEnabled { get; init; }

    [JsonPropertyName("productCreationEnabled")]
    public bool? ProductCreationEnabled { get; init; }

    [JsonPropertyName("customParametersEnabled")]
    public bool? CustomParametersEnabled { get; init; }

    [JsonPropertyName("sellerCanRequirePurchaseComments")]
    public bool? SellerCanRequirePurchaseComments { get; init; }
}

public record CategoryParameterList
{
    [JsonPropertyName("parameters")]
    public List<CategoryParameterDto>? Parameters { get; init; }
}

public record CategoryParameterDto
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("required")]
    public bool? Required { get; init; }

    [JsonPropertyName("requiredForProduct")]
    public bool? RequiredForProduct { get; init; }

    [JsonPropertyName("unit")]
    public string? Unit { get; init; }

    [JsonPropertyName("options")]
    public CategoryParameterOptions? Options { get; init; }

    [JsonPropertyName("restrictions")]
    public CategoryParameterRestrictions? Restrictions { get; init; }

    [JsonPropertyName("dictionary")]
    public List<CategoryParameterDictionaryValue>? Dictionary { get; init; }
}

public record CategoryParameterOptions
{
    [JsonPropertyName("variantsAllowed")]
    public bool? VariantsAllowed { get; init; }

    [JsonPropertyName("variantsEqual")]
    public bool? VariantsEqual { get; init; }

    [JsonPropertyName("ambiguousValueId")]
    public string? AmbiguousValueId { get; init; }

    [JsonPropertyName("dependsOnParameterId")]
    public string? DependsOnParameterId { get; init; }

    [JsonPropertyName("describesProduct")]
    public bool? DescribesProduct { get; init; }

    [JsonPropertyName("customValuesEnabled")]
    public bool? CustomValuesEnabled { get; init; }
}

public record CategoryParameterRestrictions
{
    [JsonPropertyName("min")]
    public decimal? Min { get; init; }

    [JsonPropertyName("max")]
    public decimal? Max { get; init; }

    [JsonPropertyName("range")]
    public bool? Range { get; init; }

    [JsonPropertyName("minLength")]
    public int? MinLength { get; init; }

    [JsonPropertyName("maxLength")]
    public int? MaxLength { get; init; }

    [JsonPropertyName("multipleOf")]
    public decimal? MultipleOf { get; init; }
}

public record CategoryParameterDictionaryValue
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }

    [JsonPropertyName("dependsOnValueIds")]
    public List<string>? DependsOnValueIds { get; init; }
}
