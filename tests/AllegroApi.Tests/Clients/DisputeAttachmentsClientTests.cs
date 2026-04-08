using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using AllegroApi.Models.Disputes;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class DisputeAttachmentsClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly DisputeAttachmentsClient _client;

    public DisputeAttachmentsClientTests()
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
        _client = new DisputeAttachmentsClient(_allegroHttpClient);
    }

    public void Dispose()
    {
        _allegroHttpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SetupHttpResponse<T>(HttpStatusCode statusCode, T? responseObject, Dictionary<string, string>? headers = null)
    {
        var json = JsonSerializer.Serialize(responseObject);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (headers != null)
        {
            foreach (var header in headers)
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    private void SetupBinaryResponse(HttpStatusCode statusCode, byte[] content)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new ByteArrayContent(content)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    [Fact]
    public async Task CreateAttachmentDeclarationAsync_ValidRequest_ReturnsIdAndUploadUrl()
    {
        // Arrange
        var declaration = new AttachmentDeclaration
        {
            FileName = "document.pdf",
            Size = 216772
        };
        var expectedResponse = new DisputeAttachmentId
        {
            Id = "eeed0007-4404-4176-a1eb-11d26f056c0d"
        };
        var uploadUrl = "https://upload.allegro.pl/sale/dispute-attachments/eeed0007-4404-4176-a1eb-11d26f056c0d";
        SetupHttpResponse(HttpStatusCode.Created, expectedResponse, new Dictionary<string, string>
        {
            { "Location", uploadUrl }
        });

        // Act
        var result = await _client.CreateAttachmentDeclarationAsync(declaration);

        // Assert
        result.Id.Should().NotBeNull();
        result.Id.Id.Should().Be("eeed0007-4404-4176-a1eb-11d26f056c0d");
        result.UploadUrl.Should().Be(uploadUrl);
    }

    [Fact]
    public async Task CreateAttachmentDeclarationAsync_NullDeclaration_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.CreateAttachmentDeclarationAsync(null!));
    }

    [Fact]
    public async Task UploadAttachmentAsync_ValidBinaryData_ReturnsTrue()
    {
        // Arrange
        var uploadUrl = "https://upload.allegro.pl/sale/dispute-attachments/test-id";
        var fileBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF header
        SetupBinaryResponse(HttpStatusCode.OK, Array.Empty<byte>());

        // Act
        var result = await _client.UploadAttachmentAsync(uploadUrl, fileBytes, "application/pdf");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UploadAttachmentAsync_NullUploadUrl_ThrowsArgumentNullException()
    {
        // Arrange
        var fileBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.UploadAttachmentAsync(null!, fileBytes, "application/pdf"));
    }

    [Fact]
    public async Task UploadAttachmentAsync_NullFileBytes_ThrowsArgumentNullException()
    {
        // Arrange
        var uploadUrl = "https://upload.allegro.pl/sale/dispute-attachments/test-id";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.UploadAttachmentAsync(uploadUrl, null!, "application/pdf"));
    }

    [Fact]
    public async Task UploadAttachmentAsync_NullContentType_ThrowsArgumentNullException()
    {
        // Arrange
        var uploadUrl = "https://upload.allegro.pl/sale/dispute-attachments/test-id";
        var fileBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.UploadAttachmentAsync(uploadUrl, fileBytes, null!));
    }

    [Fact]
    public async Task DownloadAttachmentAsync_ValidId_ReturnsByteArray()
    {
        // Arrange
        var attachmentId = "eeed0007-4404-4176-a1eb-11d26f056c0d";
        var expectedBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34 };
        SetupBinaryResponse(HttpStatusCode.OK, expectedBytes);

        // Act
        var result = await _client.DownloadAttachmentAsync(attachmentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public async Task DownloadAttachmentAsync_NullAttachmentId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _client.DownloadAttachmentAsync(null!));
    }
}
