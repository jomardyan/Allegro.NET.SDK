using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Http;
using AllegroApi.Models.Offers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.Tests.Clients;

public class OfferManagementClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly OfferManagementClient _client;
    private readonly AllegroApiOptions _options;

    public OfferManagementClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _options = new AllegroApiOptions
        {
            BaseUrl = "https://api.allegro.pl",
            AccessToken = "test-token"
        };
        _allegroHttpClient = new AllegroHttpClient(_httpClient, _options, NullLogger<AllegroHttpClient>.Instance);
        _client = new OfferManagementClient(_allegroHttpClient, NullLogger<OfferManagementClient>.Instance);
    }

    [Fact]
    public async Task CreateProductOfferAsync_ValidRequest_ReturnsResponse()
    {
        // Arrange
        var request = new SaleProductOfferRequestV1
        {
            ProductSet = new List<ProductSet>
            {
                new ProductSet
                {
                    Product = new ProductIdentifier { Id = "test-product-id" },
                    Quantity = 1
                }
            },
            SellingMode = new SellingMode
            {
                Price = new Models.Common.Money { Amount = "99.99", Currency = "PLN" }
            },
            Stock = new Stock { Available = 10 }
        };

        var expectedResponse = new SaleProductOfferResponseV1
        {
            Id = "test-offer-id"
        };

        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.CreateProductOfferAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("test-offer-id");
    }

    [Fact]
    public async Task GetProductOfferAsync_ValidOfferId_ReturnsOffer()
    {
        // Arrange
        var offerId = "12345678";
        var expectedResponse = new SaleProductOfferResponseV1
        {
            Id = offerId
        };

        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetProductOfferAsync(offerId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(offerId);
    }

    [Fact]
    public async Task GetProductOfferAsync_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        var offerId = "non-existent-id";
        SetupHttpResponse(HttpStatusCode.NotFound, new { });

        // Act & Assert
        await Assert.ThrowsAsync<AllegroNotFoundException>(() => 
            _client.GetProductOfferAsync(offerId));
    }

    [Fact]
    public async Task EditProductOfferAsync_ValidRequest_ReturnsUpdatedOffer()
    {
        // Arrange
        var offerId = "12345678";
        var request = new SaleProductOfferPatchRequestV1
        {
            Stock = new Stock { Available = 20 }
        };

        var expectedResponse = new SaleProductOfferResponseV1
        {
            Id = offerId
        };

        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.EditProductOfferAsync(offerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(offerId);
    }

    [Fact]
    public async Task SearchOffersAsync_WithFilters_ReturnsOffers()
    {
        // Arrange
        var searchParams = new OfferSearchParams
        {
            Name = "iPhone",
            Limit = 10,
            Offset = 0
        };

        var expectedResponse = new OffersSearchResultDto
        {
            Offers = new List<OfferListingDto>
            {
                new OfferListingDto { Id = "123", Name = "iPhone 15" }
            },
            Count = 1,
            TotalCount = 1
        };

        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.SearchOffersAsync(searchParams);

        // Assert
        result.Should().NotBeNull();
        result.Offers.Should().HaveCount(1);
        result.Offers[0].Name.Should().Contain("iPhone");
    }

    [Fact]
    public async Task DeleteOfferAsync_ValidOfferId_Succeeds()
    {
        // Arrange
        var offerId = "12345678";
        SetupHttpResponse<object>(HttpStatusCode.NoContent, null);

        // Act
        await _client.DeleteOfferAsync(offerId);

        // Assert
        // No exception thrown means success
    }

    [Fact]
    public async Task ChangePriceAsync_ValidRequest_ReturnsChangePrice()
    {
        // Arrange
        var offerId = "12345678";
        var commandId = Guid.NewGuid().ToString();
        var request = new ChangePriceWithoutOutput
        {
            Input = new ChangePriceInput
            {
                BuyNowPrice = new Models.Common.Money { Amount = "89.99", Currency = "PLN" }
            }
        };

        var expectedResponse = new ChangePrice
        {
            Id = commandId,
            Input = request.Input,
            Output = new ChangePriceOutput { Status = "SUCCESS" }
        };

        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.ChangePriceAsync(offerId, commandId, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(commandId);
        result.Output?.Status.Should().Be("SUCCESS");
    }

    [Fact]
    public async Task ChangePublicationStatusAsync_ValidRequest_ReturnsReport()
    {
        // Arrange
        var commandId = Guid.NewGuid().ToString();
        var request = new PublicationChangeCommandDto
        {
            Publication = new PublicationCommand { Action = "ACTIVATE" },
            OfferCriteria = new List<OfferCriterion>
            {
                new OfferCriterion
                {
                    Type = "CONTAINS_OFFERS",
                    Offers = new List<OfferIdReference>
                    {
                        new OfferIdReference { Id = "12345678" }
                    }
                }
            }
        };

        var expectedResponse = new GeneralReport
        {
            Id = commandId,
            Status = "RUNNING",
            Summary = new Summary { Total = 1, Success = 0, Failed = 0, Pending = 1 }
        };

        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.ChangePublicationStatusAsync(commandId, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(commandId);
        result.Status.Should().Be("RUNNING");
    }

    [Fact]
    public async Task GetPublicationReportAsync_ValidCommandId_ReturnsReport()
    {
        // Arrange
        var commandId = Guid.NewGuid().ToString();
        var expectedResponse = new GeneralReport
        {
            Id = commandId,
            Status = "SUCCESS",
            Summary = new Summary { Total = 1, Success = 1, Failed = 0, Pending = 0 }
        };

        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetPublicationReportAsync(commandId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("SUCCESS");
        result.Summary?.Success.Should().Be(1);
    }

    private void SetupHttpResponse<T>(HttpStatusCode statusCode, T? responseObject)
    {
        var responseContent = responseObject != null 
            ? JsonSerializer.Serialize(responseObject, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            })
            : string.Empty;

        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseContent, System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _allegroHttpClient?.Dispose();
    }
}
