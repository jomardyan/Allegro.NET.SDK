using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Images;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class ImageClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly ImageClient _client;

    public ImageClientTests()
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
        _client = new ImageClient(_allegroHttpClient, "https://upload.allegro.pl");
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
    public async Task UploadImageFromUrlAsync_WithValidUrl_ReturnsResponse()
    {
        // Arrange
        var imageUrl = "https://example.com/image.jpg";
        var expectedResponse = new ImageUploadResponse
        {
            Location = "https://allegro.pl/images/uploaded-123.jpg"
        };
        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.UploadImageFromUrlAsync(imageUrl);

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().Be("https://allegro.pl/images/uploaded-123.jpg");
    }

    [Fact]
    public async Task UploadImageAsync_WithValidBytes_ReturnsResponse()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header
        var expectedResponse = new ImageUploadResponse
        {
            Location = "https://allegro.pl/images/uploaded-456.jpg"
        };
        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.UploadImageAsync(imageBytes);

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().Be("https://allegro.pl/images/uploaded-456.jpg");
    }

    [Fact]
    public async Task UploadImageFromStreamAsync_WithValidStream_ReturnsResponse()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        using var stream = new MemoryStream(imageBytes);
        var expectedResponse = new ImageUploadResponse
        {
            Location = "https://allegro.pl/images/uploaded-789.jpg"
        };
        SetupHttpResponse(HttpStatusCode.Created, expectedResponse);

        // Act
        var result = await _client.UploadImageFromStreamAsync(stream);

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().Be("https://allegro.pl/images/uploaded-789.jpg");
    }

    [Fact]
    public async Task UploadImageFromUrlAsync_WithNullRequest_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.UploadImageFromUrlAsync(null!));
    }

    [Fact]
    public async Task UploadImageAsync_WithNullBytes_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.UploadImageAsync(null!));
    }

    [Fact]
    public async Task UploadImageFromStreamAsync_WithNullStream_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _client.UploadImageFromStreamAsync(null!));
    }
}
