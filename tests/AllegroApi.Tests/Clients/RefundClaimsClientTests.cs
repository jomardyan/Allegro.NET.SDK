using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Orders;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class RefundClaimsClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly RefundClaimsClient _client;

    public RefundClaimsClientTests()
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
        _client = new RefundClaimsClient(_allegroHttpClient);
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
    public async Task GetRefundClaimsAsync_ReturnsClaims()
    {
        // Arrange
        var expectedResponse = new RefundClaimsResponse
        {
            RefundClaims = new List<RefundClaim>
            {
                new RefundClaim { Id = "claim1", Status = "IN_PROGRESS" }
            },
            Count = 1
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetRefundClaimsAsync();

        // Assert
        result.Should().NotBeNull();
        result.RefundClaims.Should().HaveCount(1);
        result.RefundClaims![0].Id.Should().Be("claim1");
    }

    [Fact]
    public async Task GetRefundClaimAsync_WithValidId_ReturnsClaim()
    {
        // Arrange
        var expectedClaim = new RefundClaim { Id = "claim2", Status = "GRANTED" };
        SetupHttpResponse(HttpStatusCode.OK, expectedClaim);

        // Act
        var result = await _client.GetRefundClaimAsync("claim2");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("claim2");
        result.Status.Should().Be("GRANTED");
    }

    [Fact]
    public async Task CreateRefundClaimAsync_WithValidRequest_ReturnsClaim()
    {
        // Arrange
        var request = new CreateRefundClaimRequest
        {
            LineItemId = "line-item-123",
            Reason = "DAMAGED"
        };
        var expectedClaim = new RefundClaim { Id = "claim3", Status = "IN_PROGRESS" };
        SetupHttpResponse(HttpStatusCode.Created, expectedClaim);

        // Act
        var result = await _client.CreateRefundClaimAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("claim3");
    }

    [Fact]
    public async Task GetRefundClaimAsync_WithNullId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _client.GetRefundClaimAsync(null!));
    }
}
