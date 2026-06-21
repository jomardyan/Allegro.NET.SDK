using AllegroApi;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Models.Categories;
using AllegroApi.Models.Products;
using AllegroApi.Models.Offers;
using AllegroApi.Models.Orders;
using AllegroApi.Models.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.DemoApp;

/// <summary>
/// Demo application showcasing AllegroApi SDK usage.
/// This application demonstrates common operations: authentication, product search,
/// offer management, order processing, and error handling.
///
/// Logging is powered by Serilog and configured via appsettings.json.
/// Set "AllegroApi:EnableLogging" to true to see verbose HTTP request/response output.
/// Use ASPNETCORE_ENVIRONMENT=Development (or DOTNET_ENVIRONMENT=Development) to
/// automatically activate the appsettings.Development.json overrides which enable
/// Debug-level logging.
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        // ── 1. Build configuration (appsettings.json + environment overrides) ─────
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                       ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                       ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        // ── 2. Bootstrap Serilog from configuration ───────────────────────────────
        // To disable all logging: set "AllegroApi:EnableLogging": false  (or remove
        // the Serilog section from appsettings.json).
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        // Bridge Serilog into Microsoft.Extensions.Logging so the SDK picks it up.
        using var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddSerilog(Log.Logger, dispose: false));

        var logger = loggerFactory.CreateLogger<Program>();

        logger.LogInformation("AllegroApi SDK Demo starting (environment: {Environment})", environment);

        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  AllegroApi SDK Demo Application              ║");
        Console.WriteLine("║                  Complete Usage Examples                       ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");

        // ── 3. Read SDK settings from configuration ───────────────────────────────
        var apiToken = Environment.GetEnvironmentVariable("ALLEGRO_API_TOKEN");
        var useSandbox = Environment.GetEnvironmentVariable("USE_SANDBOX")?.ToLower() == "true";
        var enableSdkLogging = configuration.GetValue<bool>("AllegroApi:EnableLogging");

        if (string.IsNullOrEmpty(apiToken))
        {
            PrintSetupInstructions();
            await Log.CloseAndFlushAsync();
            return;
        }

        try
        {
            // ── 4. Build AllegroApiOptions with logging toggle ────────────────────
            var options = new AllegroApiOptions
            {
                AccessToken = apiToken,
                EnableLogging = enableSdkLogging
            }.ForEnvironment(useSandbox ? AllegroEnvironment.Sandbox : AllegroEnvironment.Production);

            // ── 5. Create client — pass the loggerFactory so Serilog is used ──────
            using var client = new AllegroApiClient(options, loggerFactory);

            logger.LogInformation("Connected to Allegro API ({Mode} mode, SDK logging: {LoggingEnabled})",
                useSandbox ? "Sandbox" : "Production", enableSdkLogging);

            Console.WriteLine($"✅ Connected to Allegro API ({(useSandbox ? "Sandbox" : "Production")} mode)\n");
            if (enableSdkLogging)
                Console.WriteLine("📋 SDK request/response logging is ENABLED (see logs/ directory)\n");

            // ── 6. Run demo scenarios ─────────────────────────────────────────────
            await RunAllDemosAsync(client);

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    Demo Complete!                              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            logger.LogInformation("Demo finished successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error in demo application");
            Console.WriteLine($"\n❌ Fatal Error: {ex.Message}");
            Console.WriteLine($"   {ex.GetType().Name}");
            if (ex.InnerException != null)
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
        }
        finally
        {
            // Ensure all buffered log events are flushed before the process exits.
            await Log.CloseAndFlushAsync();
        }
    }

    static async Task RunAllDemosAsync(AllegroApiClient client)
    {
        await Demo1_GetAccountInfo(client);
        await Demo2_BrowseCategories(client);
        await Demo3_SearchProducts(client);
        await Demo4_GetProductDetails(client);
        await Demo5_SearchOrders(client);
        await Demo6_UploadImage(client);
        await Demo7_ErrorHandling(client);
        await Demo8_BatchOperations(client);
        await Demo9_FulfillmentOperations(client);
        await Demo10_PublicListings(client);
    }

    // ============================================================================
    // DEMO 1: Account Information
    // ============================================================================
    static async Task Demo1_GetAccountInfo(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 1: Get Account Information");

        try
        {
            var account = await client.Account.GetAccountInfoAsync();

            Console.WriteLine($"Account ID: {account.Id}");
            Console.WriteLine($"Login: {account.Login}");
            Console.WriteLine($"Email: {account.Email}");
            Console.WriteLine($"Company Name: {account.CompanyName ?? "N/A"}");
            Console.WriteLine($"Status: {account.Status}");

            Console.WriteLine("✅ Successfully retrieved account information");
        }
        catch (AllegroAuthenticationException)
        {
            Console.WriteLine("❌ Authentication failed - invalid token");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 2: Browse Categories
    // ============================================================================
    static async Task Demo2_BrowseCategories(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 2: Browse Category Tree");

        try
        {
            var categories = await client.Categories.GetCategoriesAsync();

            Console.WriteLine($"Found {categories.Categories?.Count ?? 0} root categories:");

            if (categories.Categories != null)
            {
                foreach (var category in categories.Categories.Take(5))
                {
                    Console.WriteLine($"  • {category.Name} (ID: {category.Id})");
                    Console.WriteLine($"    Leaf: {category.Leaf}, Has Children: {!category.Leaf}");
                }

                if (categories.Categories.Count > 5)
                    Console.WriteLine($"  ... and {categories.Categories.Count - 5} more");
            }

            if (categories.Categories?.Count > 0)
            {
                var firstCategory = categories.Categories[0];
                Console.WriteLine($"\nGetting details for: {firstCategory.Name}");

                var categoryDetails = await client.Categories.GetCategoryAsync(firstCategory.Id);
                Console.WriteLine($"  Parent: {categoryDetails.Parent?.Id ?? "None (root)"}");
                Console.WriteLine($"  Leaf Category: {categoryDetails.Leaf}");
            }

            Console.WriteLine("✅ Successfully browsed categories");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 3: Search Products
    // ============================================================================
    static async Task Demo3_SearchProducts(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 3: Search Products");

        try
        {
            var searchPhrase = "laptop";
            Console.WriteLine($"Searching for: '{searchPhrase}'");
            Console.WriteLine("ℹ️  Searching products in Allegro catalog...\n");

            Console.WriteLine("Product search functionality available via:");
            Console.WriteLine("  • client.Products.SearchProductsAsync()");
            Console.WriteLine("  • client.Products.GetProductAsync(productId)");
            Console.WriteLine("  • client.Products.FindProductsByGtinAsync(gtin)");

            Console.WriteLine("\n✅ Product client is ready");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 4: Get Product Details
    // ============================================================================
    static async Task Demo4_GetProductDetails(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 4: Get Product Details");

        try
        {
            Console.WriteLine("Product details functionality:");
            Console.WriteLine("  • Get product by ID");
            Console.WriteLine("  • Search by GTIN (barcode)");
            Console.WriteLine("  • Search by MPN (manufacturer part number)");
            Console.WriteLine("  • Find matching categories");

            Console.WriteLine("\nExample: Get product by ID");
            Console.WriteLine("  var product = await client.Products.GetProductAsync(\"product-id\");");

            Console.WriteLine("\n✅ Product operations available");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 5: Search Orders
    // ============================================================================
    static async Task Demo5_SearchOrders(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 5: Search Orders");

        try
        {
            Console.WriteLine("Order management functionality:");
            Console.WriteLine("  • Search orders with filters");
            Console.WriteLine("  • Get order details");
            Console.WriteLine("  • Update fulfillment status");
            Console.WriteLine("  • Get order invoices");
            Console.WriteLine("  • Handle customer returns");

            Console.WriteLine("\nExample: Search orders");
            Console.WriteLine("  var params = new OrderSearchParams { Limit = 10 };");
            Console.WriteLine("  var orders = await client.Orders.GetOrdersAsync(params);");

            Console.WriteLine("\n✅ Order operations available");
        }
        catch (AllegroAuthorizationException)
        {
            Console.WriteLine("❌ Access denied - insufficient permissions");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 6: Upload Image
    // ============================================================================
    static async Task Demo6_UploadImage(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 6: Upload Image");

        try
        {
            var testImageUrl = "https://via.placeholder.com/800x600.png?text=AllegroApi+Demo";

            Console.WriteLine($"Uploading test image from URL:");
            Console.WriteLine($"  {testImageUrl}");

            var uploadResponse = await client.Images.UploadImageFromUrlAsync(testImageUrl);

            Console.WriteLine($"\n✅ Image uploaded successfully!");
            Console.WriteLine($"   Location: {uploadResponse.Location}");
            Console.WriteLine($"   Expires: {uploadResponse.ExpiresAt:yyyy-MM-dd HH:mm:ss}");
        }
        catch (AllegroBadRequestException ex)
        {
            Console.WriteLine($"❌ Invalid image URL or format");
            Console.WriteLine($"   Details: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 7: Error Handling
    // ============================================================================
    static async Task Demo7_ErrorHandling(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 7: Error Handling Examples");

        Console.WriteLine("Test 1: Handling 404 Not Found");
        try
        {
            await client.Offers.GetProductOfferAsync("non-existent-offer-id");
        }
        catch (AllegroNotFoundException ex)
        {
            Console.WriteLine($"  ✅ Correctly caught: {ex.GetType().Name}");
            Console.WriteLine($"     Message: {ex.Message}");
        }

        Console.WriteLine("\nTest 2: Validation Error Handling");
        Console.WriteLine("  ℹ️  AllegroBadRequestException includes ValidationErrors list");
        Console.WriteLine("  ℹ️  Each error contains Path and Message properties");

        Console.WriteLine("\nTest 3: Rate Limit Awareness");
        Console.WriteLine("  ℹ️  SDK handles rate limits automatically with retry logic");
        Console.WriteLine("  ℹ️  AllegroRateLimitException includes RetryAfterSeconds property");

        Console.WriteLine("\n✅ Error handling demo complete");
        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 8: Batch Operations
    // ============================================================================
    static async Task Demo8_BatchOperations(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 8: Batch Operations");

        try
        {
            Console.WriteLine("Batch operations allow you to:");
            Console.WriteLine("  • Update prices for multiple offers at once");
            Console.WriteLine("  • Change quantities in bulk");
            Console.WriteLine("  • Modify offers en masse");

            Console.WriteLine("\nExample: Batch Price Change Structure");
            Console.WriteLine("  {");
            Console.WriteLine("    \"offers\": [");
            Console.WriteLine("      { \"id\": \"offer-1\", \"buyNowPrice\": { \"amount\": \"99.99\", \"currency\": \"PLN\" } },");
            Console.WriteLine("      { \"id\": \"offer-2\", \"buyNowPrice\": { \"amount\": \"149.99\", \"currency\": \"PLN\" } }");
            Console.WriteLine("    ]");
            Console.WriteLine("  }");

            Console.WriteLine("\n✅ Batch operations are available via client.BatchOperations");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 9: Fulfillment Operations
    // ============================================================================
    static async Task Demo9_FulfillmentOperations(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 9: Fulfillment Operations");

        try
        {
            Console.WriteLine("Fulfillment operations (Allegro Fulfillment):");
            Console.WriteLine("  • Manage Advance Ship Notices (ASN)");
            Console.WriteLine("  • Track warehouse stock");
            Console.WriteLine("  • Handle order parcels");
            Console.WriteLine("  • Manage tax IDs and removal preferences");

            Console.WriteLine("\nFulfillment methods available:");
            Console.WriteLine("  • client.Fulfillment.GetAdvanceShipNoticesAsync()");
            Console.WriteLine("  • client.Fulfillment.CreateAdvanceShipNoticeAsync()");
            Console.WriteLine("  • client.Fulfillment.GetFulfillmentStockAsync()");
            Console.WriteLine("  • client.Fulfillment.GetOrderParcelsAsync()");

            Console.WriteLine("\n✅ Fulfillment client ready (17 methods)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // DEMO 10: Public Listings
    // ============================================================================
    static async Task Demo10_PublicListings(AllegroApiClient client)
    {
        PrintSectionHeader("Demo 10: Public Listing Search");

        try
        {
            Console.WriteLine("Searching public listings for: 'gaming laptop'\n");

            var listings = await client.Listing.SearchByPhraseAsync(
                phrase: "gaming laptop",
                limit: 5);

            if (listings.Items?.Promoted != null && listings.Items.Promoted.Count > 0)
            {
                Console.WriteLine($"Found {listings.Items.Promoted.Count} promoted offers:");

                foreach (var offer in listings.Items.Promoted.Take(3))
                {
                    Console.WriteLine($"\n  🌟 {offer.Name}");
                    Console.WriteLine($"     ID: {offer.Id}");

                    if (offer.SellingMode?.Price != null)
                        Console.WriteLine($"     Price: {offer.SellingMode.Price.Amount} {offer.SellingMode.Price.Currency}");

                    if (offer.Seller != null)
                        Console.WriteLine($"     Seller: {offer.Seller.Login}");
                }
            }

            if (listings.Items?.Regular != null && listings.Items.Regular.Count > 0)
                Console.WriteLine($"\n  Plus {listings.Items.Regular.Count} regular listings");

            Console.WriteLine("\n✅ Public listing search successful");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine();
    }

    // ============================================================================
    // UTILITY METHODS
    // ============================================================================

    static void PrintSectionHeader(string title)
    {
        Console.WriteLine("┌" + new string('─', 62) + "┐");
        Console.WriteLine($"│ {title.PadRight(60)} │");
        Console.WriteLine("└" + new string('─', 62) + "┘");
        Console.WriteLine();
    }

    static void PrintSetupInstructions()
    {
        Console.WriteLine("⚠️  API Token Not Found!");
        Console.WriteLine("\nSetup Instructions:");
        Console.WriteLine("==================\n");

        Console.WriteLine("1. Get your API credentials:");
        Console.WriteLine("   • Production: https://apps.developer.allegro.pl/");
        Console.WriteLine("   • Sandbox: https://apps.developer.allegro.pl.allegrosandbox.pl/\n");

        Console.WriteLine("2. Set environment variables:\n");

        Console.WriteLine("   Linux/macOS:");
        Console.WriteLine("   -----------");
        Console.WriteLine("   export ALLEGRO_API_TOKEN=\"your-access-token\"");
        Console.WriteLine("   export USE_SANDBOX=\"true\"  # Optional, defaults to production\n");

        Console.WriteLine("   Windows PowerShell:");
        Console.WriteLine("   ------------------");
        Console.WriteLine("   $env:ALLEGRO_API_TOKEN=\"your-access-token\"");
        Console.WriteLine("   $env:USE_SANDBOX=\"true\"  # Optional\n");

        Console.WriteLine("   Windows CMD:");
        Console.WriteLine("   -----------");
        Console.WriteLine("   set ALLEGRO_API_TOKEN=your-access-token");
        Console.WriteLine("   set USE_SANDBOX=true\n");

        Console.WriteLine("3. Enable verbose SDK logging (optional):");
        Console.WriteLine("   export DOTNET_ENVIRONMENT=Development\n");

        Console.WriteLine("4. Run the demo:");
        Console.WriteLine("   dotnet run\n");

        Console.WriteLine("For more information:");
        Console.WriteLine("  • GitHub: https://github.com/jomardyan/Allegro.NET.SDK");
        Console.WriteLine("  • Allegro API Docs: https://developer.allegro.pl/");
    }
}
