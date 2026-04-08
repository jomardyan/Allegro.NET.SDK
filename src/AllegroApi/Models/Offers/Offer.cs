using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Offers;

/// <summary>
/// Represents a complete offer in the Allegro marketplace
/// </summary>
public class Offer
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public Category? Category { get; set; }

    [JsonPropertyName("primaryImage")]
    public Image? PrimaryImage { get; set; }

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = new();

    [JsonPropertyName("description")]
    public Description? Description { get; set; }

    [JsonPropertyName("sellingMode")]
    public SellingMode? SellingMode { get; set; }

    [JsonPropertyName("stock")]
    public Stock? Stock { get; set; }

    [JsonPropertyName("publication")]
    public Publication? Publication { get; set; }

    [JsonPropertyName("delivery")]
    public Delivery? Delivery { get; set; }

    [JsonPropertyName("afterSalesServices")]
    public AfterSalesServices? AfterSalesServices { get; set; }

    [JsonPropertyName("parameters")]
    public List<Parameter> Parameters { get; set; } = new();

    [JsonPropertyName("external")]
    public External? External { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("product")]
    public ProductReference? Product { get; set; }

    [JsonPropertyName("b2b")]
    public B2B? B2B { get; set; }

    [JsonPropertyName("tax")]
    public Tax? Tax { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class Category
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Image
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class Description
{
    [JsonPropertyName("sections")]
    public List<DescriptionSection> Sections { get; set; } = new();
}

public class DescriptionSection
{
    [JsonPropertyName("items")]
    public List<DescriptionItem> Items { get; set; } = new();
}

public class DescriptionItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public class SellingMode
{
    [JsonPropertyName("format")]
    public string Format { get; set; } = "BUY_NOW";

    [JsonPropertyName("price")]
    public Money? Price { get; set; }

    [JsonPropertyName("netPrice")]
    public Money? NetPrice { get; set; }

    [JsonPropertyName("priceAutomation")]
    public PriceAutomation? PriceAutomation { get; set; }
}

public class PriceAutomation
{
    [JsonPropertyName("rule")]
    public PriceAutomationRule? Rule { get; set; }
}

public class PriceAutomationRule
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Stock
{
    [JsonPropertyName("available")]
    public int? Available { get; set; }

    [JsonPropertyName("sold")]
    public int? Sold { get; set; }

    [JsonPropertyName("unit")]
    public string? Unit { get; set; }
}

public class Publication
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "INACTIVE";

    [JsonPropertyName("startingAt")]
    public DateTime? StartingAt { get; set; }

    [JsonPropertyName("endingAt")]
    public DateTime? EndingAt { get; set; }

    [JsonPropertyName("republish")]
    public bool? Republish { get; set; }

    [JsonPropertyName("marketplace")]
    public string? Marketplace { get; set; }

    [JsonPropertyName("additionalMarketplaces")]
    public List<string>? AdditionalMarketplaces { get; set; }
}

public class Delivery
{
    [JsonPropertyName("shippingRates")]
    public ShippingRates? ShippingRates { get; set; }

    [JsonPropertyName("handlingTime")]
    public string? HandlingTime { get; set; }
}

public class ShippingRates
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class AfterSalesServices
{
    [JsonPropertyName("impliedWarranty")]
    public ImpliedWarranty? ImpliedWarranty { get; set; }

    [JsonPropertyName("returnPolicy")]
    public ReturnPolicy? ReturnPolicy { get; set; }

    [JsonPropertyName("warranty")]
    public Warranty? Warranty { get; set; }
}

public class ImpliedWarranty
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class ReturnPolicy
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Warranty
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Parameter
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("values")]
    public List<string> Values { get; set; } = new();

    [JsonPropertyName("valuesIds")]
    public List<string>? ValuesIds { get; set; }

    [JsonPropertyName("valuesLabels")]
    public List<string>? ValuesLabels { get; set; }

    [JsonPropertyName("rangeValue")]
    public ParameterRangeValue? RangeValue { get; set; }
}

public class ParameterRangeValue
{
    [JsonPropertyName("from")]
    public decimal? From { get; set; }

    [JsonPropertyName("to")]
    public decimal? To { get; set; }
}

public class External
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class Location
{
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("postCode")]
    public string? PostCode { get; set; }

    [JsonPropertyName("province")]
    public string? Province { get; set; }
}

public class ProductReference
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class B2B
{
    [JsonPropertyName("buyableOnlyByBusiness")]
    public bool BuyableOnlyByBusiness { get; set; }
}

public class Tax
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }
}
