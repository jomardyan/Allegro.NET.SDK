using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Products;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class ProductClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly ProductClient _client;

    public ProductClientTests()
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
        _client = new ProductClient(_allegroHttpClient);
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
    public async Task SearchProductsAsync_WithEan_ReturnsProducts()
    {
        // Arrange
        var expectedResponse = new ProductsSearchResult
        {
            Products = new List<Product>
            {
                new Product { Id = "123", Name = "Test Product" }
            },
            Count = 1,
            TotalCount = 1
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var searchParams = new ProductSearchParams { Ean = "1234567890123" };

        // Act
        var result = await _client.SearchProductsAsync(searchParams);

        // Assert
        result.Should().NotBeNull();
        result.Products.Should().HaveCount(1);
        result.Products[0].Id.Should().Be("123");
    }

    [Fact]
    public async Task SearchProductsAsync_WithPhrase_ReturnsProducts()
    {
        // Arrange
        var expectedResponse = new ProductsSearchResult
        {
            Products = new List<Product>
            {
                new Product { Id = "456", Name = "Search Result" }
            },
            Count = 1,
            TotalCount = 1
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var searchParams = new ProductSearchParams { Phrase = "test" };

        // Act
        var result = await _client.SearchProductsAsync(searchParams);

        // Assert
        result.Should().NotBeNull();
        result.Products.Should().HaveCount(1);
    }

    [Fact]
    public async Task SearchProductsAsync_WithoutEanOrPhrase_ThrowsArgumentException()
    {
        // Arrange
        var searchParams = new ProductSearchParams();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.SearchProductsAsync(searchParams));
    }

    [Fact]
    public async Task GetProductAsync_ReturnsProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = "123", Name = "Test Product" };
        SetupHttpResponse(HttpStatusCode.OK, expectedProduct);

        // Act
        var result = await _client.GetProductAsync("123");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("123");
        result.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task ProposeProductChangesAsync_ReturnsProposal()
    {
        // Arrange
        var request = new ProductChangeProposalRequest
        {
            Name = "Updated Product Name"
        };
        var expectedResponse = new ProductChangeProposalDto
        {
            Id = "proposal-123",
            ProductId = "product-123",
            Status = "PENDING"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.ProposeProductChangesAsync("product-123", request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("proposal-123");
        result.Status.Should().Be("PENDING");
    }

    [Fact]
    public async Task ProposeProductChangesAsync_WithNullProductId_ThrowsArgumentNullException()
    {
        // Arrange
        var request = new ProductChangeProposalRequest { Name = "Test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.ProposeProductChangesAsync(null!, request));
    }

    [Fact]
    public async Task GetProductChangeProposalAsync_ReturnsProposal()
    {
        // Arrange
        var expectedResponse = new ProductChangeProposalDto
        {
            Id = "proposal-123",
            ProductId = "product-123",
            Status = "APPROVED"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetProductChangeProposalAsync("proposal-123");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("proposal-123");
        result.Status.Should().Be("APPROVED");
    }

    [Fact]
    public async Task GetProductChangeProposalAsync_WithNullId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _client.GetProductChangeProposalAsync(null!));
    }
}
