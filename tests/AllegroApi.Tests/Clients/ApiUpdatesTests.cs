using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using AllegroApi.Models.Messaging;
using AllegroApi.Models.Orders;
using AllegroApi.Models.PostPurchaseIssues;
using AllegroApi.Models.PriceAutomation;
using AllegroApi.Models.Pricing;
using AllegroApi.Models.SaleExtensions;
using AllegroApi.Models.Shipping;
using AllegroApi.Models.Users;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

/// <summary>
/// Tests for newly implemented Allegro API endpoints and for corrected (bug-fixed) endpoint paths.
/// </summary>
public class ApiUpdatesTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly AllegroHttpClient _http;
    private HttpRequestMessage? _lastRequest;

    public ApiUpdatesTests()
    {
        _handler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_handler.Object) { BaseAddress = new Uri("https://api.allegro.pl/") };
        var options = new AllegroApiOptions { AccessToken = "test-token", BaseUrl = "https://api.allegro.pl/" };
        _http = new AllegroHttpClient(httpClient, options);
    }

    public void Dispose()
    {
        _http?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void Setup(HttpStatusCode statusCode, object? body = null, byte[]? raw = null)
    {
        HttpContent content = raw != null
            ? new ByteArrayContent(raw)
            : new StringContent(body != null ? JsonSerializer.Serialize(body) : string.Empty, Encoding.UTF8, "application/json");

        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => _lastRequest = req)
            .ReturnsAsync(new HttpResponseMessage { StatusCode = statusCode, Content = content });
    }

    private void AssertRequest(HttpMethod method, string pathAndQuery)
    {
        _lastRequest.Should().NotBeNull();
        _lastRequest!.Method.Should().Be(method);
        (_lastRequest.RequestUri!.PathAndQuery).Should().Be(pathAndQuery);
    }

    // ----- Bug fixes -----

    [Fact]
    public async Task Messaging_GetMessage_UsesCorrectPath()
    {
        Setup(HttpStatusCode.OK, new Message { Id = "m1" });
        var client = new MessagingClient(_http);

        var result = await client.GetMessageAsync("m1");

        result.Id.Should().Be("m1");
        AssertRequest(HttpMethod.Get, "/messaging/messages/m1");
    }

    [Fact]
    public async Task Messaging_MarkThreadRead_UsesThreadReadPath()
    {
        Setup(HttpStatusCode.OK, new MessageThreadReadResult { Id = "t1", Read = true });
        var client = new MessagingClient(_http);

        var result = await client.MarkThreadReadAsync("t1");

        result.Read.Should().BeTrue();
        AssertRequest(HttpMethod.Put, "/messaging/threads/t1/read");
    }

    [Fact]
    public async Task Messaging_WriteNewMessage_PostsToMessages()
    {
        Setup(HttpStatusCode.Created, new Message { Id = "m2" });
        var client = new MessagingClient(_http);

        var result = await client.WriteNewMessageAsync(new NewMessage { Text = "hi", Recipient = new MessageRecipient { Login = "buyer" } });

        result.Id.Should().Be("m2");
        AssertRequest(HttpMethod.Post, "/messaging/messages");
    }

    [Fact]
    public async Task Shipping_GetDeliverySettings_UsesQueryParameterNotPathSegment()
    {
        Setup(HttpStatusCode.OK, new DeliverySettingsResponse());
        var client = new ShippingClient(_http);

        await client.GetDeliverySettingsAsync("allegro-pl");

        AssertRequest(HttpMethod.Get, "/sale/delivery-settings?marketplace.id=allegro-pl");
    }

    [Fact]
    public async Task Shipping_UpdateDeliverySettings_PutsToBarePath()
    {
        Setup(HttpStatusCode.OK, new DeliverySettingsResponse());
        var client = new ShippingClient(_http);

        await client.UpdateDeliverySettingsAsync(new DeliverySettingsRequest { Marketplace = new DeliveryMarketplace { Id = "allegro-pl" } });

        AssertRequest(HttpMethod.Put, "/sale/delivery-settings");
    }

    [Fact]
    public async Task Users_RequestRatingRemoval_UsesPutToRemoval()
    {
        Setup(HttpStatusCode.Created, new UserRatingRemoval { PossibleTo = "2026-01-01T00:00:00Z" });
        var client = new UsersClient(_http);

        var result = await client.RequestRatingRemovalAsync("r1",
            new RatingRemovalRequest { Request = new RatingRemovalRequestDetails { Message = "please remove" } });

        result.PossibleTo.Should().Be("2026-01-01T00:00:00Z");
        AssertRequest(HttpMethod.Put, "/sale/user-ratings/r1/removal");
    }

    // ----- New endpoints -----

    [Fact]
    public async Task PriceAutomation_GetRules_ReturnsRules()
    {
        Setup(HttpStatusCode.OK, new AutomaticPricingRulesResponse
        {
            Rules = new List<AutomaticPricingRuleResponse> { new() { Id = "rule1", Type = "EXCHANGE_RATE", Name = "r" } }
        });
        var client = new PriceAutomationClient(_http);

        var result = await client.GetRulesAsync();

        result.Rules.Should().HaveCount(1);
        result.Rules![0].Id.Should().Be("rule1");
        AssertRequest(HttpMethod.Get, "/sale/price-automation/rules");
    }

    [Fact]
    public async Task PriceAutomation_GetOfferRules_UsesOfferPath()
    {
        Setup(HttpStatusCode.OK, new OfferRules());
        var client = new PriceAutomationClient(_http);

        await client.GetOfferRulesAsync("offer-1");

        AssertRequest(HttpMethod.Get, "/sale/price-automation/offers/offer-1/rules");
    }

    [Fact]
    public async Task PriceAutomation_DeleteRule_NullId_Throws()
    {
        var client = new PriceAutomationClient(_http);
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteRuleAsync(null!));
    }

    [Fact]
    public async Task SaleExtensions_GetBundle_ReturnsBundle()
    {
        Setup(HttpStatusCode.OK, new OfferBundleDTO { Id = "b1", CreatedBy = "USER" });
        var client = new SaleExtensionsClient(_http);

        var result = await client.GetBundleAsync("b1");

        result.Id.Should().Be("b1");
        AssertRequest(HttpMethod.Get, "/sale/bundles/b1");
    }

    [Fact]
    public async Task SaleExtensions_UpdateBundleDiscount_PutsToDiscountPath()
    {
        Setup(HttpStatusCode.OK, new OfferBundleDTO { Id = "b1" });
        var client = new SaleExtensionsClient(_http);

        await client.UpdateBundleDiscountAsync("b1", new UpdateOfferBundleDiscountDTO
        {
            Discounts = new List<BundleDiscountDTO> { new() { Amount = "5.00", Currency = "PLN" } }
        });

        AssertRequest(HttpMethod.Put, "/sale/bundles/b1/discount");
    }

    [Fact]
    public async Task SaleExtensions_DeleteOfferTag_DeletesTag()
    {
        Setup(HttpStatusCode.NoContent);
        var client = new SaleExtensionsClient(_http);

        await client.DeleteOfferTagAsync("tag1");

        AssertRequest(HttpMethod.Delete, "/sale/offer-tags/tag1");
    }

    [Fact]
    public async Task SaleExtensions_GetLoyaltyPromotion_ReturnsPromotion()
    {
        Setup(HttpStatusCode.OK, new SellerRebateDto { Id = "p1", Status = "ACTIVE" });
        var client = new SaleExtensionsClient(_http);

        var result = await client.GetLoyaltyPromotionAsync("p1");

        result.Status.Should().Be("ACTIVE");
        AssertRequest(HttpMethod.Get, "/sale/loyalty/promotions/p1");
    }

    [Fact]
    public async Task Orders_GetOrderEvents_BuildsQuery()
    {
        Setup(HttpStatusCode.OK, new OrderEventsList { Events = new List<OrderEvent> { new() { Id = "e1", Type = "BOUGHT" } } });
        var client = new OrderManagementClient(_http);

        var result = await client.GetOrderEventsAsync(from: "100", limit: 50);

        result.Events.Should().HaveCount(1);
        AssertRequest(HttpMethod.Get, "/order/events?from=100&limit=50");
    }

    [Fact]
    public async Task Orders_AddShipment_PostsWaybill()
    {
        Setup(HttpStatusCode.Created, new CheckoutFormAddWaybillCreated { Id = "s1", Waybill = "WB1" });
        var client = new OrderManagementClient(_http);

        var result = await client.AddOrderShipmentAsync("order-1", new CheckoutFormAddWaybillRequest { CarrierId = "INPOST", Waybill = "WB1" });

        result.Waybill.Should().Be("WB1");
        AssertRequest(HttpMethod.Post, "/order/checkout-forms/order-1/shipments");
    }

    [Fact]
    public async Task Orders_GetParcelTracking_BuildsWaybillQuery()
    {
        Setup(HttpStatusCode.OK, new CarrierParcelTrackingResponse { CarrierId = "INPOST" });
        var client = new OrderManagementClient(_http);

        await client.GetParcelTrackingAsync("INPOST", new[] { "WB1", "WB2" });

        AssertRequest(HttpMethod.Get, "/order/carriers/INPOST/tracking?waybill=WB1&waybill=WB2");
    }

    [Fact]
    public async Task AllegroPrices_GetAccountParticipation_ReturnsStatus()
    {
        Setup(HttpStatusCode.OK, new AllegroPricesAccountParticipationResponse
        {
            Marketplaces = new List<AccountParticipationMarketplace> { new() { Id = "allegro-pl", Status = "ACTIVE" } }
        });
        var client = new AllegroPricesClient(_http);

        var result = await client.GetAccountParticipationAsync();

        result.Marketplaces.Should().HaveCount(1);
        AssertRequest(HttpMethod.Get, "/sale/allegro-prices/accounts/participations");
    }

    [Fact]
    public async Task AllegroPrices_QueryOffers_PostsQuery()
    {
        Setup(HttpStatusCode.OK, new OfferStatusQueryResponseDto { Count = 0, TotalCount = 0 });
        var client = new AllegroPricesClient(_http);

        await client.QueryOffersAsync(new OfferStatusQueryRequestDto
        {
            Offer = new OfferStatusFilterDto { Scope = "DISCOUNTED" }
        });

        AssertRequest(HttpMethod.Post, "/sale/allegro-prices/offers-queries");
    }

    [Fact]
    public async Task Category_GetProductParameters_ReturnsParameters()
    {
        Setup(HttpStatusCode.OK, new AllegroApi.Models.Categories.CategoryProductParameterList
        {
            Parameters = new List<AllegroApi.Models.Categories.CategoryProductParameter> { new() { Id = "p1", Name = "Color" } }
        });
        var client = new CategoryClient(_http);

        var result = await client.GetProductParametersAsync("123");

        result.Parameters.Should().HaveCount(1);
        AssertRequest(HttpMethod.Get, "/sale/categories/123/product-parameters");
    }

    [Fact]
    public async Task Offers_GetOfferRating_ReturnsRating()
    {
        Setup(HttpStatusCode.OK, new AllegroApi.Models.Offers.OfferRating { AverageScore = "4.5", TotalResponses = 10 });
        var client = new OfferManagementClient(_http);

        var result = await client.GetOfferRatingAsync("offer-1");

        result.TotalResponses.Should().Be(10);
        AssertRequest(HttpMethod.Get, "/sale/offers/offer-1/rating");
    }

    [Fact]
    public async Task PostPurchaseIssues_ChangeStatus_PostsToStatusPath()
    {
        Setup(HttpStatusCode.OK);
        var client = new PostPurchaseIssuesClient(_http);

        await client.ChangeIssueStatusAsync("issue-1", new ClaimStatusChangeRequest { Status = "ACCEPTED_REFUND", Message = "ok" });

        AssertRequest(HttpMethod.Post, "/sale/issues/issue-1/status");
    }
}
