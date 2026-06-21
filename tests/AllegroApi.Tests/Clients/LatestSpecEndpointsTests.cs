using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using AllegroApi.Models.BatchOperations;
using AllegroApi.Models.Fulfillment;
using AllegroApi.Models.Orders;
using AllegroApi.Models.SaleExtensions;
using AllegroApi.Models.ShipmentManagement;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

/// <summary>
/// Tests for endpoints added by the latest Allegro API spec (flexible bundles,
/// bulk offer modification, serial numbers, delivery proposals, refund dispositions).
/// </summary>
public class LatestSpecEndpointsTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly AllegroHttpClient _http;
    private HttpRequestMessage? _lastRequest;

    public LatestSpecEndpointsTests()
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

    private void Setup(HttpStatusCode statusCode, object? body = null)
    {
        var content = new StringContent(body != null ? JsonSerializer.Serialize(body) : string.Empty, Encoding.UTF8, "application/json");
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => _lastRequest = req)
            .ReturnsAsync(new HttpResponseMessage { StatusCode = statusCode, Content = content });
    }

    private void AssertRequest(HttpMethod method, string pathAndQuery)
    {
        _lastRequest.Should().NotBeNull();
        _lastRequest!.Method.Should().Be(method);
        _lastRequest.RequestUri!.PathAndQuery.Should().Be(pathAndQuery);
    }

    [Fact]
    public async Task FlexibleBundles_List_BuildsQuery()
    {
        Setup(HttpStatusCode.OK, new FlexibleBundlesListingDTO { Bundles = new List<FlexibleBundleListingDTO> { new() { Id = "b1" } } });
        var client = new SaleExtensionsClient(_http);

        var result = await client.GetFlexibleBundlesAsync(offerId: "12345", limit: 10);

        result.Bundles.Should().HaveCount(1);
        AssertRequest(HttpMethod.Get, "/sale/flexible-bundles?offer.id=12345&limit=10");
    }

    [Fact]
    public async Task FlexibleBundles_Create_Posts()
    {
        Setup(HttpStatusCode.Created, new FlexibleBundleGetDTO { Id = "b1", CreatedBy = "USER" });
        var client = new SaleExtensionsClient(_http);

        var result = await client.CreateFlexibleBundleAsync(new FlexibleBundleCreateDTO
        {
            Slots = new List<FlexibleBundleSlotDTO> { new() { Order = 1, RequiredQuantity = 1 } }
        });

        result.Id.Should().Be("b1");
        AssertRequest(HttpMethod.Post, "/sale/flexible-bundles");
    }

    [Fact]
    public async Task FlexibleBundles_Update_Puts()
    {
        Setup(HttpStatusCode.OK, new FlexibleBundleGetDTO { Id = "b1" });
        var client = new SaleExtensionsClient(_http);

        await client.UpdateFlexibleBundleAsync("b1", new FlexibleBundleUpdateDTO());

        AssertRequest(HttpMethod.Put, "/sale/flexible-bundles/b1");
    }

    [Fact]
    public async Task FlexibleBundles_Delete_Deletes()
    {
        Setup(HttpStatusCode.NoContent);
        var client = new SaleExtensionsClient(_http);

        await client.DeleteFlexibleBundleAsync("b1");

        AssertRequest(HttpMethod.Delete, "/sale/flexible-bundles/b1");
    }

    [Fact]
    public async Task OfferBulkModification_Create_Posts()
    {
        Setup(HttpStatusCode.Created, new OfferBulkModificationReport { Id = "cmd-1", TaskCount = new OfferBulkTaskCount { Total = 1 } });
        var client = new BatchOperationsClient(_http);

        var result = await client.CreateOfferBulkModificationCommandAsync(new OfferBulkChangeCommand
        {
            CommandId = "11111111-1111-1111-1111-111111111111",
            Modifications = new List<OfferBulkModification>
            {
                new() { OfferId = "123", Stock = new OfferBulkStockModification { ChangeType = "FIXED", Value = 10 } }
            }
        });

        result.Id.Should().Be("cmd-1");
        AssertRequest(HttpMethod.Post, "/sale/offer-bulk-modification-commands");
    }

    [Fact]
    public async Task OfferBulkModification_Tasks_BuildsQuery()
    {
        Setup(HttpStatusCode.OK, new OfferBulkModificationTaskReport());
        var client = new BatchOperationsClient(_http);

        await client.GetOfferBulkModificationCommandTasksAsync("cmd-1", limit: 50, offset: 0);

        AssertRequest(HttpMethod.Get, "/sale/offer-bulk-modification-commands/cmd-1/tasks?limit=50&offset=0");
    }

    [Fact]
    public async Task SerialNumbers_Post_WithRevision()
    {
        Setup(HttpStatusCode.NoContent);
        var client = new OrderManagementClient(_http);

        await client.SetLineItemsSerialNumbersAsync(
            "order-1",
            new CheckoutFormLineItemsSetSerialNumbersRequest
            {
                LineItems = new List<CheckoutFormLineItemSetSerialNumbersRequest>
                {
                    new() { Id = "li-1", SerialNumbers = new CheckoutFormLineItemSetSerialNumbersEntries
                    {
                        Entries = new List<CheckoutFormLineItemSerialNumberEntry> { new() { Value = "SN-1" } }
                    } }
                }
            },
            revision: "abc");

        AssertRequest(HttpMethod.Post, "/order/checkout-forms/order-1/serial-numbers?checkoutForm.revision=abc");
    }

    [Fact]
    public async Task DeliveryProposals_Get_UsesOrderPath()
    {
        Setup(HttpStatusCode.OK, new DeliveryProposalDto { OrderId = "order-1" });
        var client = new ShipmentManagementClient(_http);

        var result = await client.GetDeliveryProposalsAsync("order-1");

        result.OrderId.Should().Be("order-1");
        AssertRequest(HttpMethod.Get, "/shipment-management/delivery-proposals/order-1");
    }

    [Fact]
    public async Task RefundDispositions_Get_BuildsQuery()
    {
        Setup(HttpStatusCode.OK, new FulfillmentRefundDispositionsResponse { Report = new List<FulfillmentRefundDisposition> { new() { Type = "RETURN" } } });
        var client = new FulfillmentClient(_http);

        var result = await client.GetRefundDispositionsAsync(createdAtGte: "2026-01-01T00:00:00Z", limit: 100);

        result.Report.Should().HaveCount(1);
        AssertRequest(HttpMethod.Get, "/fulfillment/returns/refund-dispositions?createdAt.gte=2026-01-01T00%3A00%3A00Z&limit=100");
    }
}
