using System.Text.Json.Serialization;

namespace AllegroApi.Models.CustomerReturns;

/// <summary>
/// Response containing a list of customer returns.
/// </summary>
public record CustomerReturnResponse
{
    /// <summary>
    /// List of customer returns.
    /// </summary>
    [JsonPropertyName("customerReturns")]
    public List<CustomerReturn>? CustomerReturns { get; init; }

    /// <summary>
    /// Total count of customer returns matching the criteria.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>
    /// Offset used in pagination.
    /// </summary>
    [JsonPropertyName("offset")]
    public int? Offset { get; init; }
}

/// <summary>
/// Detailed information about a customer return.
/// </summary>
public record CustomerReturn
{
    /// <summary>
    /// Customer return identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Associated order identifier.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Reference number for the return.
    /// </summary>
    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Buyer information.
    /// </summary>
    [JsonPropertyName("buyer")]
    public BuyerDto? Buyer { get; init; }

    /// <summary>
    /// Items being returned.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ReturnItemDto>? Items { get; init; }

    /// <summary>
    /// Parcels associated with the return.
    /// </summary>
    [JsonPropertyName("parcels")]
    public List<ReturnParcelDto>? Parcels { get; init; }

    /// <summary>
    /// Current status of the return.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Marketplace where the return was created.
    /// </summary>
    [JsonPropertyName("marketplaceId")]
    public string? MarketplaceId { get; init; }

    /// <summary>
    /// Return creation timestamp.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Rejection information (if return was rejected).
    /// </summary>
    [JsonPropertyName("rejection")]
    public ReturnRejectionDto? Rejection { get; init; }
}

/// <summary>
/// Buyer information for a return.
/// </summary>
public record BuyerDto
{
    /// <summary>
    /// Buyer's login name.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// Buyer's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Buyer's first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Buyer's last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
}

/// <summary>
/// Item being returned.
/// </summary>
public record ReturnItemDto
{
    /// <summary>
    /// Offer identifier for the item.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; init; }

    /// <summary>
    /// Item name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Quantity being returned.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }

    /// <summary>
    /// Return reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }
}

/// <summary>
/// Parcel information for a return.
/// </summary>
public record ReturnParcelDto
{
    /// <summary>
    /// Waybill number.
    /// </summary>
    [JsonPropertyName("waybill")]
    public string? Waybill { get; init; }

    /// <summary>
    /// Waybill of the transporting carrier.
    /// </summary>
    [JsonPropertyName("transportingWaybill")]
    public string? TransportingWaybill { get; init; }

    /// <summary>
    /// Carrier identifier.
    /// </summary>
    [JsonPropertyName("carrierId")]
    public string? CarrierId { get; init; }

    /// <summary>
    /// Transporting carrier identifier.
    /// </summary>
    [JsonPropertyName("transportingCarrierId")]
    public string? TransportingCarrierId { get; init; }

    /// <summary>
    /// Sender information.
    /// </summary>
    [JsonPropertyName("sender")]
    public ParcelSenderDto? Sender { get; init; }
}

/// <summary>
/// Parcel sender information.
/// </summary>
public record ParcelSenderDto
{
    /// <summary>
    /// Sender's phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }
}

/// <summary>
/// Rejection information for a return.
/// </summary>
public record ReturnRejectionDto
{
    /// <summary>
    /// Rejection reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    /// <summary>
    /// Rejection timestamp.
    /// </summary>
    [JsonPropertyName("rejectedAt")]
    public DateTime? RejectedAt { get; init; }
}

/// <summary>
/// Request to reject a customer return refund.
/// </summary>
public record CustomerReturnRefundRejectionRequest
{
    /// <summary>
    /// Reason for rejecting the return refund.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }
}
