using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using AllegroApi.Models.Classifieds;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class ClassifiedsClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly ClassifiedsClient _client;

    public ClassifiedsClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl")
        };
        var options = new AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl"
        };
        _allegroHttpClient = new AllegroHttpClient(_httpClient, options);
        _client = new ClassifiedsClient(_allegroHttpClient);
    }

    public void Dispose()
    {
        _allegroHttpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SetupHttpResponse<T>(HttpStatusCode statusCode, T? responseObject)
    {
        var json = JsonSerializer.Serialize(responseObject);
        var response = new HttpResponseMessage(statusCode)
        {
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
    public async Task GetClassifiedPackagesAsync_ValidOfferId_ReturnsPackages()
    {
        // Arrange
        var offerId = "offer-123";
        var expectedResponse = new ClassifiedResponse
        {
            BasePackage = new ClassifiedPackage { Id = "base-pkg-1" },
            ExtraPackages = new List<ClassifiedExtraPackage>
            {
                new ClassifiedExtraPackage { Id = "extra-1", Republish = true },
                new ClassifiedExtraPackage { Id = "extra-2", Republish = false }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetClassifiedPackagesAsync(offerId);

        // Assert
        result.Should().NotBeNull();
        result.BasePackage.Should().NotBeNull();
        result.BasePackage.Id.Should().Be("base-pkg-1");
        result.ExtraPackages.Should().HaveCount(2);
        result.ExtraPackages[0].Republish.Should().BeTrue();
    }

    [Fact]
    public async Task GetClassifiedPackagesAsync_NullOfferId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetClassifiedPackagesAsync(null!));
    }

    [Fact]
    public async Task AssignClassifiedPackagesAsync_ValidData_CompletesSuccessfully()
    {
        // Arrange
        var offerId = "offer-456";
        var packages = new ClassifiedPackages
        {
            BasePackage = new ClassifiedPackage { Id = "base-1" },
            ExtraPackages = new List<ClassifiedPackage>
            {
                new ClassifiedPackage { Id = "extra-1" }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, (object?)null);

        // Act
        await _client.AssignClassifiedPackagesAsync(offerId, packages);

        // Assert - no exception thrown
    }

    [Fact]
    public async Task AssignClassifiedPackagesAsync_NullOfferId_ThrowsArgumentNullException()
    {
        // Arrange
        var packages = new ClassifiedPackages();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.AssignClassifiedPackagesAsync(null!, packages));
    }

    [Fact]
    public async Task AssignClassifiedPackagesAsync_NullPackages_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.AssignClassifiedPackagesAsync("offer-id", null!));
    }

    [Fact]
    public async Task GetClassifiedPackageConfigurationsAsync_ValidCategoryId_ReturnsConfigurations()
    {
        // Arrange
        var categoryId = "cat-123";
        var expectedResponse = new ClassifiedPackageConfigs
        {
            Packages = new List<ClassifiedPackageConfig>
            {
                new ClassifiedPackageConfig { Id = "pkg-1", Name = "Basic Package", Type = "BASE" },
                new ClassifiedPackageConfig { Id = "pkg-2", Name = "Premium Package", Type = "EXTRA" }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetClassifiedPackageConfigurationsAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(2);
        result.Packages[0].Name.Should().Be("Basic Package");
        result.Packages[1].Type.Should().Be("EXTRA");
    }

    [Fact]
    public async Task GetClassifiedPackageConfigurationsAsync_NullCategoryId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetClassifiedPackageConfigurationsAsync(null!));
    }

    [Fact]
    public async Task GetClassifiedPackageConfigurationAsync_ValidPackageId_ReturnsConfiguration()
    {
        // Arrange
        var packageId = "package-999";
        var expectedResponse = new ClassifiedPackageConfig
        {
            Id = packageId,
            Name = "Configured Package",
            Type = "BASE"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetClassifiedPackageConfigurationAsync(packageId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(packageId);
        result.Name.Should().Be("Configured Package");
    }

    [Fact]
    public async Task GetClassifiedPackageConfigurationAsync_NullPackageId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetClassifiedPackageConfigurationAsync(null!));
    }

    [Fact]
    public async Task GetSellerClassifiedStatsAsync_NoDateRange_ReturnsStats()
    {
        // Arrange
        var expectedResponse = new SellerOfferStatsResponseDto
        {
            EventStatsTotal = new List<ClassifiedEventStat>
            {
                new ClassifiedEventStat { Count = 10, EventType = "ASKED_QUESTION" }
            },
            EventsPerDay = new List<ClassifiedDailyEventStatResponseDto>
            {
                new ClassifiedDailyEventStatResponseDto
                {
                    Date = "2024-01-01",
                    EventStats = new List<ClassifiedEventStat>
                    {
                        new ClassifiedEventStat { Count = 5, EventType = "ASKED_QUESTION" }
                    }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetSellerClassifiedStatsAsync();

        // Assert
        result.Should().NotBeNull();
        result.EventStatsTotal.Should().HaveCount(1);
        result.EventStatsTotal![0].Count.Should().Be(10);
        result.EventsPerDay.Should().HaveCount(1);
        result.EventsPerDay![0].Date.Should().Be("2024-01-01");
    }

    [Fact]
    public async Task GetSellerClassifiedStatsAsync_WithDateRange_ReturnsStats()
    {
        // Arrange
        var dateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var dateTo = new DateTime(2024, 1, 31, 0, 0, 0, DateTimeKind.Utc);
        var expectedResponse = new SellerOfferStatsResponseDto
        {
            EventStatsTotal = new List<ClassifiedEventStat>(),
            EventsPerDay = new List<ClassifiedDailyEventStatResponseDto>()
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetSellerClassifiedStatsAsync(dateFrom, dateTo);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetClassifiedOffersStatsAsync_ValidOfferIds_ReturnsStats()
    {
        // Arrange
        var offerIds = new List<string> { "offer-1", "offer-2" };
        var expectedResponse = new OfferStatsResponseDto
        {
            OfferStats = new List<OfferStatResponseDto>
            {
                new OfferStatResponseDto
                {
                    Offer = new OfferStatModelDto { Id = "offer-1" },
                    EventStatsTotal = new List<ClassifiedEventStat>
                    {
                        new ClassifiedEventStat { Count = 3, EventType = "SHOWED_PHONE_NUMBER" }
                    }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetClassifiedOffersStatsAsync(offerIds);

        // Assert
        result.Should().NotBeNull();
        result.OfferStats.Should().HaveCount(1);
        result.OfferStats![0].Offer!.Id.Should().Be("offer-1");
    }

    [Fact]
    public async Task GetClassifiedOffersStatsAsync_NullOfferIds_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.GetClassifiedOffersStatsAsync(null!));
    }
}
