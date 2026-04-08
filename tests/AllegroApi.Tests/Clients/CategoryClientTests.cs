using AllegroApi.Clients;
using AllegroApi.Http;
using AllegroApi.Models.Categories;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Clients;

public class CategoryClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly AllegroHttpClient _allegroHttpClient;
    private readonly CategoryClient _client;

    public CategoryClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.allegro.pl/")
        };
        var options = new AllegroApi.Configuration.AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/"
        };
        _allegroHttpClient = new AllegroHttpClient(httpClient, options);
        _client = new CategoryClient(_allegroHttpClient);
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
    public async Task GetCategoriesAsync_WithoutParent_ReturnsMainCategories()
    {
        // Arrange
        var expectedResponse = new CategoriesDto
        {
            Categories = new List<CategoryDto>
            {
                new CategoryDto { Id = "1", Name = "Electronics" },
                new CategoryDto { Id = "2", Name = "Fashion" }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Categories.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCategoriesAsync_WithParent_ReturnsChildCategories()
    {
        // Arrange
        var expectedResponse = new CategoriesDto
        {
            Categories = new List<CategoryDto>
            {
                new CategoryDto { Id = "10", Name = "Smartphones", Parent = new CategoryParent { Id = "1" } },
                new CategoryDto { Id = "11", Name = "Laptops", Parent = new CategoryParent { Id = "1" } }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCategoriesAsync("1");

        // Assert
        result.Should().NotBeNull();
        result!.Categories.Should().HaveCount(2);
        result.Categories.Should().AllSatisfy(c => c.Parent!.Id.Should().Be("1"));
    }

    [Fact]
    public async Task GetCategoryAsync_ReturnsCategory()
    {
        // Arrange
        var expectedCategory = new CategoryDto
        {
            Id = "123",
            Name = "Smartphones",
            Leaf = true,
            Options = new CategoryOptionsDto
            {
                Advertisement = false,
                ProductCreationEnabled = true
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedCategory);

        // Act
        var result = await _client.GetCategoryAsync("123");

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("123");
        result.Name.Should().Be("Smartphones");
        result.Leaf.Should().BeTrue();
    }

    [Fact]
    public async Task GetCategoryAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCategoryAsync(null!));
    }

    [Fact]
    public async Task GetCategoryParametersAsync_ReturnsParameters()
    {
        // Arrange
        var expectedResponse = new CategoryParameterList
        {
            Parameters = new List<CategoryParameterDto>
            {
                new CategoryParameterDto 
                { 
                    Id = "param1", 
                    Name = "Brand", 
                    Required = true 
                },
                new CategoryParameterDto 
                { 
                    Id = "param2", 
                    Name = "Color", 
                    Required = false 
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCategoryParametersAsync("123");

        // Assert
        result.Should().NotBeNull();
        result!.Parameters.Should().HaveCount(2);
        result.Parameters![0].Id.Should().Be("param1");
        result.Parameters[0].Required.Should().BeTrue();
    }

    [Fact]
    public async Task GetCategoryParametersAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.GetCategoryParametersAsync(null!));
    }

    [Fact]
    public async Task GetCategoryEventsAsync_ReturnsEvents()
    {
        // Arrange
        var expectedResponse = new CategoryEventsResponse
        {
            Events = new List<CategoryEvent>
            {
                new CategoryEvent
                {
                    Id = "event1",
                    Type = "CATEGORY_CREATED",
                    Category = new CategoryEventDetails { Id = "123", Name = "New Category" }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCategoryEventsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Events.Should().HaveCount(1);
        result.Events![0].Type.Should().Be("CATEGORY_CREATED");
    }

    [Fact]
    public async Task GetCategoryEventsAsync_WithFilters_ReturnsFilteredEvents()
    {
        // Arrange
        var expectedResponse = new CategoryEventsResponse
        {
            Events = new List<CategoryEvent>
            {
                new CategoryEvent
                {
                    Id = "event2",
                    Type = "CATEGORY_RENAMED",
                    Category = new CategoryEventDetails { Id = "456", Name = "Renamed Category" }
                }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _client.GetCategoryEventsAsync(
            from: "event1",
            limit: 10,
            types: new List<string> { "CATEGORY_RENAMED" });

        // Assert
        result.Should().NotBeNull();
        result.Events.Should().HaveCount(1);
    }
}
