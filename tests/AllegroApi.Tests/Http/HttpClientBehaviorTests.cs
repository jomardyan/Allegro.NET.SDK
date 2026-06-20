using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Http;
using AllegroApi.Models.Images;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Http;

/// <summary>
/// Tests for transport-level behavior of <see cref="AllegroHttpClient"/>:
/// exception translation, retry semantics and binary uploads.
/// </summary>
public class HttpClientBehaviorTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly AllegroHttpClient _http;

    public HttpClientBehaviorTests()
    {
        _handler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_handler.Object) { BaseAddress = new Uri("https://api.allegro.pl/") };
        var options = new AllegroApiOptions
        {
            AccessToken = "test-token",
            BaseUrl = "https://api.allegro.pl/",
            MaxRetryAttempts = 2,
            RetryDelayMilliseconds = 1
        };
        _http = new AllegroHttpClient(httpClient, options);
    }

    public void Dispose()
    {
        _http?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SetupThrows(Exception ex)
    {
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(ex);
    }

    [Fact]
    public async Task NetworkFailure_IsTranslatedToAllegroNetworkException()
    {
        SetupThrows(new HttpRequestException("connection refused"));

        var act = () => _http.GetAsync<object>("/sale/offers");

        await act.Should().ThrowAsync<AllegroNetworkException>();
    }

    [Fact]
    public async Task Timeout_IsTranslatedToAllegroTimeoutException()
    {
        // HttpClient surfaces a timeout as a TaskCanceledException with no cancellation requested.
        SetupThrows(new TaskCanceledException("timed out"));

        var act = () => _http.GetAsync<object>("/sale/offers");

        await act.Should().ThrowAsync<AllegroTimeoutException>();
    }

    [Fact]
    public async Task CallerCancellation_PropagatesAsOperationCanceled()
    {
        SetupThrows(new HttpRequestException("connection refused"));
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var act = () => _http.GetAsync<object>("/sale/offers", null, cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task NetworkFailure_IsRetriedUpToConfiguredAttempts()
    {
        var calls = 0;
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback(() => calls++)
            .ThrowsAsync(new HttpRequestException("boom"));

        var act = () => _http.GetAsync<object>("/sale/offers");

        await act.Should().ThrowAsync<AllegroNetworkException>();
        // MaxRetryAttempts (2) + 1 initial attempt = 3 calls.
        calls.Should().Be(3);
    }

    [Fact]
    public async Task ImageBinaryUpload_PostsRawBytesToUploadHost()
    {
        HttpRequestMessage? captured = null;
        byte[]? sentBytes = null;
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
            {
                captured = req;
                sentBytes = req.Content!.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonSerializer.Serialize(new ImageUploadResponse { Location = "https://x/img.jpg" }), Encoding.UTF8, "application/json")
            });

        var client = new ImageClient(_http, "https://upload.allegro.pl");
        var data = new byte[] { 1, 2, 3, 4 };

        var result = await client.UploadImageAsync(data, "image/png");

        result.Should().NotBeNull();
        captured!.Method.Should().Be(HttpMethod.Post);
        captured.RequestUri!.ToString().Should().Be("https://upload.allegro.pl/sale/images");
        captured.Content!.Headers.ContentType!.MediaType.Should().Be("image/png");
        sentBytes.Should().Equal(data);
    }
}
