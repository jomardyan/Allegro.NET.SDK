# AllegroApi Test Suite

## Overview

This test suite provides comprehensive coverage for the AllegroApi library, testing all major components including API clients, HTTP communication, configuration, and exception handling.

**Test Statistics:**
- **Total Tests:** 143
- **Passing:** 143 (100%)
- **Test Files:** 22

## Test Structure

### Client Tests (`Clients/`)

**OfferManagementClientTests.cs** - Offer management operations including creating, editing, searching, deleting offers, price changes, and publication status changes.

**ProductClientTests.cs** - Product operations including search by EAN/phrase, product details, change proposals, and input validation.

**CategoryClientTests.cs** - Category operations including main/child categories, details, parameters, and category events.

**OrderManagementClientTests.cs** - Order management including search, details, events, carriers, fulfillment status updates, and invoice retrieval.

### HTTP Layer Tests (`Http/`)

**AllegroHttpClientTests.cs** - HTTP communication including GET/POST/PUT/DELETE operations, status code mapping (200, 201, 400, 401, 403, 404, 429, 500+), exception mapping, query parameter building, binary downloads, and Retry-After header handling.

### Configuration Tests (`Configuration/`)

**AllegroApiOptionsTests.cs** - Configuration validation including required fields, URL format, timeout range, and default values.

### Exception Tests (`Exceptions/`)

**AllegroExceptionsTests.cs** - All custom exception types: `AllegroBadRequestException`, `AllegroUnauthorizedException`, `AllegroForbiddenException`, `AllegroNotFoundException`, `AllegroConflictException`, `AllegroUnprocessableEntityException`, `AllegroRateLimitException`, `AllegroInternalServerException`, `AllegroServiceUnavailableException`, `AllegroGatewayTimeoutException`.

### Main Client Tests

**AllegroApiClientTests.cs** - Main client initialization, factory methods (production/sandbox), sub-client registration, and disposal pattern.

## Test Patterns

All tests follow the Arrange-Act-Assert (AAA) pattern:

```csharp
// Arrange - Set up test data and mocks
var expectedResponse = new SomeResponse { Id = "test-id" };
SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

// Act - Execute the method under test
var result = await _client.GetSomethingAsync("test-id");

// Assert - Verify the results
result.Should().NotBeNull();
result.Id.Should().Be("test-id");
```

### Mocking Strategy

Tests use `Moq` to mock `HttpMessageHandler`, allowing full control over HTTP responses without external dependencies. This validates request URIs, query parameters, and response deserialization in isolation.

## Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~ProductClientTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Dependencies

- **xUnit** - Test framework
- **Moq** - HTTP handler mocking
- **FluentAssertions** - Readable assertions
- **Microsoft.NET.Test.Sdk** - Test SDK
- **coverlet.collector** - Code coverage
