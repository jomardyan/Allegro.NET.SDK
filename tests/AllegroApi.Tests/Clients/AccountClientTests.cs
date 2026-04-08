using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Account;
using AllegroApi.Models.Users;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class AccountClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly AccountClient _client;

    public AccountClientTests()
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
        _client = new AccountClient(_allegroHttpClient);
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
    public async Task GetMeAsync_ReturnsUserInfo()
    {
        // Arrange
        var expectedUser = new AccountInfoResponse
        {
            Id = "user123",
            Login = "testuser",
            Email = "test@example.com"
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedUser);

        // Act
        var result = await _client.GetMeAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("user123");
        result.Login.Should().Be("testuser");
    }

    [Fact]
    public async Task GetAccountSettingsAsync_ReturnsSettings()
    {
        // Arrange
        var expectedSettings = new AccountSettings
        {
            NotificationsEnabled = true
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedSettings);

        // Act
        var result = await _client.GetAccountSettingsAsync();

        // Assert
        result.Should().NotBeNull();
        result.NotificationsEnabled.Should().BeTrue();
    }
}
