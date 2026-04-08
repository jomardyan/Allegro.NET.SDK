using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using FluentAssertions;
using Xunit;

namespace AllegroApi.Tests.Configuration;

public class AllegroApiOptionsTests
{
    [Fact]
    public void Validate_WithValidAccessToken_Succeeds()
    {
        // Arrange
        var options = new AllegroApiOptions
        {
            AccessToken = "valid-token"
        };

        // Act & Assert
        options.Invoking(o => o.Validate()).Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidClientCredentials_Succeeds()
    {
        // Arrange
        var options = new AllegroApiOptions
        {
            ClientId = "client-id",
            ClientSecret = "client-secret"
        };

        // Act & Assert
        options.Invoking(o => o.Validate()).Should().NotThrow();
    }

    [Fact]
    public void Validate_WithoutCredentials_ThrowsException()
    {
        // Arrange
        var options = new AllegroApiOptions();

        // Act & Assert
        options.Invoking(o => o.Validate())
            .Should().Throw<ArgumentException>()
            .WithMessage("*AccessToken*ClientId*ClientSecret*");
    }

    [Fact]
    public void Validate_WithEmptyBaseUrl_ThrowsException()
    {
        // Arrange
        var options = new AllegroApiOptions
        {
            BaseUrl = "",
            AccessToken = "token"
        };

        // Act & Assert
        options.Invoking(o => o.Validate())
            .Should().Throw<ArgumentException>()
            .WithMessage("*BaseUrl*");
    }

    [Fact]
    public void Validate_WithInvalidTimeout_ThrowsException()
    {
        // Arrange
        var options = new AllegroApiOptions
        {
            AccessToken = "token",
            TimeoutSeconds = -1
        };

        // Act & Assert
        options.Invoking(o => o.Validate())
            .Should().Throw<ArgumentException>()
            .WithMessage("*TimeoutSeconds*");
    }

    [Fact]
    public void ForEnvironment_Production_SetsCorrectUrls()
    {
        // Arrange
        var options = new AllegroApiOptions { AccessToken = "token" };

        // Act
        options.ForEnvironment(AllegroEnvironment.Production);

        // Assert
        options.BaseUrl.Should().Be("https://api.allegro.pl");
        options.TokenEndpoint.Should().Be("https://allegro.pl/auth/oauth/token");
    }

    [Fact]
    public void ForEnvironment_Sandbox_SetsCorrectUrls()
    {
        // Arrange
        var options = new AllegroApiOptions { AccessToken = "token" };

        // Act
        options.ForEnvironment(AllegroEnvironment.Sandbox);

        // Assert
        options.BaseUrl.Should().Be("https://api.allegro.pl.allegrosandbox.pl");
        options.TokenEndpoint.Should().Be("https://allegro.pl.allegrosandbox.pl/auth/oauth/token");
    }
}
