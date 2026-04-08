using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using AllegroApi.Models.SaleExtensions;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class SaleExtensionsClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly SaleExtensionsClient _client;

    public SaleExtensionsClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl/")
        };
        var options = new AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/"
        };
        _allegroHttpClient = new AllegroHttpClient(httpClient, options);
        _client = new SaleExtensionsClient(_allegroHttpClient);
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
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    [Fact]
    public void Constructor_WithValidHttpClient_CreatesInstance()
    {
        // Act & Assert
        _client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SaleExtensionsClient(null!));
    }

    [Fact]
    public async Task GetTurnoverDiscountsAsync_ReturnsDiscountList()
    {
        // Arrange
        var expectedResponse = new List<TurnoverDiscountDto>
        {
            new TurnoverDiscountDto
            {
                MarketplaceId = "allegro-business-cz",
                Status = "ACTIVE",
                Definitions = new List<TurnoverDiscountDefinitionDto>
                {
                    new TurnoverDiscountDefinitionDto
                    {
                        CumulatingFromDate = "2024-01-01",
                        SpendingFromDate = "2024-02-01"
                    }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetTurnoverDiscountsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].MarketplaceId.Should().Be("allegro-business-cz");
        result[0].Status.Should().Be("ACTIVE");
    }

    [Fact]
    public async Task CreateOrModifyTurnoverDiscountAsync_ValidRequest_ReturnsTurnoverDiscount()
    {
        // Arrange
        var marketplaceId = "allegro-business-cz";
        var request = new TurnoverDiscountRequest
        {
            Thresholds = new List<TurnoverDiscountThresholdDto>
            {
                new TurnoverDiscountThresholdDto
                {
                    MinimumTurnover = new TurnoverAmount { Amount = "4000", Currency = "CZK" },
                    Discount = new TurnoverDiscountPercentage { Percentage = "5" }
                }
            }
        };
        var expectedResponse = new TurnoverDiscountDto
        {
            MarketplaceId = marketplaceId,
            Status = "ACTIVATING"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.CreateOrModifyTurnoverDiscountAsync(marketplaceId, request);

        // Assert
        result.Should().NotBeNull();
        result.MarketplaceId.Should().Be(marketplaceId);
        result.Status.Should().Be("ACTIVATING");
    }

    [Fact]
    public async Task CreateOrModifyTurnoverDiscountAsync_NullMarketplaceId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.CreateOrModifyTurnoverDiscountAsync(null!, new TurnoverDiscountRequest()));
    }

    [Fact]
    public async Task CreateOrModifyTurnoverDiscountAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.CreateOrModifyTurnoverDiscountAsync("allegro-business-cz", null!));
    }

    [Fact]
    public async Task GetPromotionPackagesAsync_ReturnsPackages()
    {
        // Arrange
        var expectedResponse = new AvailablePromotionPackages
        {
            MarketplaceId = "allegro-pl",
            BasePackages = new List<AvailablePromotionPackage>
            {
                new AvailablePromotionPackage { Id = "emphasized1d", Name = "Flexible Feature", CycleDuration = "PT24H" }
            },
            ExtraPackages = new List<AvailablePromotionPackage>
            {
                new AvailablePromotionPackage { Id = "highlight", Name = "Highlight", CycleDuration = "PT240H" }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetPromotionPackagesAsync();

        // Assert
        result.Should().NotBeNull();
        result.MarketplaceId.Should().Be("allegro-pl");
        result.BasePackages.Should().HaveCount(1);
        result.ExtraPackages.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetOfferPromoOptionsAsync_ValidOfferId_ReturnsPromoOptions()
    {
        // Arrange
        var offerId = "offer-123";
        var expectedResponse = new OfferPromoOptions
        {
            OfferId = offerId,
            MarketplaceId = "allegro-pl",
            BasePackage = new OfferPromoOption { Id = "emphasized1d" }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetOfferPromoOptionsAsync(offerId);

        // Assert
        result.Should().NotBeNull();
        result.OfferId.Should().Be(offerId);
        result.BasePackage.Should().NotBeNull();
        result.BasePackage!.Id.Should().Be("emphasized1d");
    }

    [Fact]
    public async Task GetOfferPromoOptionsAsync_NullOfferId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetOfferPromoOptionsAsync(null!));
    }

    [Fact]
    public async Task GetPromoOptionsForSellerOffersAsync_ReturnsPaginatedOptions()
    {
        // Arrange
        var expectedResponse = new OfferPromoOptionsForSeller
        {
            PromoOptions = new List<OfferPromoOptions>
            {
                new OfferPromoOptions { OfferId = "offer-1", MarketplaceId = "allegro-pl" }
            },
            Count = 1,
            TotalCount = 1
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetPromoOptionsForSellerOffersAsync(limit: 100);

        // Assert
        result.Should().NotBeNull();
        result.PromoOptions.Should().HaveCount(1);
        result.Count.Should().Be(1);
    }

    [Fact]
    public async Task ModifyOfferPromoOptionsAsync_ValidRequest_ReturnsUpdatedOptions()
    {
        // Arrange
        var offerId = "offer-123";
        var modifications = new PromoOptionsModifications
        {
            Modifications = new List<PromoOptionsModification>
            {
                new PromoOptionsModification { ModificationType = "CHANGE", PackageType = "BASE", PackageId = "emphasized1d" }
            }
        };
        var expectedResponse = new OfferPromoOptions
        {
            OfferId = offerId,
            MarketplaceId = "allegro-pl",
            BasePackage = new OfferPromoOption { Id = "emphasized1d" }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.ModifyOfferPromoOptionsAsync(offerId, modifications);

        // Assert
        result.Should().NotBeNull();
        result.OfferId.Should().Be(offerId);
        result.BasePackage!.Id.Should().Be("emphasized1d");
    }

    [Fact]
    public async Task ModifyOfferPromoOptionsAsync_NullOfferId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.ModifyOfferPromoOptionsAsync(null!, new PromoOptionsModifications()));
    }

    [Fact]
    public async Task ModifyOfferPromoOptionsAsync_NullModifications_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.ModifyOfferPromoOptionsAsync("offer-123", null!));
    }

    [Fact]
    public async Task CreatePromoOptionsCommandAsync_ValidRequest_ReturnsReport()
    {
        // Arrange
        var commandId = "550e8400-e29b-41d4-a716-446655440000";
        var command = new PromoOptionsCommand
        {
            OfferCriteria = new List<OfferCriterium>
            {
                new OfferCriterium
                {
                    Type = "CONTAINS_OFFERS",
                    Offers = new List<OfferId> { new OfferId { Id = "offer-123" } }
                }
            },
            Modification = new PromoOptionsCommandModification
            {
                BasePackage = new PromoOptionsCommandModificationPackage { Id = "emphasized1d" },
                ModificationTime = "NOW"
            }
        };
        var expectedResponse = new PromoGeneralReport
        {
            Id = commandId,
            TaskCount = new TaskCountDto { Total = 1, Success = 0, Failed = 0 }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.CreatePromoOptionsCommandAsync(commandId, command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(commandId);
    }

    [Fact]
    public async Task CreatePromoOptionsCommandAsync_NullCommandId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.CreatePromoOptionsCommandAsync(null!, new PromoOptionsCommand()));
    }

    [Fact]
    public async Task CreatePromoOptionsCommandAsync_NullCommand_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.CreatePromoOptionsCommandAsync("some-id", null!));
    }

    [Fact]
    public async Task GetPromoOptionsCommandResultAsync_ValidCommandId_ReturnsReport()
    {
        // Arrange
        var commandId = "550e8400-e29b-41d4-a716-446655440000";
        var expectedResponse = new PromoGeneralReport
        {
            Id = commandId,
            TaskCount = new TaskCountDto { Total = 5, Success = 5, Failed = 0 }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetPromoOptionsCommandResultAsync(commandId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(commandId);
        result.TaskCount!.Success.Should().Be(5);
        result.TaskCount.Failed.Should().Be(0);
    }

    [Fact]
    public async Task GetPromoOptionsCommandResultAsync_NullCommandId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetPromoOptionsCommandResultAsync(null!));
    }
}
