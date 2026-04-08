using System.Text.Json.Serialization;
using AllegroApi.Models.Common;

namespace AllegroApi.Models.Offers;

/// <summary>
/// Response from offer search
/// </summary>
public class OffersSearchResultDto
{
    [JsonPropertyName("offers")]
    public List<OfferListingDto> Offers { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

public class OfferListingDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public Category? Category { get; set; }

    [JsonPropertyName("primaryImage")]
    public Image? PrimaryImage { get; set; }

    [JsonPropertyName("sellingMode")]
    public SellingMode? SellingMode { get; set; }

    [JsonPropertyName("stock")]
    public Stock? Stock { get; set; }

    [JsonPropertyName("publication")]
    public Publication? Publication { get; set; }

    [JsonPropertyName("delivery")]
    public Delivery? Delivery { get; set; }

    [JsonPropertyName("external")]
    public External? External { get; set; }
}

/// <summary>
/// Query parameters for searching offers
/// </summary>
public class OfferSearchParams
{
    public List<string>? OfferIds { get; set; }
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? PriceAutomationRuleId { get; set; }
    public bool? PriceAutomationRuleIdEmpty { get; set; }
    public List<string>? PublicationStatus { get; set; }
    public string? PublicationMarketplace { get; set; }
    public List<string>? SellingModeFormat { get; set; }
    public List<string>? ExternalIds { get; set; }
    public string? DeliveryShippingRatesId { get; set; }
    public bool? DeliveryShippingRatesIdEmpty { get; set; }
    public string? Sort { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public string? CategoryId { get; set; }
    public bool? ProductIdEmpty { get; set; }
    public bool? ProductizationRequired { get; set; }
    public bool? B2bBuyableOnlyByBusiness { get; set; }
    public string? FundraisingCampaignId { get; set; }
    public bool? FundraisingCampaignIdEmpty { get; set; }
    public string? ReturnPolicyId { get; set; }

    public Dictionary<string, string> ToQueryParams()
    {
        var parameters = new Dictionary<string, string>();
        
        if (OfferIds != null && OfferIds.Any())
            foreach (var id in OfferIds) parameters.Add("offer.id", id);
        
        if (!string.IsNullOrEmpty(Name)) 
            parameters["name"] = Name;
        
        if (MinPrice.HasValue) 
            parameters["sellingMode.price.amount.gte"] = MinPrice.Value.ToString("F2");
        
        if (MaxPrice.HasValue) 
            parameters["sellingMode.price.amount.lte"] = MaxPrice.Value.ToString("F2");
        
        if (!string.IsNullOrEmpty(PriceAutomationRuleId))
            parameters["sellingMode.priceAutomation.rule.id"] = PriceAutomationRuleId;
        
        if (PriceAutomationRuleIdEmpty.HasValue)
            parameters["sellingMode.priceAutomation.rule.id.empty"] = PriceAutomationRuleIdEmpty.Value.ToString().ToLower();
        
        if (PublicationStatus != null && PublicationStatus.Any())
            foreach (var status in PublicationStatus) parameters.Add("publication.status", status);
        
        if (!string.IsNullOrEmpty(PublicationMarketplace))
            parameters["publication.marketplace"] = PublicationMarketplace;
        
        if (SellingModeFormat != null && SellingModeFormat.Any())
            foreach (var format in SellingModeFormat) parameters.Add("sellingMode.format", format);
        
        if (ExternalIds != null && ExternalIds.Any())
            foreach (var id in ExternalIds) parameters.Add("external.id", id);
        
        if (!string.IsNullOrEmpty(DeliveryShippingRatesId))
            parameters["delivery.shippingRates.id"] = DeliveryShippingRatesId;
        
        if (DeliveryShippingRatesIdEmpty.HasValue)
            parameters["delivery.shippingRates.id.empty"] = DeliveryShippingRatesIdEmpty.Value.ToString().ToLower();
        
        if (!string.IsNullOrEmpty(Sort))
            parameters["sort"] = Sort;
        
        if (Limit.HasValue)
            parameters["limit"] = Limit.Value.ToString();
        
        if (Offset.HasValue)
            parameters["offset"] = Offset.Value.ToString();
        
        if (!string.IsNullOrEmpty(CategoryId))
            parameters["category.id"] = CategoryId;
        
        if (ProductIdEmpty.HasValue)
            parameters["product.id.empty"] = ProductIdEmpty.Value.ToString().ToLower();
        
        if (ProductizationRequired.HasValue)
            parameters["productizationRequired"] = ProductizationRequired.Value.ToString().ToLower();
        
        if (B2bBuyableOnlyByBusiness.HasValue)
            parameters["b2b.buyableOnlyByBusiness"] = B2bBuyableOnlyByBusiness.Value.ToString().ToLower();
        
        if (!string.IsNullOrEmpty(FundraisingCampaignId))
            parameters["fundraisingCampaign.id"] = FundraisingCampaignId;
        
        if (FundraisingCampaignIdEmpty.HasValue)
            parameters["fundraisingCampaign.id.empty"] = FundraisingCampaignIdEmpty.Value.ToString().ToLower();
        
        if (!string.IsNullOrEmpty(ReturnPolicyId))
            parameters["afterSalesServices.returnPolicy.id"] = ReturnPolicyId;
        
        return parameters;
    }
}
