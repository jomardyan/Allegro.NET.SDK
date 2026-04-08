using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Orders;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class OrderManagementClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly OrderManagementClient _client;

    public OrderManagementClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl/")
        };
        var options = new AllegroApi.Configuration.AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/"
        };
        _allegroHttpClient = new AllegroHttpClient(httpClient, options);
        _client = new OrderManagementClient(_allegroHttpClient);
    }

    public void Dispose()
    {
        _allegroHttpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SetupHttpResponse<T>(HttpStatusCode statusCode, T? responseObject)
    {
        var json = JsonSerializer.Serialize(responseObject);
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsOrders()
    {
        // Arrange
        var expectedResponse = new OrdersSearchResult
        {
            CheckoutForms = new List<Order>
            {
                new Order { Id = "order1", Status = "READY_FOR_PROCESSING" },
                new Order { Id = "order2", Status = "READY_FOR_PROCESSING" }
            },
            Count = 2,
            TotalCount = 2
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var searchParams = new OrderSearchParams { Limit = 10 };

        // Act
        var result = await _client.GetOrdersAsync(searchParams);

        // Assert
        result.Should().NotBeNull();
        result.CheckoutForms.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetOrdersAsync_WithFilters_ReturnsFilteredOrders()
    {
        // Arrange
        var expectedResponse = new OrdersSearchResult
        {
            CheckoutForms = new List<Order>
            {
                new Order { Id = "order1", Status = "READY_FOR_PROCESSING" }
            },
            Count = 1,
            TotalCount = 1
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var searchParams = new OrderSearchParams 
        { 
            Limit = 10
        };

        // Act
        var result = await _client.GetOrdersAsync(searchParams);

        // Assert
        result.Should().NotBeNull();
        result.CheckoutForms.Should().HaveCount(1);
        result.CheckoutForms[0].Status.Should().Be("READY_FOR_PROCESSING");
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsOrder()
    {
        // Arrange
        var expectedOrder = new Order
        {
            Id = "order123",
            Status = "READY_FOR_PROCESSING",
            Summary = new OrderSummary { TotalToPay = new Models.Common.Money { Amount = "100.00", Currency = "PLN" } }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedOrder);

        // Act
        var result = await _client.GetOrderAsync("order123");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("order123");
        result.Status.Should().Be("READY_FOR_PROCESSING");
    }

    [Fact]
    public async Task GetOrderEventsStatisticsAsync_ReturnsStatistics()
    {
        // Arrange
        var expectedResponse = new OrderEventStats
        {
            LatestEvent = new LatestEvent
            {
                Id = "event123",
                OccurredAt = DateTime.UtcNow
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetOrderEventsStatisticsAsync();

        // Assert
        result.Should().NotBeNull();
        result.LatestEvent.Should().NotBeNull();
        result.LatestEvent!.Id.Should().Be("event123");
    }

    [Fact]
    public async Task GetCarriersAsync_ReturnsCarriers()
    {
        // Arrange
        var expectedResponse = new CarriersResponse
        {
            Carriers = new List<Carrier>
            {
                new Carrier { Id = "carrier1", Name = "DHL" },
                new Carrier { Id = "carrier2", Name = "UPS" }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCarriersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Carriers.Should().HaveCount(2);
        result.Carriers![0].Name.Should().Be("DHL");
    }

    [Fact]
    public async Task UpdateFulfillmentStatusAsync_WithNullCheckoutFormId_ThrowsArgumentNullException()
    {
        // Arrange
        var request = new FulfillmentUpdateRequest { Status = "SENT" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.UpdateFulfillmentStatusAsync(null!, request));
    }

    [Fact]
    public async Task UpdateFulfillmentStatusAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.UpdateFulfillmentStatusAsync("order123", null!));
    }

    [Fact]
    public async Task GetOrderInvoicesAsync_ReturnsInvoices()
    {
        // Arrange
        var expectedResponse = new OrderInvoicesResponse
        {
            Invoices = new List<OrderInvoice>
            {
                new OrderInvoice 
                { 
                    Id = "inv1", 
                    Number = "INV/2025/001",
                    Type = "VAT"
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetOrderInvoicesAsync("order123");

        // Assert
        result.Should().NotBeNull();
        result.Invoices.Should().HaveCount(1);
        result.Invoices![0].Number.Should().Be("INV/2025/001");
    }

    [Fact]
    public async Task GetOrderInvoicesAsync_WithNullId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.GetOrderInvoicesAsync(null!));
    }

    [Fact]
    public async Task GetInvoiceFileAsync_ReturnsByteArray()
    {
        // Arrange
        var pdfContent = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF header
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ByteArrayContent(pdfContent)
        };
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetInvoiceFileAsync("order123", "inv1");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4);
        result[0].Should().Be(0x25); // PDF starts with %
    }

    [Fact]
    public async Task GetInvoiceFileAsync_WithNullCheckoutFormId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.GetInvoiceFileAsync(null!, "inv1"));
    }

    [Fact]
    public async Task GetInvoiceFileAsync_WithNullInvoiceId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.GetInvoiceFileAsync("order123", null!));
    }
}
