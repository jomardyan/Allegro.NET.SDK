using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Users;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class UsersClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly UsersClient _client;

    public UsersClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl")
        };
        var options = new AllegroApi.Configuration.AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl"
        };
        _allegroHttpClient = new AllegroHttpClient(_httpClient, options);
        _client = new UsersClient(_allegroHttpClient);
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
    public async Task GetUserAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var expectedUser = new UserInfo
        {
            Id = "user456",
            Login = "testuser2"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedUser);

        // Act
        var result = await _client.GetUserAsync("user456");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("user456");
        result.Login.Should().Be("testuser2");
    }

    [Fact]
    public async Task GetUserAsync_WithNullId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _client.GetUserAsync(null!));
    }
}
