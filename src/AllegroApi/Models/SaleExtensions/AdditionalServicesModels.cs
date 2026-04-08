using System.Text.Json.Serialization;

namespace AllegroApi.Models.SaleExtensions;

/// <summary>
/// Represents an additional services group response.
/// </summary>
public record AdditionalServicesGroupResponse
{
    /// <summary>
    /// Gets the additional services group ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the name of the group provided by merchant (invisible for buyers).
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the IETF language tag.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }

    /// <summary>
    /// Gets whether the group is automatically created and managed by Allegro.
    /// </summary>
    [JsonPropertyName("managedByAllegro")]
    public bool ManagedByAllegro { get; init; }

    /// <summary>
    /// Gets the list of additional services in this group.
    /// </summary>
    [JsonPropertyName("additionalServices")]
    public List<AdditionalServiceResponse> AdditionalServices { get; init; } = new();

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Gets the last update timestamp.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Gets the seller reference.
    /// </summary>
    [JsonPropertyName("seller")]
    public Reference? Seller { get; init; }
}

/// <summary>
/// Represents a reference to an entity.
/// </summary>
public record Reference
{
    /// <summary>
    /// Gets the entity ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Represents an additional service response.
/// </summary>
public record AdditionalServiceResponse
{
    /// <summary>
    /// Gets the service definition.
    /// </summary>
    [JsonPropertyName("definition")]
    public BasicDefinitionResponse? Definition { get; init; }

    /// <summary>
    /// Gets the description provided by merchant while configuring the service.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets the list of service configurations.
    /// </summary>
    [JsonPropertyName("configurations")]
    public List<Configuration>? Configurations { get; init; }
}

/// <summary>
/// Represents a basic definition response.
/// </summary>
public record BasicDefinitionResponse
{
    /// <summary>
    /// Gets the definition ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the definition name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Represents a service configuration.
/// </summary>
public record Configuration
{
    /// <summary>
    /// Gets the marketplace ID.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Gets the price for this configuration.
    /// </summary>
    [JsonPropertyName("price")]
    public Price? Price { get; init; }

    /// <summary>
    /// Gets the constraints for this configuration.
    /// </summary>
    [JsonPropertyName("constraints")]
    public Constraints? Constraints { get; init; }
}

/// <summary>
/// Represents price constraints.
/// </summary>
public record Constraints
{
    /// <summary>
    /// Gets the minimum price.
    /// </summary>
    [JsonPropertyName("min")]
    public Price? Min { get; init; }

    /// <summary>
    /// Gets the maximum price.
    /// </summary>
    [JsonPropertyName("max")]
    public Price? Max { get; init; }
}

/// <summary>
/// Represents a price.
/// </summary>
public record Price
{
    /// <summary>
    /// Gets the amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Gets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Represents a list of additional services groups.
/// </summary>
public record AdditionalServicesGroups
{
    /// <summary>
    /// Gets the list of additional services groups.
    /// </summary>
    [JsonPropertyName("additionalServicesGroups")]
    public List<AdditionalServicesGroupResponse> Groups { get; init; } = new();
}

/// <summary>
/// Represents a request to create an additional services group.
/// </summary>
public record AdditionalServicesGroupRequest
{
    /// <summary>
    /// Gets or sets the name of the group (invisible for buyers).
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the IETF language tag (must match marketplace: pl-PL for allegro.pl, cs-CZ for allegro.cz).
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of additional services.
    /// </summary>
    [JsonPropertyName("additionalServices")]
    public List<AdditionalServiceRequest> AdditionalServices { get; init; } = new();
}

/// <summary>
/// Represents an additional service request.
/// </summary>
public record AdditionalServiceRequest
{
    /// <summary>
    /// Gets or sets the service definition.
    /// </summary>
    [JsonPropertyName("definition")]
    public AdditionalServiceDefinitionRequest Definition { get; init; } = new();

    /// <summary>
    /// Gets or sets the service description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the service configurations.
    /// </summary>
    [JsonPropertyName("configurations")]
    public List<Configuration>? Configurations { get; init; }
}

/// <summary>
/// Represents an additional service definition request.
/// </summary>
public record AdditionalServiceDefinitionRequest
{
    /// <summary>
    /// Gets or sets the definition ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}

/// <summary>
/// Represents a response with categories of additional services.
/// </summary>
public record CategoriesResponse
{
    /// <summary>
    /// Gets the list of categories.
    /// </summary>
    [JsonPropertyName("categories")]
    public List<CategoryResponse> Categories { get; init; } = new();
}

/// <summary>
/// Represents a category of additional services.
/// </summary>
public record CategoryResponse
{
    /// <summary>
    /// Gets the name of the additional services category.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of definitions in this category.
    /// </summary>
    [JsonPropertyName("definitions")]
    public List<CategoryDefinitionResponse> Definitions { get; init; } = new();
}

/// <summary>
/// Represents a category definition response.
/// </summary>
public record CategoryDefinitionResponse
{
    /// <summary>
    /// Gets the additional service definition ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the name that should be shown to the customer.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the description information.
    /// </summary>
    [JsonPropertyName("description")]
    public CategoryDefinitionDescriptionResponse? Description { get; init; }

    /// <summary>
    /// Gets the maximum price for this service.
    /// </summary>
    [JsonPropertyName("maxPrice")]
    public Price? MaxPrice { get; init; }

    /// <summary>
    /// Gets the available constraints.
    /// </summary>
    [JsonPropertyName("availableConstraints")]
    public List<AvailableConstraint>? AvailableConstraints { get; init; }

    /// <summary>
    /// Gets the last update timestamp.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Represents description information for a category definition.
/// </summary>
public record CategoryDefinitionDescriptionResponse
{
    /// <summary>
    /// Gets the hint for better description.
    /// </summary>
    [JsonPropertyName("hint")]
    public string? Hint { get; init; }

    /// <summary>
    /// Gets whether the description can be set by the seller.
    /// </summary>
    [JsonPropertyName("editable")]
    public bool Editable { get; init; }

    /// <summary>
    /// Gets the default description provided by Allegro.
    /// </summary>
    [JsonPropertyName("default")]
    public string? Default { get; init; }
}

/// <summary>
/// Represents an available constraint.
/// </summary>
public record AvailableConstraint
{
    /// <summary>
    /// Gets the constraint type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>
/// Represents a translation response for an additional service group.
/// </summary>
public record AdditionalServiceGroupTranslationResponse
{
    /// <summary>
    /// Gets the list of translations.
    /// </summary>
    [JsonPropertyName("translations")]
    public List<AdditionalServiceGroupTranslation> Translations { get; init; } = new();
}

/// <summary>
/// Represents a single translation for an additional service group.
/// </summary>
public record AdditionalServiceGroupTranslation
{
    /// <summary>
    /// Gets the IETF language tag.
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; init; } = string.Empty;

    /// <summary>
    /// Gets the translated additional services.
    /// </summary>
    [JsonPropertyName("additionalServices")]
    public AdditionalServicesGroupTranslationWrapperWithType? AdditionalServices { get; init; }
}

/// <summary>
/// Represents a wrapper for translations with type.
/// </summary>
public record AdditionalServicesGroupTranslationWrapperWithType
{
    /// <summary>
    /// Gets the translation type (MANUAL or AUTO).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the list of service translations.
    /// </summary>
    [JsonPropertyName("translation")]
    public List<AdditionalServiceTranslation>? Translation { get; init; }
}

/// <summary>
/// Represents a translation for an additional service.
/// </summary>
public record AdditionalServiceTranslation
{
    /// <summary>
    /// Gets the service definition.
    /// </summary>
    [JsonPropertyName("definition")]
    public AdditionalServiceDefinitionRequest? Definition { get; init; }

    /// <summary>
    /// Gets the translated description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
/// Represents a request to create/update translations for an additional service group.
/// </summary>
public record AdditionalServicesGroupTranslationRequest
{
    /// <summary>
    /// Gets or sets the additional services wrapper.
    /// </summary>
    [JsonPropertyName("additionalServices")]
    public AdditionalServicesGroupTranslationWrapper AdditionalServices { get; init; } = new();
}

/// <summary>
/// Represents a wrapper for translation requests.
/// </summary>
public record AdditionalServicesGroupTranslationWrapper
{
    /// <summary>
    /// Gets or sets the list of service translations.
    /// </summary>
    [JsonPropertyName("translation")]
    public List<AdditionalServiceTranslation> Translation { get; init; } = new();
}

/// <summary>
/// Represents a response after patching a translation.
/// </summary>
public record AdditionalServiceGroupTranslationPatchResponse
{
    /// <summary>
    /// Gets the IETF language tag.
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; init; } = string.Empty;

    /// <summary>
    /// Gets the translated additional services.
    /// </summary>
    [JsonPropertyName("additionalServices")]
    public AdditionalServicesGroupTranslationWrapperWithType? AdditionalServices { get; init; }
}
