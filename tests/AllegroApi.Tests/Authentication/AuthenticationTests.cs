using AllegroApi;
using AllegroApi.Authentication;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AllegroApi.Tests.Authentication;

public class AuthenticationTests
{
    private static HttpClient HandlerReturning(Func<HttpRequestMessage, HttpResponseMessage> responder, out List<HttpRequestMessage> captured)
    {
        var requests = new List<HttpRequestMessage>();
        captured = requests;
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((req, _) =>
            {
                requests.Add(req);
                return Task.FromResult(responder(req));
            });
        return new HttpClient(handler.Object);
    }

    private static HttpResponseMessage TokenResponse(string token, int expiresIn = 3600)
        => new(HttpStatusCode.OK)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(new { access_token = token, token_type = "Bearer", expires_in = expiresIn }),
                Encoding.UTF8, "application/json")
        };

    [Fact]
    public async Task ClientCredentials_AcquiresAndCachesToken()
    {
        var http = HandlerReturning(_ => TokenResponse("tok-1"), out var requests);
        var provider = new ClientCredentialsTokenProvider("https://allegro.pl/auth/oauth/token", "id", "secret", http);

        var first = await provider.GetAccessTokenAsync();
        var second = await provider.GetAccessTokenAsync();

        first.Should().Be("tok-1");
        second.Should().Be("tok-1");
        requests.Should().HaveCount(1); // cached - only one token request
        requests[0].Headers.Authorization!.Scheme.Should().Be("Basic");
    }

    [Fact]
    public async Task ClientCredentials_ReacquiresAfterInvalidate()
    {
        var counter = 0;
        var http = HandlerReturning(_ => TokenResponse($"tok-{++counter}"), out var requests);
        var provider = new ClientCredentialsTokenProvider("https://allegro.pl/auth/oauth/token", "id", "secret", http);

        var first = await provider.GetAccessTokenAsync();
        provider.Invalidate();
        var second = await provider.GetAccessTokenAsync();

        first.Should().Be("tok-1");
        second.Should().Be("tok-2");
        requests.Should().HaveCount(2);
    }

    [Fact]
    public async Task ClientCredentials_ThrowsAuthenticationExceptionOnFailure()
    {
        var http = HandlerReturning(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("{\"error\":\"invalid_client\"}", Encoding.UTF8, "application/json")
        }, out _);
        var provider = new ClientCredentialsTokenProvider("https://allegro.pl/auth/oauth/token", "id", "secret", http);

        var act = () => provider.GetAccessTokenAsync();

        await act.Should().ThrowAsync<AllegroAuthenticationException>();
    }

    private sealed class SequenceTokenProvider : IAllegroTokenProvider
    {
        private int _index;
        private readonly string[] _tokens;
        public int InvalidateCount { get; private set; }
        public SequenceTokenProvider(params string[] tokens) => _tokens = tokens;
        public Task<string> GetAccessTokenAsync(CancellationToken ct = default)
            => Task.FromResult(_tokens[Math.Min(_index, _tokens.Length - 1)]);
        public void Invalidate() { InvalidateCount++; _index++; }
    }

    [Fact]
    public async Task AuthHandler_AttachesBearerToken()
    {
        var provider = new SequenceTokenProvider("the-token");
        HttpRequestMessage? seen = null;
        var inner = new Mock<HttpMessageHandler>();
        inner.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => seen = req)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var handler = new AllegroAuthenticationHandler(provider) { InnerHandler = inner.Object };
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.allegro.pl/") };

        await client.GetAsync("/sale/offers");

        seen!.Headers.Authorization!.Parameter.Should().Be("the-token");
    }

    [Fact]
    public async Task AuthHandler_RefreshesAndRetriesOnce_On401()
    {
        var provider = new SequenceTokenProvider("token-1", "token-2");
        var calls = 0;
        var tokensSeen = new List<string?>();
        var inner = new Mock<HttpMessageHandler>();
        inner.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((req, _) =>
            {
                calls++;
                tokensSeen.Add(req.Headers.Authorization?.Parameter);
                return Task.FromResult(new HttpResponseMessage(calls == 1 ? HttpStatusCode.Unauthorized : HttpStatusCode.OK));
            });

        var handler = new AllegroAuthenticationHandler(provider, autoRefresh: true) { InnerHandler = inner.Object };
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.allegro.pl/") };

        var response = await client.GetAsync("/sale/offers");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        calls.Should().Be(2);
        provider.InvalidateCount.Should().Be(1);
        tokensSeen.Should().Equal("token-1", "token-2");
    }

    [Fact]
    public void AddAllegroApi_RegistersTypedClient_WithStaticToken()
    {
        var services = new ServiceCollection();
        services.AddAllegroApi(o =>
        {
            o.AccessToken = "static-token";
            o.BaseUrl = "https://api.allegro.pl";
        });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<AllegroApiClient>();

        client.Should().NotBeNull();
        client.Offers.Should().NotBeNull();
        client.PriceAutomation.Should().NotBeNull();
        provider.GetRequiredService<IAllegroTokenProvider>().Should().BeOfType<StaticTokenProvider>();
    }

    [Fact]
    public void AddAllegroApi_UsesClientCredentialsProvider_WhenNoAccessToken()
    {
        var services = new ServiceCollection();
        services.AddAllegroApi(o =>
        {
            o.ClientId = "id";
            o.ClientSecret = "secret";
            o.BaseUrl = "https://api.allegro.pl";
        });

        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IAllegroTokenProvider>().Should().BeOfType<ClientCredentialsTokenProvider>();
        provider.GetRequiredService<AllegroApiClient>().Should().NotBeNull();
    }
}
