using AllegroApi;
using AllegroApi.Configuration;
using FluentAssertions;
using Xunit;

namespace AllegroApi.Tests;

public class AllegroApiClientTests : IDisposable
{
    private AllegroApiClient? _client;

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void CreateProduction_CreatesClient()
    {
        // Act
        _client = AllegroApiClient.CreateProduction("test-token");

        // Assert
        _client.Should().NotBeNull();
    }

    [Fact]
    public void CreateSandbox_CreatesClient()
    {
        // Act
        _client = AllegroApiClient.CreateSandbox("test-token");

        // Assert
        _client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithOptions_CreatesClient()
    {
        // Arrange
        var options = new AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/",
            TimeoutSeconds = 60
        };

        // Act
        _client = new AllegroApiClient(options);

        // Assert
        _client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullOptions_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AllegroApiClient(null!));
    }

    [Fact]
    public void Offers_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.Offers.Should().NotBeNull();
    }

    [Fact]
    public void Products_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.Products.Should().NotBeNull();
    }

    [Fact]
    public void Categories_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.Categories.Should().NotBeNull();
    }

    [Fact]
    public void Orders_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.Orders.Should().NotBeNull();
    }

    [Fact]
    public void Images_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.Images.Should().NotBeNull();
    }

    [Fact]
    public void BatchOperations_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.BatchOperations.Should().NotBeNull();
    }

    [Fact]
    public void ShipmentManagement_Property_IsInitialized()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act & Assert
        _client.ShipmentManagement.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_DisposesResources()
    {
        // Arrange
        _client = AllegroApiClient.CreateProduction("test-token");

        // Act
        _client.Dispose();

        // Assert - No exception thrown
    }
}
