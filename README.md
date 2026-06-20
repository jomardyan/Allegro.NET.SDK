# Unofficial Allegro .NET SDK

[![NuGet](https://img.shields.io/nuget/v/AllegroApi.svg)](https://www.nuget.org/packages/AllegroApi/)
[![Downloads](https://img.shields.io/nuget/dt/AllegroApi.svg)](https://www.nuget.org/packages/AllegroApi/)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](LICENSE)  
[![CI](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml)
[![Build and Test (Multi-OS)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml)
[![Publish to NuGet](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml)

A modern .NET client library for integrating with the Allegro marketplace API. This SDK provides strongly-typed access to over 170 API endpoints, covering everything from listing management to order fulfillment.

> **Note:** This is an unofficial, community-maintained SDK. It is not officially endorsed or supported by Allegro.

---

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Sandbox Testing](#sandbox-testing)
- [API Coverage](#api-coverage)
- [Usage Examples](#usage-examples)
- [Error Handling](#error-handling)
- [Configuration](#configuration)
- [Testing](#testing)
- [Contributing](#contributing)
- [Release Notes](#release-notes)
- [License](#license)

---

## Features

### Comprehensive API Support

This library implements 230+ endpoints across 36 specialized clients, providing access to all documented Allegro API functionality:

- **Listing Management** - Create, update, and manage product offers with full support for variants, translations, and attachments
- **Order Processing** - Handle orders from receipt through fulfillment, including invoicing and returns
- **Fulfillment Integration** - Complete support for Allegro Fulfillment with ASN management and inventory tracking
- **Category System** - Navigate the category tree and manage product parameters
- **Pricing & Promotions** - Calculate fees, manage Allegro Prices, and run discount campaigns
- **Shipping** - Configure shipping rates and delivery methods
- **Customer Service** - Handle messaging, disputes, and after-sales support
- **Batch Operations** - Update prices and quantities across multiple listings efficiently
- **Marketplace Tools** - Manage badges, classifieds, and loyalty programs
- **Compliance** - EU GPSR support for responsible persons and producers

### Built for Production

- **Strongly Typed** - Full IntelliSense support with comprehensive XML documentation
- **Modern Async** - All API calls use async/await patterns with cancellation token support
- **Resilient** - Automatic retries with exponential backoff and intelligent rate limit handling
- **Well Tested** - 143 unit tests ensure reliability
- **Multiple Environments** - Seamless switching between production and sandbox
- **Error Handling** - Detailed exception types make debugging straightforward

---

## Installation

### NuGet Package Manager
```bash
Install-Package AllegroApi
```

### .NET CLI
```bash
dotnet add package AllegroApi
```

### Package Reference
```xml
<PackageReference Include="AllegroApi" Version="2.0.0" />
```

**Requirements:** .NET 8.0 or higher

---

## Quick Start

### Installation and Setup

```csharp
using AllegroApi;

// Production environment
var client = AllegroApiClient.CreateProduction("your-access-token");

// Sandbox environment (for testing)
var sandboxClient = AllegroApiClient.CreateSandbox("your-sandbox-token");

// Custom configuration
var options = new AllegroApiOptions
{
    AccessToken = "your-access-token",
    BaseUrl = "https://api.allegro.pl",
    TimeoutSeconds = 100,
    MaxRetryAttempts = 3,
    EnableLogging = true
};
var customClient = new AllegroApiClient(options);
```

### Dependency Injection (recommended for ASP.NET Core / hosts)

```csharp
// Registers AllegroApiClient as a typed client backed by IHttpClientFactory.
services.AddAllegroApi(options =>
{
    options.AccessToken = "your-access-token";
    // …or use the client-credentials grant (auto-acquired & refreshed):
    // options.ClientId = "your-client-id";
    // options.ClientSecret = "your-client-secret";
});

// Then inject AllegroApiClient anywhere:
public class MyService(AllegroApiClient allegro) { /* ... */ }
```

### OAuth2 client credentials (application access)

```csharp
// No pre-acquired token needed — the SDK obtains and refreshes one automatically.
var options = new AllegroApiOptions
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret"
};
using var client = new AllegroApiClient(options);
```

### Basic Operations

```csharp
// Get categories
var categories = await client.Categories.GetCategoriesAsync();

// Search products
var products = await client.Products.SearchProductsByPhraseAsync("laptop");

// Create an offer
var offer = await client.Offers.CreateProductOfferAsync(offerRequest);

// Get orders
var orders = await client.Orders.GetOrdersAsync(new OrderSearchParams 
{ 
    Status = "READY_FOR_PROCESSING" 
});

// Upload image
var imageUrl = "https://example.com/product.jpg";
var image = await client.Images.UploadImageFromUrlAsync(imageUrl);
```

---

## Sandbox Testing

AllegroApi includes complete support for Allegro's sandbox environment, letting you test your integration safely without affecting production data or real transactions.

### Using the Sandbox

```csharp
using AllegroApi;

// Create sandbox client
var sandboxClient = AllegroApiClient.CreateSandbox("your-sandbox-access-token");

// Use exactly like production - all 36 clients available
var categories = await sandboxClient.Categories.GetCategoriesAsync();
var products = await sandboxClient.Products.SearchProductsByPhraseAsync("test");
```

### Environment Comparison

| Feature | Production | Sandbox |
|---------|-----------|---------|
| **API Base URL** | `https://api.allegro.pl` | `https://api.allegro.pl.allegrosandbox.pl` |
| **Auth URL** | `https://allegro.pl/auth/oauth/token` | `https://allegro.pl.allegrosandbox.pl/auth/oauth/token` |
| **Image Upload** | `https://upload.allegro.pl` | `https://upload.allegro.pl.allegrosandbox.pl` |
| **Real Money** | ✅ Yes | ❌ No (test mode) |
| **Real Orders** | ✅ Yes | ❌ No (simulated) |
| **Rate Limits** | Full limits | Same limits |
| **All 35 Clients** | ✅ Available | ✅ Available |

### Getting Sandbox Credentials

1. **Register Sandbox Application**
   - Visit: https://apps.developer.allegro.pl.allegrosandbox.pl/
   - Create a new app to get Client ID and Client Secret
   - Configure OAuth redirect URI

2. **Generate Access Token**
   
   **Option A: Using OAuth2 Authorization Code Flow (Recommended)**
   
   ```csharp
   // Step 1: Generate authorization URL
   var clientId = "your-sandbox-client-id";
   var redirectUri = "http://localhost:5000/callback";
   var authUrl = $"https://allegro.pl.allegrosandbox.pl/auth/oauth/authorize?" +
                 $"response_type=code&client_id={clientId}&redirect_uri={redirectUri}";
   
   // Step 2: User visits authUrl and authorizes
   // Step 3: Exchange authorization code for access token
   using var httpClient = new HttpClient();
   var tokenRequest = new FormUrlEncodedContent(new[]
   {
       new KeyValuePair<string, string>("grant_type", "authorization_code"),
       new KeyValuePair<string, string>("code", "authorization-code-from-callback"),
       new KeyValuePair<string, string>("redirect_uri", redirectUri)
   });
   
   var credentials = Convert.ToBase64String(
       Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
   httpClient.DefaultRequestHeaders.Authorization = 
       new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
   
   var response = await httpClient.PostAsync(
       "https://allegro.pl.allegrosandbox.pl/auth/oauth/token", 
       tokenRequest);
   var tokenResponse = await response.Content.ReadAsStringAsync();
   // Parse JSON to get access_token and refresh_token
   ```
   
   **Option B: Using Client Credentials Flow (For Testing)**
   
   ```csharp
   using var httpClient = new HttpClient();
   var tokenRequest = new FormUrlEncodedContent(new[]
   {
       new KeyValuePair<string, string>("grant_type", "client_credentials")
   });
   
   var credentials = Convert.ToBase64String(
       Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
   httpClient.DefaultRequestHeaders.Authorization = 
       new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
   
   var response = await httpClient.PostAsync(
       "https://allegro.pl.allegrosandbox.pl/auth/oauth/token", 
       tokenRequest);
   var tokenResponse = await response.Content.ReadAsStringAsync();
   // Parse JSON to get access_token
   ```

### Configuration Options

#### Method 1: Factory Methods (Recommended)

```csharp
// Production
var prodClient = AllegroApiClient.CreateProduction("prod-token");

// Sandbox
var sandboxClient = AllegroApiClient.CreateSandbox("sandbox-token");
```

#### Method 2: Manual Configuration

```csharp
using AllegroApi.Configuration;

var sandboxOptions = new AllegroApiOptions
{
    AccessToken = "your-sandbox-token",
    TimeoutSeconds = 100,
    MaxRetryAttempts = 3,
    EnableLogging = true,
    AcceptLanguage = "en-US"
}.ForEnvironment(AllegroEnvironment.Sandbox);

var client = new AllegroApiClient(sandboxOptions);
```

#### Method 3: Custom URLs

```csharp
var customOptions = new AllegroApiOptions
{
    AccessToken = "your-token",
    BaseUrl = "https://api.allegro.pl.allegrosandbox.pl",
    TokenEndpoint = "https://allegro.pl.allegrosandbox.pl/auth/oauth/token"
};

var client = new AllegroApiClient(customOptions);
```

### Testing Best Practices

#### 1. Environment-Specific Configuration

```csharp
public class AllegroClientFactory
{
    public static AllegroApiClient Create(bool useSandbox = false)
    {
        var token = useSandbox 
            ? Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN")
            : Environment.GetEnvironmentVariable("ALLEGRO_PRODUCTION_TOKEN");
        
        return useSandbox 
            ? AllegroApiClient.CreateSandbox(token)
            : AllegroApiClient.CreateProduction(token);
    }
}

// Usage
var client = AllegroClientFactory.Create(useSandbox: true);
```

#### 2. Integration Tests with Sandbox

```csharp
using Xunit;
using AllegroApi;

public class AllegroIntegrationTests
{
    private readonly AllegroApiClient _client;
    
    public AllegroIntegrationTests()
    {
        var sandboxToken = Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN");
        _client = AllegroApiClient.CreateSandbox(sandboxToken);
    }
    
    [Fact]
    public async Task Can_SearchProducts_InSandbox()
    {
        // Arrange
        var searchPhrase = "laptop";
        
        // Act
        var products = await _client.Products.SearchProductsByPhraseAsync(searchPhrase);
        
        // Assert
        Assert.NotNull(products);
        Assert.NotEmpty(products.Products);
    }
    
    [Fact]
    public async Task Can_CreateTestOffer_InSandbox()
    {
        // Arrange
        var offerRequest = new SaleProductOfferRequestV1
        {
            // Test offer data
        };
        
        // Act
        var offer = await _client.Offers.CreateProductOfferAsync(offerRequest);
        
        // Assert
        Assert.NotNull(offer);
        Assert.NotEmpty(offer.Id);
    }
    
    [Fact]
    public async Task Can_HandleRateLimits_InSandbox()
    {
        // Test rate limit handling
        var tasks = Enumerable.Range(0, 100)
            .Select(_ => _client.Categories.GetCategoriesAsync());
        
        // Should not throw - rate limit handling built-in
        await Task.WhenAll(tasks);
    }
}
```

#### 3. Feature Toggle Pattern

```csharp
public class OfferService
{
    private readonly AllegroApiClient _client;
    private readonly bool _isProduction;
    
    public OfferService(IConfiguration config)
    {
        _isProduction = config.GetValue<bool>("Allegro:UseProduction");
        var token = config.GetValue<string>(
            _isProduction ? "Allegro:ProductionToken" : "Allegro:SandboxToken");
        
        _client = _isProduction 
            ? AllegroApiClient.CreateProduction(token)
            : AllegroApiClient.CreateSandbox(token);
    }
    
    public async Task<string> CreateOfferAsync(SaleProductOfferRequestV1 request)
    {
        if (!_isProduction)
        {
            // Add test data prefix in sandbox
            request.Name = $"[TEST] {request.Name}";
        }
        
        var offer = await _client.Offers.CreateProductOfferAsync(request);
        return offer.Id;
    }
}
```

### Sandbox-Specific Features

#### Test Data Management

```csharp
// Sandbox allows unlimited test offers
var sandboxClient = AllegroApiClient.CreateSandbox(token);

// Create test offers for testing
for (int i = 0; i < 100; i++)
{
    var testOffer = new SaleProductOfferRequestV1
    {
        Name = $"Test Offer {i}",
        // ... test data
    };
    
    await sandboxClient.Offers.CreateProductOfferAsync(testOffer);
}

// Clean up test data
var offers = await sandboxClient.Offers.GetOffersAsync();
foreach (var offer in offers.Offers)
{
    await sandboxClient.Offers.DeleteOfferAsync(offer.Id);
}
```

#### Error Scenario Testing

```csharp
// Test authentication error
try
{
    var badClient = AllegroApiClient.CreateSandbox("invalid-token");
    await badClient.Categories.GetCategoriesAsync();
}
catch (AllegroAuthenticationException ex)
{
    Console.WriteLine($"Expected: {ex.Message}");
}

// Test rate limit handling
var client = AllegroApiClient.CreateSandbox(validToken);
for (int i = 0; i < 1000; i++)
{
    try
    {
        await client.Categories.GetCategoriesAsync();
    }
    catch (AllegroRateLimitException ex)
    {
        Console.WriteLine($"Rate limited - retry after {ex.RetryAfterSeconds}s");
        await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds));
    }
}
```

### Switching Between Environments

```csharp
public class MultiEnvironmentClient
{
    private readonly AllegroApiClient _prodClient;
    private readonly AllegroApiClient _sandboxClient;
    
    public MultiEnvironmentClient(string prodToken, string sandboxToken)
    {
        _prodClient = AllegroApiClient.CreateProduction(prodToken);
        _sandboxClient = AllegroApiClient.CreateSandbox(sandboxToken);
    }
    
    public AllegroApiClient GetClient(bool useSandbox)
    {
        return useSandbox ? _sandboxClient : _prodClient;
    }
    
    // Test in sandbox, then promote to production
    public async Task<string> CreateOfferWithValidation(
        SaleProductOfferRequestV1 request, 
        bool validateInSandboxFirst = true)
    {
        if (validateInSandboxFirst)
        {
            // Test in sandbox first
            try
            {
                var sandboxOffer = await _sandboxClient.Offers
                    .CreateProductOfferAsync(request);
                Console.WriteLine($"✅ Sandbox validation passed: {sandboxOffer.Id}");
                
                // Clean up sandbox test
                await _sandboxClient.Offers.DeleteOfferAsync(sandboxOffer.Id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Sandbox validation failed: {ex.Message}", ex);
            }
        }
        
        // Create in production
        var prodOffer = await _prodClient.Offers.CreateProductOfferAsync(request);
        return prodOffer.Id;
    }
}
```

### Troubleshooting Sandbox Issues

#### Common Issues

1. **"401 Unauthorized" Error**
   ```csharp
   // ❌ Wrong: Using production token with sandbox
   var client = AllegroApiClient.CreateSandbox(productionToken);
   
   // ✅ Correct: Use sandbox-specific token
   var client = AllegroApiClient.CreateSandbox(sandboxToken);
   ```

2. **"404 Not Found" - Wrong Base URL**
   ```csharp
   // ❌ Wrong: Manual config with wrong URL
   var options = new AllegroApiOptions
   {
       AccessToken = sandboxToken,
       BaseUrl = "https://api.allegro.pl" // Production URL!
   };
   
   // ✅ Correct: Use factory method or ForEnvironment
   var client = AllegroApiClient.CreateSandbox(sandboxToken);
   ```

3. **Token Expiration**
   ```csharp
   // Implement token refresh
   public class TokenManager
   {
       private string _accessToken;
       private DateTime _expiresAt;
       
       public async Task<string> GetValidTokenAsync()
       {
           if (DateTime.UtcNow >= _expiresAt.AddMinutes(-5))
           {
               await RefreshTokenAsync();
           }
           return _accessToken;
       }
       
       private async Task RefreshTokenAsync()
       {
           // Use refresh_token to get new access_token
           using var httpClient = new HttpClient();
           var request = new FormUrlEncodedContent(new[]
           {
               new KeyValuePair<string, string>("grant_type", "refresh_token"),
               new KeyValuePair<string, string>("refresh_token", _refreshToken)
           });
           
           var credentials = Convert.ToBase64String(
               Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
           httpClient.DefaultRequestHeaders.Authorization = 
               new AuthenticationHeaderValue("Basic", credentials);
           
           var response = await httpClient.PostAsync(
               "https://allegro.pl.allegrosandbox.pl/auth/oauth/token", 
               request);
           var json = await response.Content.ReadAsStringAsync();
           // Update _accessToken and _expiresAt
       }
   }
   ```

### Environment Variables Setup

```bash
# .env file for development
ALLEGRO_PRODUCTION_TOKEN=your-production-token
ALLEGRO_SANDBOX_TOKEN=your-sandbox-token
ALLEGRO_USE_SANDBOX=true

# Linux/macOS
export ALLEGRO_SANDBOX_TOKEN="your-sandbox-token"
export ALLEGRO_USE_SANDBOX=true

# Windows PowerShell
$env:ALLEGRO_SANDBOX_TOKEN="your-sandbox-token"
$env:ALLEGRO_USE_SANDBOX="true"

# Windows Command Prompt
set ALLEGRO_SANDBOX_TOKEN=your-sandbox-token
set ALLEGRO_USE_SANDBOX=true
```

### ASP.NET Core Integration

```csharp
// Program.cs / Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<AllegroApiClient>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var useSandbox = config.GetValue<bool>("Allegro:UseSandbox");
        var token = config.GetValue<string>(
            useSandbox ? "Allegro:SandboxToken" : "Allegro:ProductionToken");
        
        return useSandbox 
            ? AllegroApiClient.CreateSandbox(token)
            : AllegroApiClient.CreateProduction(token);
    });
}

// appsettings.Development.json
{
  "Allegro": {
    "UseSandbox": true,
    "SandboxToken": "your-sandbox-token"
  }
}

// appsettings.Production.json
{
  "Allegro": {
    "UseSandbox": false,
    "ProductionToken": "your-production-token"
  }
}
```

### Sandbox Limitations

**Important Differences from Production:**

1. **No Real Payments** - Orders are simulated, no actual money transactions
2. **Limited Test Data** - Some categories may have fewer products
3. **Simulated Fulfillment** - Warehouse operations are mocked
4. **Email Notifications** - Not sent in sandbox (or sent to test addresses)
5. **Same Rate Limits** - Sandbox has the same API rate limits as production
6. **Data Persistence** - Sandbox data may be periodically reset by Allegro

### Resources

- **Sandbox Documentation:** https://developer.allegro.pl/tutorials/uwierzytelnianie-i-autoryzacja-zlq9e75GdIR#testowanie-w-srodowisku-sandbox
- **Sandbox Apps Console:** https://apps.developer.allegro.pl.allegrosandbox.pl/
- **Developer Forum:** https://allegro.pl/dla-sprzedajacych/forum/
- **API Reference:** https://developer.allegro.pl/documentation/

---

## API Coverage

AllegroApi implements all 266 documented Allegro REST API operations (100% coverage) through 36 specialized clients.

### Client Organization

<details>
<summary><b>Core Commerce (22 methods)</b></summary>

- **OfferManagementClient** - Create, edit, delete offers, translations, batch operations
- **ProductClient** - Product search, details, change proposals
- **OrderManagementClient** - Order search, details, fulfillment, customer returns
- **CategoryClient** - Categories, parameters, tax settings, events
</details>

<details>
<summary><b>Fulfillment & Warehouse (17 methods)</b></summary>

- **FulfillmentClient** - ASN management, stock, parcels, tax IDs, removal preferences
</details>

<details>
<summary><b>Pricing & Campaigns (14 methods)</b></summary>

- **PricingClient** - Fee calculations
- **AllegroPricesClient** - Allegro Prices consent, account participation, Alle Discount campaigns, offer subsidy commands
- **PriceAutomationClient** - Automatic pricing rules (create, read, update, delete, per-offer rules)
- **BatchOperationsClient** - Price automation commands
</details>

<details>
<summary><b>Shipping & Logistics (20 methods)</b></summary>

- **ShippingClient** - Shipping rates, delivery methods
- **ShipmentManagementClient** - Shipments, labels, protocols, pickups
- **PointsOfServiceClient** - Pickup locations CRUD
</details>

<details>
<summary><b>After-Sales & Compliance (30 methods)</b></summary>

- **AfterSalesClient** - Return policies, warranties, implied warranties
- **CustomerReturnsClient** - Returns management
- **RefundClaimsClient** - Refund claims CRUD
- **PostPurchaseIssuesClient** - Issue management
- **ResponsiblePersonsClient** - EU GPSR compliance
- **ResponsibleProducersClient** - Producer compliance
</details>

<details>
<summary><b>Communication (14 methods)</b></summary>

- **MessagingClient** - Buyer-seller messaging
- **DisputesClient** - Dispute management
- **DisputeAttachmentsClient** - Binary file handling
</details>

<details>
<summary><b>Marketplace Features (21 methods)</b></summary>

- **BadgesClient** - Badge campaigns and applications
- **ClassifiedsClient** - Classifieds packages
- **ListingClient** - Public offer search
- **SaleExtensionsClient** - Bundles, loyalty, turnover discount, additional services
- **AdvancedOffersClient** - Smart offers, variants, attachments
</details>

<details>
<summary><b>Account & Quality (11 methods)</b></summary>

- **AccountClient** - Profile, sales quality, Smart! classification
- **UsersClient** - User ratings CRUD
- **MarketplacesClient** - Marketplace information
</details>

<details>
<summary><b>Additional Services (16 methods)</b></summary>

- **ImageClient** - Image uploads
- **SizeTablesClient** - Size tables CRUD
- **ContactsClient** - Seller contacts CRUD
- **AdditionalEmailsClient** - Additional emails CRUD
- **MiscellaneousClient** - Charity, bidding, affiliate, compatibility lists, offer events
</details>

<details>
<summary><b>Financial Operations (9 methods)</b></summary>

- **PaymentsClient** - Payment operations
- **BillingClient** - Billing entries, invoices
</details>

---

## Usage Examples

### Offer Management

#### Create a Product Offer

```csharp
using AllegroApi.Models.Offers;
using AllegroApi.Models.Common;

var request = new SaleProductOfferRequestV1
{
    ProductSet = new List<ProductSet>
    {
        new ProductSet
        {
            Product = new ProductIdentifier { Id = "product-id" },
            Quantity = 1
        }
    },
    SellingMode = new SellingMode
    {
        Format = "BUY_NOW",
        Price = new Money { Amount = "99.99", Currency = "PLN" }
    },
    Stock = new Stock { Available = 10 },
    Publication = new Publication { Status = "ACTIVE" }
};

var response = await client.Offers.CreateProductOfferAsync(request);
Console.WriteLine($"Offer created: {response.Id}");
```

#### Batch Price Update

```csharp
var priceChanges = new OfferPriceChangeCommand
{
    Offers = new List<OfferPriceChange>
    {
        new OfferPriceChange 
        { 
            Id = "offer-1", 
            BuyNowPrice = new Money { Amount = "149.99", Currency = "PLN" } 
        },
        new OfferPriceChange 
        { 
            Id = "offer-2", 
            BuyNowPrice = new Money { Amount = "199.99", Currency = "PLN" } 
        }
    }
};

var command = await client.BatchOperations.ChangePricesAsync(priceChanges);
Console.WriteLine($"Command ID: {command.Id}");
```

### Order Management

```csharp
// Search orders
var orderParams = new OrderSearchParams
{
    Status = "READY_FOR_PROCESSING",
    BuyerLogin = "buyer123",
    Sort = "-boughtAt" // Sort by newest first
};

var orders = await client.Orders.GetOrdersAsync(orderParams);

foreach (var order in orders.CheckoutForms)
{
    Console.WriteLine($"Order {order.Id}: {order.Buyer.Email}");
    
    // Update fulfillment status
    await client.Orders.UpdateFulfillmentStatusAsync(order.Id, "SENT");
    
    // Get invoice
    var invoice = await client.Orders.GetOrderInvoiceAsync(order.Id);
}
```

### Fulfillment & Warehouse

```csharp
// Create Advance Ship Notice
var asnRequest = new CreateAdvanceShipNoticeRequest
{
    HandlingUnit = new HandlingUnit { Type = "PALLETS", Quantity = 2 },
    EstimatedDeliveryDate = DateTime.UtcNow.AddDays(3),
    Products = new List<FulfillmentProduct>
    {
        new FulfillmentProduct { Ean = "1234567890123", Quantity = 100 }
    }
};

var asn = await client.Fulfillment.CreateAdvanceShipNoticeAsync(asnRequest);

// Submit ASN
await client.Fulfillment.SubmitAdvanceShipNoticeAsync(asn.Id, submitCommand);

// Get fulfillment stock
var stock = await client.Fulfillment.GetFulfillmentStockAsync(limit: 50);
```

### Public Listing Search

```csharp
// Search public offers
var listings = await client.Listing.SearchOfferingsByPhraseAsync(
    phrase: "laptop gaming", 
    limit: 20
);

// Search within category
var categoryListings = await client.Listing.SearchOfferingsByCategoryAsync(
    categoryId: "12345",
    limit: 50
);

// Search by seller
var sellerListings = await client.Listing.SearchOfferingsBySellerAsync(
    sellerId: "seller-123"
);
```

### Badge Management

```csharp
// List available badge campaigns
var campaigns = await client.Badges.GetBadgeCampaignsAsync();

// Create badge application
var application = new BadgeApplicationRequest
{
    CampaignId = "campaign-123",
    Offers = new List<BadgeOfferCriteria>
    {
        new BadgeOfferCriteria { OfferId = "offer-1" },
        new BadgeOfferCriteria { OfferId = "offer-2" }
    }
};

var result = await client.Badges.CreateBadgeApplicationAsync(application);
```

### Classifieds Packages

```csharp
// Get packages for offer
var packages = await client.Classifieds.GetClassifiedPackagesAsync("offer-id");

// Assign package to offer
var assignRequest = new ClassifiedPackages
{
    BasePackage = new ClassifiedPackage { Id = "base-pkg-1" },
    ExtraPackages = new List<ClassifiedPackage>
    {
        new ClassifiedPackage { Id = "extra-pkg-1" }
    }
};

await client.Classifieds.AssignClassifiedPackagesAsync("offer-id", assignRequest);
```

---

## Error Handling

AllegroApi uses 11 specialized exception types to help you handle different error scenarios:

```csharp
using AllegroApi.Exceptions;

try
{
    var offer = await client.Offers.GetProductOfferAsync("offer-id");
}
catch (AllegroNotFoundException)
{
    // 404 - Resource not found
    Console.WriteLine("Offer not found");
}
catch (AllegroBadRequestException ex)
{
    // 400 - Validation error
    foreach (var error in ex.ValidationErrors)
    {
        Console.WriteLine($"{error.Field}: {error.Message}");
    }
}
catch (AllegroRateLimitException ex)
{
    // 429 - Rate limit exceeded
    Console.WriteLine($"Rate limit hit. Retry after {ex.RetryAfterSeconds}s");
    await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds));
}
catch (AllegroAuthenticationException)
{
    // 401 - Invalid or expired token
    Console.WriteLine("Authentication failed");
}
catch (AllegroAuthorizationException)
{
    // 403 - Insufficient permissions
    Console.WriteLine("Access denied");
}
catch (AllegroServerException ex)
{
    // 500+ - Server error
    Console.WriteLine($"Server error: {ex.StatusCode}");
}
```

### Exception Hierarchy

- `AllegroApiException` (base)
  - `AllegroBadRequestException` (400)
  - `AllegroAuthenticationException` (401)
  - `AllegroAuthorizationException` (403)
  - `AllegroNotFoundException` (404)
  - `AllegroConflictException` (409)
  - `AllegroUnprocessableEntityException` (422)
  - `AllegroRateLimitException` (429)
  - `AllegroServerException` (500+)
  - `AllegroNetworkException` (network errors)
  - `AllegroTimeoutException` (timeouts)

---

## Configuration

### AllegroApiOptions

```csharp
var options = new AllegroApiOptions
{
    // Required
    AccessToken = "your-access-token",
    
    // Optional
    BaseUrl = "https://api.allegro.pl", // or sandbox URL
    TimeoutSeconds = 100,
    MaxRetryAttempts = 3,
    RetryDelayMilliseconds = 1000,
    EnableLogging = true,
    AcceptLanguage = "en-US" // en-US, pl-PL, uk-UA, sk-SK, cs-CZ, hu-HU
};

var client = new AllegroApiClient(options);
```

### Environment URLs

| Environment | Base URL |
|-------------|----------|
| **Production** | `https://api.allegro.pl` |
| **Sandbox** | `https://api.allegro.pl.allegrosandbox.pl` |

### Rate Limits

The client automatically handles rate limiting:
- Respects `X-RateLimit-*` headers
- Exponential backoff on 429 responses
- Configurable retry attempts
- `AllegroRateLimitException` with `RetryAfterSeconds`

---

## Testing

The project includes comprehensive unit tests covering API clients, HTTP communication, configuration, and exception handling.

**Test Coverage:** 164 tests with 100% pass rate, covering the core API clients and the latest endpoint additions.

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~ProductClientTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Categories

- **Client Tests** (19 files) - API client functionality
- **HTTP Tests** - HTTP communication and error mapping
- **Configuration Tests** - Options validation
- **Exception Tests** - Exception behavior

---

## Contributing

Contributions are welcome. Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes with clear messages
4. Push to the branch and open a Pull Request

### Development Guidelines

- Follow existing code patterns and naming conventions
- Add unit tests for new features
- Include XML documentation for all public APIs
- Verify implementations against the `swagger.yaml` specification
- Ensure all tests pass before submitting

---

## Release Notes

### Version 2.0.0 (October 14, 2025)

This release adds warehouse management and marketplace features, bringing API coverage to 95%.

**New Clients:**

- FulfillmentClient (17 methods) - ASN management, inventory tracking, tax IDs
- ListingClient (4 methods) - Public offer search and discovery
- BadgesClient (6 methods) - Badge campaign management
- ClassifiedsClient (4 methods) - Classifieds package handling
- DisputeAttachmentsClient (3 methods) - Binary file uploads

**Extended Clients:**

- SaleExtensionsClient (+6 methods) - Additional service management
- AdvancedOffersClient (+3 methods) - Offer attachment support
- MiscellaneousClient (+1 method) - Charity campaign search

**Statistics:** API coverage increased from 86% to 95% (170+ of 180 endpoints). Added 44 new methods across 5 new clients.

### Version 1.4.0 (October 2025)

Added compatibility lists (4 methods), offer events monitoring, CPS conversion tracking, deposit types, and auction bidding. Coverage increased from 81% to 86%.

### Version 1.3.0 (October 2025)

Implemented post-purchase issues (5 methods), refund claims (4 methods), additional emails and contacts (9 methods), and size tables (4 methods). Coverage increased from 75% to 81%.

### Version 1.2.0 (October 2025)

Added shipment management (13 methods), batch operations (9 methods), and customer returns (3 methods). Coverage increased from 65% to 75%.

### Version 1.1.0 (October 2025)

Introduced after-sales services (12 methods), points of service (5 methods), user ratings (5 methods), and messaging and disputes (9 methods). Coverage increased from 50% to 65%.

### Version 1.0.0 (October 2025)

Initial release with 82 API methods covering core functionality (offers, products, orders, categories), 11 exception types, and retry logic with exponential backoff.

---

## License

This project is licensed under the **GNU General Public License v3.0 or later** (GPL-3.0-or-later).

See [LICENSE](LICENSE) file for details.

---

## Links

- **NuGet Package:** https://www.nuget.org/packages/AllegroApi/
- **GitHub Repository:** https://github.com/jomardyan/Allegro.NET.SDK
- **Allegro API Documentation:** https://developer.allegro.pl/
- **Issue Tracker:** https://github.com/jomardyan/Allegro.NET.SDK/issues

---

## Support

For questions, issues, or feature requests:
- Open an issue on GitHub
- Check existing documentation
- Review the [Allegro API docs](https://developer.allegro.pl/)

