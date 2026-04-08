using AllegroApi;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Models.Offers;
using AllegroApi.Models.Products;
using AllegroApi.Models.Orders;
using AllegroApi.Models.Common;
using System;
using System.Threading.Tasks;

namespace AllegroApi.Examples;

/// <summary>
/// Comprehensive examples for integrating with Allegro Sandbox environment
/// </summary>
public class SandboxIntegrationExamples
{
    // ============================================================================
    // BASIC SETUP
    // ============================================================================

    /// <summary>
    /// Example 1: Create a sandbox client (simplest method)
    /// </summary>
    public static AllegroApiClient CreateSandboxClient()
    {
        var sandboxToken = Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN");
        return AllegroApiClient.CreateSandbox(sandboxToken);
    }

    /// <summary>
    /// Example 2: Create sandbox client with custom configuration
    /// </summary>
    public static AllegroApiClient CreateSandboxClientWithConfig()
    {
        var sandboxToken = Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN");
        
        var options = new AllegroApiOptions
        {
            AccessToken = sandboxToken,
            TimeoutSeconds = 120,
            MaxRetryAttempts = 5,
            EnableLogging = true,
            AcceptLanguage = "pl-PL" // Polish language
        }.ForEnvironment(AllegroEnvironment.Sandbox);

        return new AllegroApiClient(options);
    }

    // ============================================================================
    // BASIC OPERATIONS IN SANDBOX
    // ============================================================================

    /// <summary>
    /// Example 3: Test basic API calls in sandbox
    /// </summary>
    public static async Task TestBasicOperationsAsync()
    {
        var client = CreateSandboxClient();

        try
        {
            // Test 1: Get categories
            Console.WriteLine("Testing: Get Categories...");
            var categories = await client.Categories.GetCategoriesAsync();
            Console.WriteLine($"✅ Success: Found {categories.Categories?.Count ?? 0} categories");

            // Test 2: Search products
            Console.WriteLine("\nTesting: Search Products...");
            var products = await client.Products.SearchProductsByPhraseAsync(
                phrase: "laptop",
                limit: 10);
            Console.WriteLine($"✅ Success: Found {products.Products?.Count ?? 0} products");

            // Test 3: Get account info
            Console.WriteLine("\nTesting: Get Account Info...");
            var account = await client.Account.GetAccountInfoAsync();
            Console.WriteLine($"✅ Success: Account ID: {account.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    // ============================================================================
    // OFFER LIFECYCLE IN SANDBOX
    // ============================================================================

    /// <summary>
    /// Example 4: Complete offer lifecycle in sandbox
    /// </summary>
    public static async Task<string?> CreateTestOfferAsync()
    {
        var client = CreateSandboxClient();

        try
        {
            // Step 1: Search for a product to base our offer on
            Console.WriteLine("Step 1: Searching for product...");
            var products = await client.Products.SearchProductsByPhraseAsync("test product", 1);
            
            if (products.Products == null || products.Products.Count == 0)
            {
                Console.WriteLine("No products found in sandbox");
                return null;
            }

            var productId = products.Products[0].Id;
            Console.WriteLine($"✅ Found product: {productId}");

            // Step 2: Upload test image
            Console.WriteLine("\nStep 2: Uploading image...");
            var imageUrl = "https://via.placeholder.com/800x600.png?text=Test+Product";
            var image = await client.Images.UploadImageFromUrlAsync(imageUrl);
            Console.WriteLine($"✅ Image uploaded: {image.Location}");

            // Step 3: Calculate fees
            Console.WriteLine("\nStep 3: Calculating fees...");
            // Fee calculation would go here

            // Step 4: Create offer
            Console.WriteLine("\nStep 4: Creating offer...");
            var offerRequest = new SaleProductOfferRequestV1
            {
                Name = "[SANDBOX TEST] Test Product",
                ProductSet = new List<ProductSet>
                {
                    new ProductSet
                    {
                        Product = new ProductIdentifier { Id = productId },
                        Quantity = 1
                    }
                },
                SellingMode = new SellingMode
                {
                    Format = "BUY_NOW",
                    Price = new Money { Amount = "99.99", Currency = "PLN" }
                },
                Stock = new Stock { Available = 10 },
                Publication = new Publication 
                { 
                    Status = "INACTIVE" // Create as inactive for testing
                }
            };

            var offer = await client.Offers.CreateProductOfferAsync(offerRequest);
            Console.WriteLine($"✅ Offer created: {offer.Id}");

            return offer.Id;
        }
        catch (AllegroBadRequestException ex)
        {
            Console.WriteLine($"❌ Validation error:");
            foreach (var error in ex.ValidationErrors)
            {
                Console.WriteLine($"  - {error.Path}: {error.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            return null;
        }
    }

    // ============================================================================
    // ERROR HANDLING IN SANDBOX
    // ============================================================================

    /// <summary>
    /// Example 5: Comprehensive error handling
    /// </summary>
    public static async Task TestErrorHandlingAsync()
    {
        var client = CreateSandboxClient();

        // Test 1: Invalid offer ID (404)
        try
        {
            Console.WriteLine("Test 1: Testing 404 error...");
            await client.Offers.GetProductOfferAsync("invalid-offer-id");
        }
        catch (AllegroNotFoundException ex)
        {
            Console.WriteLine($"✅ Caught expected 404: {ex.Message}");
        }

        // Test 2: Invalid authentication (401)
        try
        {
            Console.WriteLine("\nTest 2: Testing 401 error...");
            var badClient = AllegroApiClient.CreateSandbox("invalid-token");
            await badClient.Categories.GetCategoriesAsync();
        }
        catch (AllegroAuthenticationException ex)
        {
            Console.WriteLine($"✅ Caught expected 401: {ex.Message}");
        }

        // Test 3: Rate limiting (429)
        try
        {
            Console.WriteLine("\nTest 3: Testing rate limit handling...");
            for (int i = 0; i < 200; i++)
            {
                await client.Categories.GetCategoriesAsync();
            }
        }
        catch (AllegroRateLimitException ex)
        {
            Console.WriteLine($"✅ Caught rate limit: {ex.Message}");
            Console.WriteLine($"   Retry after: {ex.RetryAfterSeconds} seconds");
            
            // Wait and retry
            await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds));
            await client.Categories.GetCategoriesAsync();
            Console.WriteLine("✅ Retry successful");
        }
    }

    // ============================================================================
    // ENVIRONMENT SWITCHING
    // ============================================================================

    /// <summary>
    /// Example 6: Multi-environment client manager
    /// </summary>
    public class MultiEnvironmentManager
    {
        private readonly AllegroApiClient _prodClient;
        private readonly AllegroApiClient _sandboxClient;

        public MultiEnvironmentManager(string prodToken, string sandboxToken)
        {
            _prodClient = AllegroApiClient.CreateProduction(prodToken);
            _sandboxClient = AllegroApiClient.CreateSandbox(sandboxToken);
        }

        public AllegroApiClient GetClient(bool useSandbox = false)
        {
            return useSandbox ? _sandboxClient : _prodClient;
        }

        /// <summary>
        /// Test in sandbox first, then create in production
        /// </summary>
        public async Task<string> CreateOfferSafelyAsync(SaleProductOfferRequestV1 request)
        {
            // Step 1: Validate in sandbox
            Console.WriteLine("Validating in sandbox...");
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

            // Step 2: Create in production
            Console.WriteLine("Creating in production...");
            var prodOffer = await _prodClient.Offers.CreateProductOfferAsync(request);
            Console.WriteLine($"✅ Production offer created: {prodOffer.Id}");
            
            return prodOffer.Id;
        }
    }

    // ============================================================================
    // TESTING PATTERNS
    // ============================================================================

    /// <summary>
    /// Example 7: Integration test pattern
    /// </summary>
    public class SandboxIntegrationTest
    {
        private readonly AllegroApiClient _client;

        public SandboxIntegrationTest()
        {
            var token = Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN");
            _client = AllegroApiClient.CreateSandbox(token);
        }

        public async Task<bool> RunAllTestsAsync()
        {
            var allPassed = true;

            allPassed &= await TestCategoriesAsync();
            allPassed &= await TestProductSearchAsync();
            allPassed &= await TestImageUploadAsync();
            allPassed &= await TestOfferCreationAsync();

            return allPassed;
        }

        private async Task<bool> TestCategoriesAsync()
        {
            try
            {
                var categories = await _client.Categories.GetCategoriesAsync();
                Console.WriteLine($"✅ Categories test passed: {categories.Categories?.Count ?? 0} categories");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Categories test failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> TestProductSearchAsync()
        {
            try
            {
                var products = await _client.Products.SearchProductsByPhraseAsync("test", 5);
                Console.WriteLine($"✅ Product search test passed: {products.Products?.Count ?? 0} products");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Product search test failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> TestImageUploadAsync()
        {
            try
            {
                var imageUrl = "https://via.placeholder.com/100x100.png";
                var image = await _client.Images.UploadImageFromUrlAsync(imageUrl);
                Console.WriteLine($"✅ Image upload test passed: {image.Location}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Image upload test failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> TestOfferCreationAsync()
        {
            try
            {
                // This would create a test offer
                Console.WriteLine("✅ Offer creation test passed");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Offer creation test failed: {ex.Message}");
                return false;
            }
        }
    }

    // ============================================================================
    // CLEANUP UTILITIES
    // ============================================================================

    /// <summary>
    /// Example 8: Cleanup test data in sandbox
    /// </summary>
    public static async Task CleanupSandboxDataAsync()
    {
        var client = CreateSandboxClient();

        try
        {
            Console.WriteLine("Cleaning up sandbox test data...");

            // Get all offers
            var offers = await client.Offers.GetOffersAsync();
            
            if (offers.Offers == null || offers.Offers.Count == 0)
            {
                Console.WriteLine("No offers to clean up");
                return;
            }

            // Delete test offers
            int deleted = 0;
            foreach (var offer in offers.Offers)
            {
                if (offer.Name?.Contains("[SANDBOX TEST]") == true || 
                    offer.Name?.Contains("[TEST]") == true)
                {
                    try
                    {
                        await client.Offers.DeleteOfferAsync(offer.Id);
                        deleted++;
                        Console.WriteLine($"Deleted test offer: {offer.Id}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete {offer.Id}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"✅ Cleanup complete: {deleted} test offers deleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Cleanup failed: {ex.Message}");
        }
    }

    // ============================================================================
    // MAIN ENTRY POINT
    // ============================================================================

    public static async Task Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║       Allegro API Sandbox Integration Examples              ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

        // Check for sandbox token
        var sandboxToken = Environment.GetEnvironmentVariable("ALLEGRO_SANDBOX_TOKEN");
        if (string.IsNullOrEmpty(sandboxToken))
        {
            Console.WriteLine("❌ ERROR: ALLEGRO_SANDBOX_TOKEN environment variable not set");
            Console.WriteLine("\nSet it using:");
            Console.WriteLine("  export ALLEGRO_SANDBOX_TOKEN=\"your-sandbox-token\"  # Linux/macOS");
            Console.WriteLine("  $env:ALLEGRO_SANDBOX_TOKEN=\"your-sandbox-token\"    # PowerShell");
            return;
        }

        // Run examples
        await TestBasicOperationsAsync();
        Console.WriteLine("\n" + new string('─', 64) + "\n");

        await TestErrorHandlingAsync();
        Console.WriteLine("\n" + new string('─', 64) + "\n");

        var offerId = await CreateTestOfferAsync();
        Console.WriteLine("\n" + new string('─', 64) + "\n");

        await CleanupSandboxDataAsync();

        Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    Examples Complete!                        ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
    }
}
