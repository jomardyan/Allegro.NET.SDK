using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Pricing;

public record FeePreviewRequest
{
    [JsonPropertyName("offer")]
    public PricingOffer? Offer { get; init; }

    [JsonPropertyName("classifiedsPackages")]
    public ClassifiedsPackages? ClassifiedsPackages { get; init; }

    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }
}

public record PricingOffer
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("category")]
    public PricingCategory? Category { get; init; }

    [JsonPropertyName("parameters")]
    public List<PricingParameter>? Parameters { get; init; }

    [JsonPropertyName("sellingMode")]
    public PricingSellingMode? SellingMode { get; init; }

    [JsonPropertyName("publication")]
    public PricingPublication? Publication { get; init; }

    [JsonPropertyName("promotion")]
    public PricingPromotion? Promotion { get; init; }

    [JsonPropertyName("fundraisingCampaign")]
    public FundraisingCampaignRef? FundraisingCampaign { get; init; }
}

public record PricingCategory
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record PricingParameter
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("valuesIds")]
    public List<string>? ValuesIds { get; init; }

    [JsonPropertyName("values")]
    public List<string>? Values { get; init; }

    [JsonPropertyName("rangeValue")]
    public PricingRangeValue? RangeValue { get; init; }
}

public record PricingRangeValue
{
    [JsonPropertyName("from")]
    public decimal? From { get; init; }

    [JsonPropertyName("to")]
    public decimal? To { get; init; }
}

public record PricingSellingMode
{
    [JsonPropertyName("format")]
    public string? Format { get; init; }

    [JsonPropertyName("price")]
    public Money? Price { get; init; }

    [JsonPropertyName("netPrice")]
    public Money? NetPrice { get; init; }

    [JsonPropertyName("minimalPrice")]
    public Money? MinimalPrice { get; init; }

    [JsonPropertyName("startingPrice")]
    public Money? StartingPrice { get; init; }
}

public record PricingPublication
{
    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("startingAt")]
    public DateTime? StartingAt { get; init; }

    [JsonPropertyName("endingAt")]
    public DateTime? EndingAt { get; init; }

    [JsonPropertyName("republish")]
    public bool? Republish { get; init; }
}

public record PricingPromotion
{
    [JsonPropertyName("emphasized")]
    public bool? Emphasized { get; init; }

    [JsonPropertyName("bold")]
    public bool? Bold { get; init; }

    [JsonPropertyName("highlight")]
    public bool? Highlight { get; init; }
}

public record FundraisingCampaignRef
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record ClassifiedsPackages
{
    [JsonPropertyName("basePackage")]
    public ClassifiedPackage? BasePackage { get; init; }

    [JsonPropertyName("extraPackages")]
    public List<ClassifiedExtraPackage>? ExtraPackages { get; init; }
}

public record ClassifiedPackage
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

public record ClassifiedExtraPackage
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("republish")]
    public bool? Republish { get; init; }
}

public record FeePreviewResponse
{
    [JsonPropertyName("commissions")]
    public List<CommissionResponse>? Commissions { get; init; }

    [JsonPropertyName("quotes")]
    public List<QuoteResponse>? Quotes { get; init; }
}

public record CommissionResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("fee")]
    public Money? Fee { get; init; }
}

public record QuoteResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("fee")]
    public Money? Fee { get; init; }

    [JsonPropertyName("cycleDuration")]
    public string? CycleDuration { get; init; }

    [JsonPropertyName("classifiedsPackage")]
    public ClassifiedPackage? ClassifiedsPackage { get; init; }
}
