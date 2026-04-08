# AllegroApi Demo Application

A console application demonstrating the major features of the AllegroApi .NET SDK.

## Prerequisites

- .NET 8.0 SDK or later
- Allegro API credentials (access token)

## Setup

### 1. Get API Credentials

- **Production**: https://apps.developer.allegro.pl/
- **Sandbox** (recommended for testing): https://apps.developer.allegro.pl.allegrosandbox.pl/

### 2. Set Environment Variables

**Linux/macOS:**
```bash
export ALLEGRO_API_TOKEN="your-access-token"
export USE_SANDBOX="true"  # Recommended for testing
```

**Windows PowerShell:**
```powershell
$env:ALLEGRO_API_TOKEN = "your-access-token"
$env:USE_SANDBOX = "true"
```

**Windows CMD:**
```cmd
set ALLEGRO_API_TOKEN=your-access-token
set USE_SANDBOX=true
```

### 3. Run the Demo

```bash
cd examples/DemoApp
dotnet run
```

## What This Demo Covers

| Demo | Description |
|------|-------------|
| Account Information | Get account details and user profile |
| Browse Categories | Fetch category tree and navigate hierarchy |
| Search Products | Search products by phrase with pagination |
| Get Product Details | Retrieve full product information and parameters |
| Search Orders | Query orders with filters and display summaries |
| Upload Images | Upload images from URLs |
| Error Handling | Demonstrate 404, 400, and 429 error handling |
| Batch Operations | Show bulk price and quantity update structures |
| Fulfillment Operations | Query fulfillment stock and manage warehousing |
| Public Listings | Search and display public offers |

## Customization

### Running Specific Demos

Edit `Program.cs` and comment out demos you don't need:

```csharp
static async Task RunAllDemosAsync(AllegroApiClient client)
{
    await Demo1_GetAccountInfo(client);
    // await Demo2_BrowseCategories(client);  // Comment out to skip
    await Demo3_SearchProducts(client);
}
```

### Adding Your Own Demos

```csharp
static async Task Demo11_YourFeature(AllegroApiClient client)
{
    Console.WriteLine("=== Demo 11: Your Feature ===");

    try
    {
        var result = await client.YourClient.YourMethodAsync();
        Console.WriteLine($"Result: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

Then add it to `RunAllDemosAsync()`:

```csharp
await Demo11_YourFeature(client);
```

## Troubleshooting

**Authentication failed - invalid token**
- Verify `ALLEGRO_API_TOKEN` is set correctly
- Check if the token has expired (tokens expire after a few hours)
- Ensure you are using the correct token type (sandbox vs. production)

**Access denied - insufficient permissions**
- Some endpoints require specific OAuth scopes
- Verify your application has the necessary permissions at the developer portal

**Rate limit exceeded**
- The SDK handles rate limits automatically with retry logic
- If you still see this error, reduce request frequency

## Resources

- **GitHub Repository:** https://github.com/jomardyan/Allegro.NET.SDK
- **NuGet Package:** https://www.nuget.org/packages/AllegroApi/
- **Allegro API Documentation:** https://developer.allegro.pl/
- **SDK Documentation:** See the main [README.md](../../README.md)

## License

GPL-3.0-or-later — see [LICENSE](../../LICENSE) for details.
