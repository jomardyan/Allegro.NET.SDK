using System.Text.Json.Serialization;

namespace AllegroApi.Models.Shipping;

public record ShippingRatesListResponse
{
    [JsonPropertyName("shippingRates")]
    public List<ShippingRateSummary>? ShippingRates { get; init; }
}

public record ShippingRateSummary
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("features")]
    public ShippingRateFeatures? Features { get; init; }

    [JsonPropertyName("marketplaces")]
    public List<MarketplaceReference>? Marketplaces { get; init; }
}

public record ShippingRateFeatures
{
    [JsonPropertyName("managedByAllegro")]
    public bool? ManagedByAllegro { get; init; }
}

public record MarketplaceReference
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record ShippingRatesSet
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("rates")]
    public List<ShippingRate>? Rates { get; init; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; init; }
}

public record ShippingRate
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("deliveryMethod")]
    public ShippingDeliveryMethod? DeliveryMethod { get; init; }

    [JsonPropertyName("rates")]
    public List<ShippingRateOption>? Rates { get; init; }
}

public record ShippingDeliveryMethod
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

public record ShippingRateOption
{
    [JsonPropertyName("maxWeight")]
    public ShippingWeight? MaxWeight { get; init; }

    [JsonPropertyName("price")]
    public ShippingPrice? Price { get; init; }
}

public record ShippingWeight
{
    [JsonPropertyName("value")]
    public decimal? Value { get; init; }

    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

public record ShippingPrice
{
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

public record DeliverySettingsResponse
{
    [JsonPropertyName("marketplace")]
    public DeliveryMarketplace? Marketplace { get; init; }

    [JsonPropertyName("freeDelivery")]
    public DeliveryThreshold? FreeDelivery { get; init; }

    [JsonPropertyName("abroadFreeDelivery")]
    public DeliveryThreshold? AbroadFreeDelivery { get; init; }

    [JsonPropertyName("joinPolicy")]
    public DeliveryJoinPolicy? JoinPolicy { get; init; }

    [JsonPropertyName("customCost")]
    public DeliveryCustomCost? CustomCost { get; init; }
}

public record DeliveryMarketplace
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record DeliveryThreshold
{
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

public record DeliveryJoinPolicy
{
    [JsonPropertyName("strategy")]
    public string? Strategy { get; init; }
}

public record DeliveryCustomCost
{
    [JsonPropertyName("allowed")]
    public bool? Allowed { get; init; }
}

public record DeliveryMethodsResponse
{
    [JsonPropertyName("deliveryMethods")]
    public List<DeliveryMethod>? DeliveryMethods { get; init; }
}

public record DeliveryMethod
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("paymentPolicy")]
    public string? PaymentPolicy { get; init; }

    [JsonPropertyName("shippingTime")]
    public ShippingTime? ShippingTime { get; init; }
}

public record ShippingTime
{
    [JsonPropertyName("from")]
    public int? From { get; init; }

    [JsonPropertyName("to")]
    public int? To { get; init; }
}
