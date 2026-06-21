# Unofficial Allegro .NET SDK

[![NuGet](https://img.shields.io/nuget/v/AllegroApi.svg)](https://www.nuget.org/packages/AllegroApi/)
[![Downloads](https://img.shields.io/nuget/dt/AllegroApi.svg)](https://www.nuget.org/packages/AllegroApi/)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![CI](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml)
[![Build and Test (Multi-OS)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml)

A modern .NET client library for the [Allegro REST API](https://developer.allegro.pl/). This SDK provides strongly-typed access to 240+ API endpoints across 36 specialized clients, covering 100% of the documented Allegro marketplace platform.

> **Note:** This is an unofficial, community-maintained SDK. It is not officially endorsed or supported by Allegro.

## Key Features

**Comprehensive Coverage** - Access to 240+ API endpoints organized into 36 specialized clients covering offers, orders, fulfillment, shipping, payments, and more.

**Developer Experience** - Strongly typed requests and responses with full IntelliSense support, making integration straightforward and reducing errors.

**Modern Async** - All API calls use async/await patterns with cancellation token support for efficient resource usage.

**Production Ready** - Automatic retry logic with exponential backoff, 11 specialized exception types, rate limit handling, and 185 unit tests.

**Multi-Environment** - Switch between production and sandbox environments without code changes.

## Installation

```bash
dotnet add package AllegroApi
```

Or via Package Manager:

```powershell
Install-Package AllegroApi
```

## Quick Start

```csharp
using AllegroApi;

// Production environment
var client = AllegroApiClient.CreateProduction("your-access-token");

// Sandbox environment (for testing)
var sandboxClient = AllegroApiClient.CreateSandbox("your-sandbox-token");

// Get categories
var categories = await client.Categories.GetCategoriesAsync();

// Search products
var products = await client.Products.SearchProductsByPhraseAsync("laptop");

// Get orders
var orders = await client.Orders.GetOrdersAsync(new OrderSearchParams
{
    Status = "READY_FOR_PROCESSING"
});
```

## API Coverage (267/267 Documented Endpoints = 100%)

| Category | Methods | Status |
|----------|---------|--------|
| Offer Management | 17 | Complete |
| Products | 5 | Complete |
| Categories | 4 | Complete |
| Orders | 7 | Complete |
| Fulfillment | 17 | Complete - ASN, Stock, Parcels, Tax IDs |
| Images & Attachments | 6 | Complete |
| Shipping & Delivery | 7 | Complete |
| After-Sales Services | 12 | Complete |
| Payments | 2 | Complete |
| Billing | 3 | Complete |
| Messaging | 5 | Complete |
| User Ratings | 5 | Complete |
| Disputes & Attachments | 7 | Complete |
| Points of Service | 5 | Complete |
| Shipment Management | 13 | Complete |
| Customer Returns | 3 | Complete |
| Batch Operations | 9 | Complete |
| Listing & Discovery | 4 | Complete |
| Badge Campaigns | 6 | Complete |
| Classifieds | 4 | Complete |
| Advanced Features | 30+ | Variants, Tags, Bundles, Services |
| EU Compliance | 10 | Responsible Persons/Producers (GPSR) |

## Core Capabilities

### Offer Management
```csharp
// Create, update, delete offers
await client.Offers.CreateProductOfferAsync(request);
await client.Offers.UpdateOfferAsync(offerId, request);
await client.Offers.DeleteOfferAsync(offerId);

// Search and filter offers
await client.Offers.SearchOffersAsync(new OfferSearchParams
{
    Name = "iPhone",
    PublicationStatus = new[] { "ACTIVE" }
});
```

### Order Processing
```csharp
// Get orders with filters
var orders = await client.Orders.GetOrdersAsync(new OrderSearchParams
{
    Status = "READY_FOR_PROCESSING"
});

// Update fulfillment status
await client.Orders.UpdateFulfillmentStatusAsync(orderId, request);
```

### Fulfillment
```csharp
// Create Advance Ship Notice
var asn = await client.Fulfillment.CreateAdvanceShipNoticeAsync(asnRequest);

// Submit for processing
await client.Fulfillment.SubmitAdvanceShipNoticeAsync(asn.Id, submitCommand);

// Check stock levels
var stock = await client.Fulfillment.GetFulfillmentStockAsync(limit: 50);
```

## Error Handling

The library provides 11 specialized exception types:

```csharp
try
{
    var offer = await client.Offers.GetProductOfferAsync(offerId);
}
catch (AllegroNotFoundException)
{
    // 404 - Resource not found
}
catch (AllegroBadRequestException ex)
{
    // 400 - Validation errors
    foreach (var error in ex.ValidationErrors)
    {
        Console.WriteLine($"{error.Field}: {error.Message}");
    }
}
catch (AllegroRateLimitException ex)
{
    // 429 - Rate limit exceeded
    await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds));
}
catch (AllegroAuthenticationException)
{
    // 401 - Invalid or expired token
}
```

**Exception Types:** `AllegroAuthenticationException` (401), `AllegroAuthorizationException` (403), `AllegroNotFoundException` (404), `AllegroBadRequestException` (400), `AllegroUnprocessableEntityException` (422), `AllegroConflictException` (409), `AllegroRateLimitException` (429), `AllegroServerException` (5xx), `AllegroNetworkException`, `AllegroTimeoutException`.

## Configuration

```csharp
var options = new AllegroApiOptions
{
    AccessToken = "your-token",
    BaseUrl = "https://api.allegro.pl",
    TimeoutSeconds = 100,
    MaxRetryAttempts = 3,
    RetryDelayMilliseconds = 1000,
    AcceptLanguage = "en-US" // pl-PL, en-US, uk-UA, cs-CZ, sk-SK, hu-HU
};
var client = new AllegroApiClient(options);
```

## Requirements

- .NET 8.0 or later
- Allegro API credentials - available at [developer.allegro.pl](https://developer.allegro.pl/)

## Links

- **NuGet Package:** https://www.nuget.org/packages/AllegroApi/
- **GitHub Repository:** https://github.com/jomardyan/Allegro.NET.SDK
- **Allegro Developer Portal:** https://developer.allegro.pl/
- **Report Issues:** https://github.com/jomardyan/Allegro.NET.SDK/issues

## License

GPL-3.0-or-later — see [LICENSE](https://github.com/jomardyan/Allegro.NET.SDK/blob/main/LICENSE) for details.

## Contributing

Contributions are welcome. Please open an issue or pull request on GitHub.
