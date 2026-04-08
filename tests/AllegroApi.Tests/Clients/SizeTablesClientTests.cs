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

public class SizeTablesClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly SizeTablesClient _client;

    public SizeTablesClientTests()
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
        _client = new SizeTablesClient(_allegroHttpClient);
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
        Assert.Throws<ArgumentNullException>(() => new SizeTablesClient(null!));
    }

    [Fact]
    public async Task GetSizeTableTemplatesAsync_ReturnsTemplates()
    {
        // Arrange
        var expectedResponse = new AllegroApi.Models.Common.SizeTableTemplatesResponse
        {
            Templates = new List<AllegroApi.Models.Common.SizeTableTemplateResponse>
            {
                new AllegroApi.Models.Common.SizeTableTemplateResponse
                {
                    Id = "template-1",
                    Name = "EU Shoe Sizes",
                    Headers = new List<AllegroApi.Models.Common.SizeTableHeader>
                    {
                        new AllegroApi.Models.Common.SizeTableHeader { Name = "EU" },
                        new AllegroApi.Models.Common.SizeTableHeader { Name = "UK" }
                    },
                    Values = new List<AllegroApi.Models.Common.SizeTableCells>
                    {
                        new AllegroApi.Models.Common.SizeTableCells { Cells = new List<string> { "36", "4" } }
                    }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetSizeTableTemplatesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Templates.Should().HaveCount(1);
        result.Templates[0].Id.Should().Be("template-1");
        result.Templates[0].Name.Should().Be("EU Shoe Sizes");
        result.Templates[0].Headers.Should().HaveCount(2);
    }
}
