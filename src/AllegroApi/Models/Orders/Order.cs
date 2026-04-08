using System.Text.Json.Serialization;

namespace AllegroApi.Models.Orders;

/// <summary>
/// Order information
/// </summary>
public class Order
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("messageToSeller")]
    public string? MessageToSeller { get; set; }

    [JsonPropertyName("buyer")]
    public Buyer? Buyer { get; set; }

    [JsonPropertyName("payment")]
    public Payment? Payment { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("fulfillment")]
    public Fulfillment? Fulfillment { get; set; }

    [JsonPropertyName("delivery")]
    public OrderDelivery? Delivery { get; set; }

    [JsonPropertyName("invoice")]
    public Invoice? Invoice { get; set; }

    [JsonPropertyName("lineItems")]
    public List<LineItem> LineItems { get; set; } = new();

    [JsonPropertyName("surcharges")]
    public List<Surcharge> Surcharges { get; set; } = new();

    [JsonPropertyName("discounts")]
    public List<Discount> Discounts { get; set; } = new();

    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonPropertyName("marketplace")]
    public OrderMarketplace? Marketplace { get; set; }

    [JsonPropertyName("summary")]
    public OrderSummary? Summary { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Checkout form revision.
    /// </summary>
    [JsonPropertyName("revision")]
    public string? Revision { get; set; }
}

public class Buyer
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("guest")]
    public bool Guest { get; set; }

    [JsonPropertyName("personalIdentity")]
    public string? PersonalIdentity { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("preferences")]
    public BuyerPreferences? Preferences { get; set; }

    [JsonPropertyName("address")]
    public Address? Address { get; set; }
}

public class BuyerPreferences
{
    [JsonPropertyName("language")]
    public string? Language { get; set; }
}

public class Address
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("postCode")]
    public string? PostCode { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}

public class Payment
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("provider")]
    public string? Provider { get; set; }

    [JsonPropertyName("finishedAt")]
    public DateTime? FinishedAt { get; set; }

    [JsonPropertyName("paidAmount")]
    public Common.Money? PaidAmount { get; set; }

    [JsonPropertyName("reconciliation")]
    public Reconciliation? Reconciliation { get; set; }

    /// <summary>
    /// Payment additional features (e.g., "ALLEGRO_PAY").
    /// </summary>
    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }
}

public class Reconciliation
{
    [JsonPropertyName("amount")]
    public Common.Money? Amount { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class Fulfillment
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("shipmentSummary")]
    public ShipmentSummary? ShipmentSummary { get; set; }
}

public class ShipmentSummary
{
    [JsonPropertyName("lineItemsSent")]
    public int LineItemsSent { get; set; }
}

public class OrderDelivery
{
    [JsonPropertyName("address")]
    public Address? Address { get; set; }

    [JsonPropertyName("method")]
    public DeliveryMethod? Method { get; set; }

    [JsonPropertyName("pickupPoint")]
    public PickupPoint? PickupPoint { get; set; }

    [JsonPropertyName("cost")]
    public Common.Money? Cost { get; set; }

    [JsonPropertyName("time")]
    public DeliveryTime? Time { get; set; }

    [JsonPropertyName("smart")]
    public bool Smart { get; set; }

    [JsonPropertyName("calculatedNumberOfPackages")]
    public int? CalculatedNumberOfPackages { get; set; }
}

public class DeliveryMethod
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class PickupPoint
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("address")]
    public Address? Address { get; set; }
}

public class DeliveryTime
{
    [JsonPropertyName("from")]
    public DateTime? From { get; set; }

    [JsonPropertyName("to")]
    public DateTime? To { get; set; }

    [JsonPropertyName("guaranteed")]
    public DeliveryGuaranteed? Guaranteed { get; set; }

    [JsonPropertyName("dispatch")]
    public DeliveryDispatch? Dispatch { get; set; }
}

public class DeliveryGuaranteed
{
    [JsonPropertyName("to")]
    public DateTime? To { get; set; }
}

public class DeliveryDispatch
{
    [JsonPropertyName("from")]
    public DateTime? From { get; set; }

    [JsonPropertyName("to")]
    public DateTime? To { get; set; }
}

public class Invoice
{
    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("address")]
    public Address? Address { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("naturalPerson")]
    public bool NaturalPerson { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("taxId")]
    public string? TaxId { get; set; }
}

public class LineItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("offer")]
    public LineItemOffer? Offer { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("originalPrice")]
    public Common.Money? OriginalPrice { get; set; }

    [JsonPropertyName("price")]
    public Common.Money? Price { get; set; }

    [JsonPropertyName("reconciliation")]
    public Reconciliation? Reconciliation { get; set; }

    [JsonPropertyName("selectedAdditionalServices")]
    public List<AdditionalService>? SelectedAdditionalServices { get; set; }

    [JsonPropertyName("boughtAt")]
    public DateTime? BoughtAt { get; set; }
}

public class LineItemOffer
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("external")]
    public External? External { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}

public class External
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class AdditionalService
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public Common.Money? Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}

public class Surcharge
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public Common.Money? Amount { get; set; }
}

public class Discount
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public Common.Money? Amount { get; set; }
}

public class OrderMarketplace
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class OrderSummary
{
    [JsonPropertyName("totalToPay")]
    public Common.Money? TotalToPay { get; set; }
}

/// <summary>
/// Response from order search
/// </summary>
public class OrdersSearchResult
{
    [JsonPropertyName("checkoutForms")]
    public List<Order> CheckoutForms { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

/// <summary>
/// Query parameters for order search
/// </summary>
public class OrderSearchParams
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
    public List<string>? Status { get; set; }
    public DateTime? BoughtAtFrom { get; set; }
    public DateTime? BoughtAtTo { get; set; }
    public DateTime? UpdatedAtFrom { get; set; }
    public DateTime? UpdatedAtTo { get; set; }
    public string? Sort { get; set; }

    public Dictionary<string, string> ToQueryParams()
    {
        var parameters = new Dictionary<string, string>();
        
        if (Offset.HasValue)
            parameters["offset"] = Offset.Value.ToString();
        
        if (Limit.HasValue)
            parameters["limit"] = Limit.Value.ToString();
        
        if (Status != null && Status.Any())
            foreach (var s in Status) parameters.Add("status", s);
        
        if (BoughtAtFrom.HasValue)
            parameters["buyerLogin"] = BoughtAtFrom.Value.ToString("o");
        
        if (BoughtAtTo.HasValue)
            parameters["boughtAt.lte"] = BoughtAtTo.Value.ToString("o");
        
        if (UpdatedAtFrom.HasValue)
            parameters["updatedAt.gte"] = UpdatedAtFrom.Value.ToString("o");
        
        if (UpdatedAtTo.HasValue)
            parameters["updatedAt.lte"] = UpdatedAtTo.Value.ToString("o");
        
        if (!string.IsNullOrEmpty(Sort))
            parameters["sort"] = Sort;
        
        return parameters;
    }
}
