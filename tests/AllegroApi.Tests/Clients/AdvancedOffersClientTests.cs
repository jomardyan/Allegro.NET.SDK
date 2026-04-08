using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class AdvancedOffersClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly AdvancedOffersClient _client;

    public AdvancedOffersClientTests()
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
        _client = new AdvancedOffersClient(_allegroHttpClient);
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
        Assert.Throws<ArgumentNullException>(() => new AdvancedOffersClient(null!));
    }
}
