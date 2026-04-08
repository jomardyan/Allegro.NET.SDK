using AllegroApi.Exceptions;
using FluentAssertions;
using System.Net;
using Xunit;

namespace AllegroApi.Tests.Exceptions;

public class AllegroExceptionsTests
{
    [Fact]
    public void AllegroApiException_WithMessage_CreatesException()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new AllegroApiException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.StatusCode.Should().BeNull();
        exception.ErrorCode.Should().BeNull();
    }

    [Fact]
    public void AllegroApiException_WithStatusCode_StoresStatusCode()
    {
        // Arrange
        var message = "Test error";
        var statusCode = HttpStatusCode.BadRequest;

        // Act
        var exception = new AllegroApiException(message, statusCode);

        // Assert
        exception.StatusCode.Should().Be(statusCode);
    }

    [Fact]
    public void AllegroAuthenticationException_InheritsFromBaseException()
    {
        // Arrange & Act
        var exception = new AllegroAuthenticationException("Auth failed");

        // Assert
        exception.Should().BeOfType<AllegroAuthenticationException>();
        exception.Should().BeAssignableTo<AllegroApiException>();
    }

    [Fact]
    public void AllegroNotFoundException_WithResourceInfo_StoresResourceDetails()
    {
        // Arrange
        var resourceType = "Offer";
        var resourceId = "12345";

        // Act
        var exception = new AllegroNotFoundException(resourceType, resourceId);

        // Assert
        exception.ResourceType.Should().Be(resourceType);
        exception.ResourceId.Should().Be(resourceId);
        exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
        exception.Message.Should().Contain(resourceType);
        exception.Message.Should().Contain(resourceId);
    }

    [Fact]
    public void AllegroBadRequestException_WithValidationErrors_StoresErrors()
    {
        // Arrange
        var errors = new List<Models.Common.Error>
        {
            new Models.Common.Error { Code = "VALIDATION_ERROR", Message = "Invalid field" }
        };

        // Act
        var exception = new AllegroBadRequestException("Validation failed", errors);

        // Assert
        exception.ValidationErrors.Should().HaveCount(1);
        exception.ValidationErrors[0].Code.Should().Be("VALIDATION_ERROR");
        exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void AllegroRateLimitException_WithRetryAfter_CalculatesRetryDate()
    {
        // Arrange
        var retryAfterSeconds = 60;
        var beforeCreation = DateTime.UtcNow;

        // Act
        var exception = new AllegroRateLimitException("Rate limit exceeded", retryAfterSeconds);
        var afterCreation = DateTime.UtcNow;

        // Assert
        exception.RetryAfterSeconds.Should().Be(retryAfterSeconds);
        exception.RetryAfterDate.Should().NotBeNull();
        exception.RetryAfterDate.Should().BeOnOrAfter(beforeCreation.AddSeconds(retryAfterSeconds));
        exception.RetryAfterDate.Should().BeOnOrBefore(afterCreation.AddSeconds(retryAfterSeconds));
    }

    [Fact]
    public void AllegroConflictException_StoresConflictStatus()
    {
        // Arrange & Act
        var exception = new AllegroConflictException("Resource conflict", "DUPLICATE_ID");

        // Assert
        exception.StatusCode.Should().Be(HttpStatusCode.Conflict);
        exception.ErrorCode.Should().Be("DUPLICATE_ID");
    }

    [Fact]
    public void AllegroTimeoutException_StoresTimeout()
    {
        // Arrange
        var timeout = TimeSpan.FromSeconds(30);

        // Act
        var exception = new AllegroTimeoutException("Request timeout", timeout);

        // Assert
        exception.Timeout.Should().Be(timeout);
    }
}
