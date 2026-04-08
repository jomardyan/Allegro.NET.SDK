using AllegroApi.Exceptions;
using AllegroApi.Http;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Http;

public class AllegroHttpClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _client;

    public AllegroHttpClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl/")
        };
        var options = new AllegroApi.Configuration.AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/",
            // For tests we don't want client to retry on rate limit (avoid long Task.Delay)
            MaxRetryAttempts = 0
        };
        _client = new AllegroHttpClient(httpClient, options);
    }

    public void Dispose()
    {
        _client?.Dispose();
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
    public async Task GetAsync_SuccessfulRequest_ReturnsData()
    {
        // Arrange
        var expectedData = new TestResponse { Id = "123", Name = "Test" };
        SetupHttpResponse(HttpStatusCode.OK, expectedData);

        // Act
        var result = await _client.GetAsync<TestResponse>("/test", null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("123");
        result.Name.Should().Be("Test");
    }

    [Fact]
    public async Task GetAsync_WithQueryParams_BuildsCorrectUrl()
    {
        // Arrange
        var expectedData = new TestResponse { Id = "123", Name = "Test" };
        SetupHttpResponse(HttpStatusCode.OK, expectedData);
        var queryParams = new Dictionary<string, string>
        {
            ["filter"] = "active",
            ["limit"] = "10"
        };

        HttpRequestMessage? capturedRequest = null;
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, ct) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedData))
            });

        // Act
        await _client.GetAsync<TestResponse>("/test", queryParams, CancellationToken.None);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.RequestUri!.Query.Should().Contain("filter=active");
        capturedRequest.RequestUri.Query.Should().Contain("limit=10");
    }

    [Fact]
    public async Task PostAsync_SuccessfulRequest_ReturnsData()
    {
        // Arrange
        var requestData = new TestRequest { Name = "Test" };
        var expectedResponse = new TestResponse { Id = "123", Name = "Test" };
        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.PostAsync<TestRequest, TestResponse>("/test", requestData, null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("123");
        result.Name.Should().Be("Test");
    }

    [Fact]
    public async Task PutAsync_SuccessfulRequest_ReturnsData()
    {
        // Arrange
        var requestData = new TestRequest { Name = "Updated" };
        var expectedResponse = new TestResponse { Id = "123", Name = "Updated" };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.PutAsync<TestRequest, TestResponse>("/test/123", requestData, null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("123");
        result.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task DeleteAsync_SuccessfulRequest_Completes()
    {
        // Arrange
        SetupHttpResponse<object>(HttpStatusCode.NoContent, null);

        // Act
        await _client.DeleteAsync("/test/123", null, CancellationToken.None);

        // Assert - No exception thrown
    }

    [Fact]
    public async Task GetAsync_NotFoundError_ThrowsNotFoundException()
    {
        // Arrange
        var errorResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = new StringContent("{\"error\":\"Not found\"}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(errorResponse);

        // Act & Assert
        await Assert.ThrowsAsync<AllegroNotFoundException>(() => 
            _client.GetAsync<TestResponse>("/test/999", null, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_BadRequestError_ThrowsBadRequestException()
    {
        // Arrange
        var errorResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("{\"errors\":[{\"message\":\"Invalid input\"}]}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(errorResponse);

        // Act & Assert
        await Assert.ThrowsAsync<AllegroBadRequestException>(() => 
            _client.GetAsync<TestResponse>("/test", null, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_UnauthorizedError_ThrowsAuthenticationException()
    {
        // Arrange
        var errorResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Content = new StringContent("{\"error\":\"Unauthorized\"}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(errorResponse);

        // Act & Assert
        await Assert.ThrowsAsync<AllegroAuthenticationException>(() => 
            _client.GetAsync<TestResponse>("/test", null, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_ForbiddenError_ThrowsAuthorizationException()
    {
        // Arrange
        var errorResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Forbidden,
            Content = new StringContent("{\"error\":\"Forbidden\"}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(errorResponse);

        // Act & Assert
        await Assert.ThrowsAsync<AllegroAuthorizationException>(() => 
            _client.GetAsync<TestResponse>("/test", null, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_RateLimitError_ThrowsRateLimitException()
    {
        // Arrange
        var errorResponse = new HttpResponseMessage
        {
            StatusCode = (HttpStatusCode)429,
            Content = new StringContent("{\"error\":\"Too many requests\"}")
        };
        errorResponse.Headers.Add("Retry-After", "60");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(errorResponse);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AllegroRateLimitException>(() => 
            _client.GetAsync<TestResponse>("/test", null, CancellationToken.None));
        
        exception.RetryAfterSeconds.Should().Be(60);
    }

    [Fact]
    public async Task GetRawAsync_ReturnsHttpResponseMessage()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ByteArrayContent(new byte[] { 1, 2, 3 })
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetRawAsync("/test/file", null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // Test helper classes
    private class TestRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private class TestResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
