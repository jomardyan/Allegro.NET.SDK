using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Offers;

/// <summary>
/// Request to create a product offer
/// </summary>
public class SaleProductOfferRequestV1
{
    [JsonPropertyName("productSet")]
    public List<ProductSet> ProductSet { get; set; } = new();

    [JsonPropertyName("parameters")]
    public List<Parameter>? Parameters { get; set; }

    [JsonPropertyName("sellingMode")]
    public SellingMode SellingMode { get; set; } = new();

    [JsonPropertyName("stock")]
    public Stock Stock { get; set; } = new();

    [JsonPropertyName("publication")]
    public Publication? Publication { get; set; }

    [JsonPropertyName("delivery")]
    public Delivery? Delivery { get; set; }

    [JsonPropertyName("afterSalesServices")]
    public AfterSalesServices? AfterSalesServices { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("external")]
    public External? External { get; set; }

    [JsonPropertyName("b2b")]
    public B2B? B2B { get; set; }

    [JsonPropertyName("tax")]
    public Tax? Tax { get; set; }

    [JsonPropertyName("sizeTable")]
    public SizeTable? SizeTable { get; set; }

    [JsonPropertyName("discounts")]
    public Discounts? Discounts { get; set; }

    [JsonPropertyName("fundraisingCampaign")]
    public FundraisingCampaign? FundraisingCampaign { get; set; }
}

/// <summary>
/// Request to patch/edit a product offer
/// </summary>
public class SaleProductOfferPatchRequestV1
{
    [JsonPropertyName("category")]
    public Category? Category { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("parameters")]
    public List<Parameter>? Parameters { get; set; }

    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }

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

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("external")]
    public External? External { get; set; }

    [JsonPropertyName("b2b")]
    public B2B? B2B { get; set; }

    [JsonPropertyName("tax")]
    public Tax? Tax { get; set; }

    [JsonPropertyName("sizeTable")]
    public SizeTable? SizeTable { get; set; }

    [JsonPropertyName("productSet")]
    public List<ProductSet>? ProductSet { get; set; }

    [JsonPropertyName("discounts")]
    public Discounts? Discounts { get; set; }

    [JsonPropertyName("fundraisingCampaign")]
    public FundraisingCampaign? FundraisingCampaign { get; set; }
}

/// <summary>
/// Response from creating or editing a product offer
/// </summary>
public class SaleProductOfferResponseV1
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("validation")]
    public Validation? Validation { get; set; }
}

public class ProductSet
{
    [JsonPropertyName("product")]
    public ProductIdentifier? Product { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;
}

public class ProductIdentifier
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("ean")]
    public List<string>? Ean { get; set; }

    [JsonPropertyName("mpart")]
    public List<string>? Mpart { get; set; }
}

public class SizeTable
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Discounts
{
    [JsonPropertyName("wholesalePriceLists")]
    public List<WholesalePriceList>? WholesalePriceLists { get; set; }
}

public class WholesalePriceList
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class FundraisingCampaign
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Validation
{
    [JsonPropertyName("errors")]
    public List<ValidationError> Errors { get; set; } = new();

    [JsonPropertyName("warnings")]
    public List<ValidationWarning> Warnings { get; set; } = new();
}

public class ValidationError
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("userMessage")]
    public string? UserMessage { get; set; }
}

public class ValidationWarning
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string? Path { get; set; }
}

/// <summary>
/// Status response for async operations
/// </summary>
public class SaleProductOfferStatusResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("errors")]
    public List<Error> Errors { get; set; } = new();
}

/// <summary>
/// Partial product offer response
/// </summary>
public class SalePartialProductOfferResponse
{
    [JsonPropertyName("stock")]
    public Stock? Stock { get; set; }

    [JsonPropertyName("price")]
    public Money? Price { get; set; }
}
