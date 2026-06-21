using AllegroApi;
using AllegroApi.Configuration;
using AllegroApi.Exceptions;
using AllegroApi.Models.Common;
using AllegroApi.Models.Listing;
using AllegroApi.Models.Offers;
using AllegroApi.Models.Orders;
using AllegroApi.Models.Pricing;
using AllegroApi.Models.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.DemoApp;

/// <summary>
/// Full-coverage demo application — exercises every major AllegroApi SDK client.
///
/// Logging: controlled via appsettings.json "AllegroApi:EnableLogging".
/// Set DOTNET_ENVIRONMENT=Development to load appsettings.Development.json which
/// enables Debug-level Serilog output and verbose SDK request/response tracing.
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        // ── Configuration ──────────────────────────────────────────────────────
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                       ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                       ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        // ── Serilog ────────────────────────────────────────────────────────────
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        using var loggerFactory = LoggerFactory.Create(b => b.AddSerilog(Log.Logger, dispose: false));
        var logger = loggerFactory.CreateLogger<Program>();

        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              AllegroApi SDK — Full Feature Demo                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        logger.LogInformation("Demo starting (environment: {Env})", environment);

        // ── Token / client setup ───────────────────────────────────────────────
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
            var options = new AllegroApiOptions { AccessToken = apiToken, EnableLogging = enableSdkLogging }
                .ForEnvironment(useSandbox ? AllegroEnvironment.Sandbox : AllegroEnvironment.Production);

            using var client = new AllegroApiClient(options, loggerFactory);

            Console.WriteLine($"Connected — {(useSandbox ? "Sandbox" : "Production")} " +
                              $"| SDK logging: {(enableSdkLogging ? "ON" : "OFF")}\n");

            await Group1_AccountAndUsers(client);
            await Group2_MarketplacesAndCategories(client);
            await Group3_ProductsAndOffers(client);
            await Group4_OrderManagement(client);
            await Group5_PaymentsAndBilling(client);
            await Group6_ShippingSetup(client);
            await Group7_AfterSales(client);
            await Group8_MessagingCenter(client);
            await Group9_DisputesAndReturns(client);
            await Group10_Fulfillment(client);
            await Group11_ShipmentManagement(client);
            await Group12_SaleExtensions(client);
            await Group13_AllegroPricesAndAutomation(client);
            await Group14_InfrastructureClients(client);
            await Group15_MiscellaneousAndDiscovery(client);
            await Group16_PublicListing(client);
            await Group17_ImageUpload(client);
            await Group18_ErrorHandling(client);

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    All demos complete!                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            logger.LogInformation("All demo groups finished");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error");
            Console.WriteLine($"\n❌ Fatal Error: {ex.GetType().Name} — {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 1 — Account & User Ratings
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group1_AccountAndUsers(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 1 — Account & User Ratings");

        await RunDemo("1a. Account Overview", async () =>
        {
            var info = await c.Account.GetAccountInfoAsync();
            Console.WriteLine($"  Login      : {info.Login}");
            Console.WriteLine($"  Email      : {info.Email}");
            Console.WriteLine($"  Type       : {info.Type}");
            Console.WriteLine($"  Status     : {info.Status}");
            Console.WriteLine($"  Company    : {info.Company?.Name ?? info.CompanyName ?? "—"}");
            Console.WriteLine($"  Marketplace: {info.BaseMarketplace?.Id ?? "—"}");
            Console.WriteLine($"  Created    : {info.CreatedAt:yyyy-MM-dd}");
            if (info.Features?.Count > 0)
                Console.WriteLine($"  Features   : {string.Join(", ", info.Features)}");
        });

        await RunDemo("1b. Account Settings", async () =>
        {
            var s = await c.Account.GetAccountSettingsAsync();
            Console.WriteLine($"  Notifications : {s.NotificationsEnabled}");
            Console.WriteLine($"  Email notifs  : {s.EmailNotifications}");
            Console.WriteLine($"  SMS notifs    : {s.SmsNotifications}");
        });

        await RunDemo("1c. Sales Quality History", async () =>
        {
            var q = await c.Account.GetSalesQualityAsync();
            Console.WriteLine($"  History entries: {q.Quality?.Count ?? 0}");
            var latest = q.Quality?.FirstOrDefault();
            if (latest != null)
                Console.WriteLine($"  Latest: {latest.ResultFor}  score: {latest.Score}/{latest.MaxScore}  grade: {latest.Grade}");
        });

        await RunDemo("1d. Smart Seller Classification", async () =>
        {
            var s = await c.Account.GetSmartClassificationAsync();
            Console.WriteLine($"  Marketplace   : {s.MarketplaceId}");
            Console.WriteLine($"  Is Smart      : {s.Smart}");
            Console.WriteLine($"  Smart since   : {s.SmartSince ?? "—"}");
            Console.WriteLine($"  Smart until   : {s.SmartUntil ?? "—"}");
            var met = s.Criteria?.Count(cr => cr.Met) ?? 0;
            Console.WriteLine($"  Criteria met  : {met}/{s.Criteria?.Count ?? 0}");
        });

        await RunDemo("1e. My Ratings (latest 5)", async () =>
        {
            var result = await c.Users.GetUserRatingsAsync(limit: 5);
            Console.WriteLine($"  Total ratings: {result.Count ?? 0}");
            foreach (var r in result.Ratings?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.Users.UserRating>())
                Console.WriteLine($"  [{r.Score}]  {r.Comment?.Truncate(55)}  — {r.Author?.Login}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 2 — Marketplaces & Categories
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group2_MarketplacesAndCategories(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 2 — Marketplaces & Categories");

        await RunDemo("2a. Available Marketplaces", async () =>
        {
            var result = await c.Marketplaces.GetAllMarketplacesAsync();
            foreach (var m in result.Marketplaces)
            {
                var currency = m.Currencies?.Base?.Code ?? "—";
                Console.WriteLine($"  {m.Id,-22} base currency: {currency}  " +
                                  $"shipping countries: {m.ShippingCountries?.Count ?? 0}");
            }
        });

        await RunDemo("2b. Root Categories", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            Console.WriteLine($"  Root categories: {cats?.Categories?.Count ?? 0}");
            foreach (var cat in cats?.Categories?.Take(6) ?? Enumerable.Empty<AllegroApi.Models.Categories.CategoryDto>())
                Console.WriteLine($"  {cat.Id,-12} {cat.Name?.Truncate(35),-35} leaf: {cat.Leaf}");
        });

        await RunDemo("2c. Category Parameters (first category)", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            var firstId = cats?.Categories?.FirstOrDefault()?.Id;
            if (firstId is null) { Console.WriteLine("  No categories found"); return; }

            var p = await c.Categories.GetCategoryParametersAsync(firstId);
            Console.WriteLine($"  Category '{firstId}' — parameters: {p?.Parameters?.Count ?? 0}");
            foreach (var param in p?.Parameters?.Take(4) ?? Enumerable.Empty<AllegroApi.Models.Categories.CategoryParameter>())
                Console.WriteLine($"  {param.Id,-8} {param.Name?.Truncate(30),-30} type: {param.Type}  required: {param.Required}");
        });

        await RunDemo("2d. Tax Settings (first category)", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            var firstId = cats?.Categories?.FirstOrDefault()?.Id;
            if (firstId is null) { Console.WriteLine("  No categories found"); return; }

            var tax = await c.Categories.GetTaxSettingsForCategoryAsync(firstId);
            Console.WriteLine($"  Tax settings for '{firstId}': {tax?.TaxSettings?.Count ?? 0} entries");
        });

        await RunDemo("2e. Category Events (latest 5)", async () =>
        {
            var result = await c.Categories.GetCategoryEventsAsync(limit: 5);
            Console.WriteLine($"  Events: {result?.CategoryEvents?.Count ?? 0}");
            foreach (var ev in result?.CategoryEvents?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Categories.CategoryEvent>())
                Console.WriteLine($"  {ev.OccurredAt:yyyy-MM-dd HH:mm}  {ev.Type,-20}  category: {ev.Category?.Id}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 3 — Products & Offers
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group3_ProductsAndOffers(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 3 — Products & Offers");

        await RunDemo("3a. Product Search ('laptop')", async () =>
        {
            var result = await c.Products.SearchProductsAsync(new ProductSearchParams { Phrase = "laptop", Limit = 5 });
            Console.WriteLine($"  Found {result.TotalCount} products (showing {result.Count})");
            foreach (var p in result.Products.Take(3))
                Console.WriteLine($"  {p.Id[..8]}…  {p.Name?.Truncate(55)}");
        });

        await RunDemo("3b. Matching Categories ('smartwatch')", async () =>
        {
            var result = await c.Products.GetMatchingCategoriesAsync("smartwatch");
            Console.WriteLine($"  Matching categories: {result?.MatchingCategories?.Count ?? 0}");
            foreach (var cat in result?.MatchingCategories?.Take(4) ?? Enumerable.Empty<AllegroApi.Models.Products.CategoryInfo>())
                Console.WriteLine($"  {cat.Id,-12} {cat.Name}");
        });

        await RunDemo("3c. My Offers (first page, 5)", async () =>
        {
            var result = await c.Offers.SearchOffersAsync(new OfferSearchParams { Limit = 5 });
            Console.WriteLine($"  Total offers: {result.TotalCount}");
            foreach (var o in result.Offers.Take(5))
            {
                var price = o.SellingMode?.Price;
                Console.WriteLine($"  {o.Id[..8]}… {o.Name?.Truncate(38),-38} " +
                                  $"{price?.Amount,8} {price?.Currency}  [{o.Publication?.Status}]");
            }
        });

        await RunDemo("3d. Offer Fee Preview", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            var catId = cats?.Categories?.FirstOrDefault()?.Id ?? "1";
            var req = new FeePreviewRequest
            {
                Offer = new PricingOffer
                {
                    Category = new PricingCategory { Id = catId },
                    SellingMode = new PricingSellingMode
                    {
                        Format = "BUY_NOW",
                        Price = new Money { Amount = "99.99", Currency = "PLN" }
                    }
                }
            };
            var preview = await c.Pricing.GetOfferFeePreviewAsync(req);
            Console.WriteLine($"  Commissions: {preview?.Commissions?.Count ?? 0}  " +
                              $"quotes: {preview?.Quotes?.Count ?? 0}");
            foreach (var f in preview?.Commissions?.Take(3) ?? Enumerable.Empty<CommissionResponse>())
                Console.WriteLine($"  {f.Name?.Truncate(35),-35} {f.Fee?.Amount,8} {f.Fee?.Currency}");
        });

        await RunDemo("3e. Offer Quotes (first 3 offers)", async () =>
        {
            var offers = await c.Offers.SearchOffersAsync(new OfferSearchParams { Limit = 3 });
            var ids = offers.Offers.Select(o => o.Id).ToList();
            if (!ids.Any()) { Console.WriteLine("  No offers to quote"); return; }

            var quotes = await c.Pricing.GetOfferQuotesAsync(ids);
            Console.WriteLine($"  Quotes: {quotes?.OfferQuotes?.Count ?? 0}");
            foreach (var q in quotes?.OfferQuotes?.Take(3) ?? Enumerable.Empty<OfferQuote>())
                Console.WriteLine($"  {q.OfferId?[..8]}…  listing fee: {q.ListingFee?.Amount?.Amount} {q.ListingFee?.Amount?.Currency}");
        });

        await RunDemo("3f. Offer Rating (first offer)", async () =>
        {
            var offers = await c.Offers.SearchOffersAsync(new OfferSearchParams { Limit = 1 });
            var offerId = offers.Offers.FirstOrDefault()?.Id;
            if (offerId is null) { Console.WriteLine("  No offers found"); return; }

            var rating = await c.Offers.GetOfferRatingAsync(offerId);
            Console.WriteLine($"  Offer {offerId[..8]}…");
            Console.WriteLine($"  Average : {rating.AverageRating}");
            Console.WriteLine($"  Count   : {rating.RatingCount ?? 0}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 4 — Order Management
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group4_OrderManagement(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 4 — Order Management");

        await RunDemo("4a. Recent Orders (5)", async () =>
        {
            var result = await c.Orders.GetOrdersAsync(new OrderSearchParams { Limit = 5 });
            Console.WriteLine($"  Total orders: {result.TotalCount}");
            foreach (var o in result.CheckoutForms.Take(5))
                Console.WriteLine($"  {o.Id[..8]}… [{o.Status,-18}] " +
                                  $"{o.Summary?.TotalToPay?.Amount,8} {o.Summary?.TotalToPay?.Currency}  " +
                                  $"buyer: {o.Buyer?.Login}");
        });

        await RunDemo("4b. Order Detail (first order)", async () =>
        {
            var orders = await c.Orders.GetOrdersAsync(new OrderSearchParams { Limit = 1 });
            var first = orders.CheckoutForms.FirstOrDefault();
            if (first is null) { Console.WriteLine("  No orders found"); return; }

            var o = await c.Orders.GetOrderAsync(first.Id);
            Console.WriteLine($"  Order   : {o.Id}");
            Console.WriteLine($"  Buyer   : {o.Buyer?.Login} ({o.Buyer?.Email})");
            Console.WriteLine($"  Status  : {o.Status}");
            Console.WriteLine($"  Items   : {o.LineItems?.Count}");
            Console.WriteLine($"  Delivery: {o.Delivery?.Method?.Name}  cost: {o.Delivery?.Cost?.Amount} {o.Delivery?.Cost?.Currency}");
            Console.WriteLine($"  Total   : {o.Summary?.TotalToPay?.Amount} {o.Summary?.TotalToPay?.Currency}");
        });

        await RunDemo("4c. Available Carriers", async () =>
        {
            var result = await c.Orders.GetCarriersAsync();
            Console.WriteLine($"  Carriers: {result.Carriers?.Count ?? 0}");
            foreach (var cr in result.Carriers?.Take(6) ?? Enumerable.Empty<Carrier>())
                Console.WriteLine($"  {cr.Id,-25} {cr.Name}");
        });

        await RunDemo("4d. Order Event Statistics", async () =>
        {
            var stats = await c.Orders.GetOrderEventsStatisticsAsync();
            Console.WriteLine($"  Latest event ID  : {stats.LatestEvent?.Id ?? "—"}");
            Console.WriteLine($"  Latest event time: {stats.LatestEvent?.OccurredAt:yyyy-MM-dd HH:mm}");
        });

        await RunDemo("4e. Allegro Pickup & Drop-Off Points (first 3)", async () =>
        {
            var result = await c.Orders.GetAllegroPickupDropOffPointsAsync();
            Console.WriteLine($"  Points: {result.Points?.Count ?? 0}");
            foreach (var pt in result.Points?.Take(3) ?? Enumerable.Empty<AllegroPickupDropOffPoint>())
                Console.WriteLine($"  {pt.Id,-20} [{pt.Type,-10}] {pt.Name?.Truncate(30)}");
        });

        await RunDemo("4f. Order Events Feed (5)", async () =>
        {
            var result = await c.Orders.GetOrderEventsAsync(limit: 5);
            Console.WriteLine($"  Events: {result.Events?.Count ?? 0}");
            foreach (var ev in result.Events?.Take(5) ?? Enumerable.Empty<OrderEvent>())
                Console.WriteLine($"  {ev.OccurredAt:HH:mm:ss}  {ev.Type,-32}  order: {ev.Order?.CheckoutForm?.Id?[..8]}…");
        });

        await RunDemo("4g. Shipments for First Order", async () =>
        {
            var orders = await c.Orders.GetOrdersAsync(new OrderSearchParams { Limit = 1 });
            var orderId = orders.CheckoutForms.FirstOrDefault()?.Id;
            if (orderId is null) { Console.WriteLine("  No orders found"); return; }

            var ship = await c.Orders.GetOrderShipmentsAsync(orderId);
            Console.WriteLine($"  Order {orderId[..8]}… — waybills: {ship.WaybillCreated?.Count ?? 0}");
            foreach (var w in ship.WaybillCreated?.Take(3) ?? Enumerable.Empty<CheckoutFormWaybill>())
                Console.WriteLine($"  carrier: {w.CarrierId,-20} tracking: {w.WaybillId}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 5 — Payments & Billing
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group5_PaymentsAndBilling(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 5 — Payments & Billing");

        await RunDemo("5a. Payment Operations History (10)", async () =>
        {
            var result = await c.Payments.GetPaymentOperationsHistoryAsync(limit: 10);
            Console.WriteLine($"  Operations: {result.Count ?? 0}  " +
                              $"total: {result.TotalValue?.Amount} {result.TotalValue?.Currency}");
            foreach (var op in result.PaymentOperations?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Payments.PaymentOperationHistory>())
                Console.WriteLine($"  {op.OccurredAt:yyyy-MM-dd}  {op.Type,-18} {op.Value?.Amount,10} {op.Value?.Currency}  " +
                                  $"participant: {op.Participant?.Login}");
        });

        await RunDemo("5b. Billing Entries (10)", async () =>
        {
            var result = await c.Billing.GetBillingEntriesAsync(limit: 10);
            Console.WriteLine($"  Entries: {result.Count ?? 0}");
            foreach (var e in result.BillingEntriesList?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Billing.BillingEntry>())
                Console.WriteLine($"  {e.OccurredAt:yyyy-MM-dd}  {e.Type?.Truncate(28),-28} {e.Value?.Amount,10} {e.Value?.Currency}");
        });

        await RunDemo("5c. Billing Types", async () =>
        {
            var result = await c.Billing.GetBillingTypesAsync();
            Console.WriteLine($"  Types: {result.BillingTypes?.Count ?? 0}");
            foreach (var t in result.BillingTypes?.Take(8) ?? Enumerable.Empty<AllegroApi.Models.Billing.BillingTypeInfo>())
                Console.WriteLine($"  {t.Id?.Truncate(35),-35} {t.Name}");
        });

        await RunDemo("5d. Invoices (5)", async () =>
        {
            var result = await c.Billing.GetInvoicesAsync(limit: 5);
            Console.WriteLine($"  Invoices: {result.Count ?? 0}");
            foreach (var inv in result.Invoices?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Billing.Invoice>())
                Console.WriteLine($"  {inv.IssuedAt:yyyy-MM-dd}  {inv.Number,-22} {inv.TotalValue?.Amount,10} {inv.TotalValue?.Currency}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 6 — Shipping Setup
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group6_ShippingSetup(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 6 — Shipping Setup");

        await RunDemo("6a. Delivery Methods", async () =>
        {
            var result = await c.Shipping.GetDeliveryMethodsAsync();
            Console.WriteLine($"  Methods: {result.DeliveryMethods?.Count ?? 0}");
            foreach (var m in result.DeliveryMethods?.Take(6) ?? Enumerable.Empty<AllegroApi.Models.Shipping.DeliveryMethod>())
                Console.WriteLine($"  {m.Id?.Truncate(35),-35} {m.Name?.Truncate(28),-28} dispatch: {m.ShippingTime?.From}–{m.ShippingTime?.To}d");
        });

        await RunDemo("6b. Delivery Settings", async () =>
        {
            var r = await c.Shipping.GetDeliverySettingsAsync();
            Console.WriteLine($"  Free delivery threshold: {r.FreeDelivery?.Amount} {r.FreeDelivery?.Currency}");
            Console.WriteLine($"  Join policy            : {r.JoinPolicy?.Strategy ?? "—"}");
            Console.WriteLine($"  Custom cost allowed    : {r.CustomCost?.Allowed}");
            Console.WriteLine($"  Last updated           : {r.UpdatedAt:yyyy-MM-dd}");
        });

        await RunDemo("6c. Seller Shipping Rate Sets", async () =>
        {
            var result = await c.Shipping.GetShippingRatesAsync();
            Console.WriteLine($"  Rate sets: {result.ShippingRates?.Count ?? 0}");
            foreach (var r in result.ShippingRates?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Shipping.ShippingRateSummary>())
                Console.WriteLine($"  {r.Id?[..8]}…  {r.Name?.Truncate(30),-30}  managed: {r.Features?.ManagedByAllegro}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 7 — After-Sales Conditions
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group7_AfterSales(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 7 — After-Sales Conditions");

        await RunDemo("7a. Return Policies", async () =>
        {
            var result = await c.AfterSales.GetReturnPoliciesAsync();
            Console.WriteLine($"  Policies: {result.ReturnPolicies?.Count ?? 0}");
            foreach (var p in result.ReturnPolicies?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.AfterSales.ReturnPolicyResponse>())
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(30),-30}  {p.ReturnPeriod}d  cost: {p.ShippingCosts}");
        });

        await RunDemo("7b. Warranties", async () =>
        {
            var result = await c.AfterSales.GetWarrantiesAsync();
            Console.WriteLine($"  Warranties: {result.Warranties?.Count ?? 0}");
            foreach (var w in result.Warranties?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.AfterSales.WarrantyResponse>())
                Console.WriteLine($"  {w.Id?[..8]}…  {w.Name?.Truncate(30),-30}  {w.Period}m");
        });

        await RunDemo("7c. Implied Warranties", async () =>
        {
            var result = await c.AfterSales.GetImpliedWarrantiesAsync();
            Console.WriteLine($"  Implied warranties: {result.ImpliedWarranties?.Count ?? 0}");
            foreach (var w in result.ImpliedWarranties?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.AfterSales.ImpliedWarrantyResponse>())
                Console.WriteLine($"  {w.Id?[..8]}…  {w.Name?.Truncate(30),-30}  {w.Period}m");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 8 — Messaging Center
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group8_MessagingCenter(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 8 — Messaging Center");

        await RunDemo("8a. Message Threads (5)", async () =>
        {
            var result = await c.Messaging.GetThreadsAsync(limit: 5);
            Console.WriteLine($"  Threads: {result.Count ?? 0}");
            foreach (var t in result.Threads?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Messaging.MessageThread>())
                Console.WriteLine($"  {t.Id?[..8]}…  unread: {t.Unread,-5}  " +
                                  $"with: {t.Interlocutor?.Login,-18}  " +
                                  $"last: {t.LastMessage?.Text?.Truncate(30)}");
        });

        await RunDemo("8b. Messages in First Thread (3)", async () =>
        {
            var threads = await c.Messaging.GetThreadsAsync(limit: 1);
            var first = threads.Threads?.FirstOrDefault();
            if (first?.Id is null) { Console.WriteLine("  No threads found"); return; }

            var msgs = await c.Messaging.GetMessagesAsync(first.Id, limit: 5);
            Console.WriteLine($"  Thread {first.Id[..8]}… — messages: {msgs.Count ?? 0}");
            foreach (var m in msgs.Messages?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.Messaging.Message>())
                Console.WriteLine($"  [{m.CreatedAt:HH:mm}] {m.Author?.Login,-15}: {m.Text?.Truncate(55)}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 9 — Disputes, Returns & Post-Purchase
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group9_DisputesAndReturns(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 9 — Disputes, Returns & Post-Purchase");

        await RunDemo("9a. Disputes (5)", async () =>
        {
            var result = await c.Disputes.GetDisputesAsync(limit: 5);
            Console.WriteLine($"  Disputes: {result.Count ?? 0}");
            foreach (var d in result.Disputes?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Disputes.Dispute>())
                Console.WriteLine($"  {d.Id?[..8]}…  [{d.Status,-12}]  {d.Type,-18}  buyer: {d.Buyer?.Login}");
        });

        await RunDemo("9b. Customer Returns (5)", async () =>
        {
            var result = await c.CustomerReturns.GetCustomerReturnsAsync(limit: 5);
            Console.WriteLine($"  Returns: {result.CustomerReturns?.Count ?? 0}");
            foreach (var r in result.CustomerReturns?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.CustomerReturns.CustomerReturn>())
                Console.WriteLine($"  {r.Id?[..8]}…  [{r.Status,-14}]  buyer: {r.Buyer?.Login}  items: {r.Items?.Count ?? 0}");
        });

        await RunDemo("9c. Post-Purchase Issues (5)", async () =>
        {
            var result = await c.PostPurchaseIssues.GetIssuesAsync(limit: 5);
            Console.WriteLine($"  Issues: {result.Issues?.Count ?? 0}");
            foreach (var i in result.Issues?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.PostPurchaseIssues.PostPurchaseIssue>())
                Console.WriteLine($"  {i.Id?[..8]}…  [{i.Status,-14}]  type: {i.Type}");
        });

        await RunDemo("9d. Commission Refund Claims (5)", async () =>
        {
            var result = await c.RefundClaims.GetRefundClaimsAsync(limit: 5);
            Console.WriteLine($"  Claims: {result.RefundClaims?.Count ?? 0}");
            foreach (var r in result.RefundClaims?.Take(5) ?? Enumerable.Empty<RefundClaim>())
                Console.WriteLine($"  {r.Id?[..8]}…  [{r.Status,-14}]  buyer: {r.Buyer?.Login}  " +
                                  $"refund: {r.Refund?.Amount} {r.Refund?.Currency}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 10 — Fulfillment Services
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group10_Fulfillment(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 10 — Fulfillment Services");

        await RunDemo("10a. Fulfillment Stock (5)", async () =>
        {
            var result = await c.Fulfillment.GetFulfillmentStockAsync(limit: 5);
            Console.WriteLine($"  Total products in stock: {result.TotalCount}");
            foreach (var p in result.Products.Take(5))
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(35),-35} " +
                                  $"avail: {p.Available,5}  reserved: {p.Reserved,5}  " +
                                  $"storage fee: {p.StorageFee?.Status}");
        });

        await RunDemo("10b. Available Products for Fulfillment (5)", async () =>
        {
            var result = await c.Fulfillment.GetAvailableProductsAsync(limit: 5);
            Console.WriteLine($"  Available: {result.TotalCount}");
            foreach (var p in result.Products.Take(5))
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(40),-40}  GTIN: {p.Gtin ?? "—"}");
        });

        await RunDemo("10c. Advance Ship Notices (5)", async () =>
        {
            var result = await c.Fulfillment.GetAdvanceShipNoticesAsync(limit: 5);
            Console.WriteLine($"  ASNs total: {result.TotalCount}");
            foreach (var a in result.AdvanceShipNotices.Take(5))
                Console.WriteLine($"  {a.DisplayNumber,-14}  [{a.Status,-18}]  items: {a.Items?.Count ?? 0}  " +
                                  $"submitted: {a.SubmittedAt:yyyy-MM-dd}");
        });

        await RunDemo("10d. Removal Preferences", async () =>
        {
            var pref = await c.Fulfillment.GetRemovalPreferencesAsync();
            Console.WriteLine($"  Type    : {pref.Type}");
            Console.WriteLine($"  Address : {pref.DeliveryAddress?.Street}, " +
                              $"{pref.DeliveryAddress?.City} {pref.DeliveryAddress?.PostCode}");
        });

        await RunDemo("10e. Fulfillment Tax ID", async () =>
        {
            var tax = await c.Fulfillment.GetTaxIdAsync();
            Console.WriteLine($"  Tax ID             : {tax.TaxId}");
            Console.WriteLine($"  Verification status: {tax.VerificationStatus}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 11 — Shipment Management
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group11_ShipmentManagement(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 11 — Shipment Management");

        await RunDemo("11a. Delivery Services", async () =>
        {
            var result = await c.ShipmentManagement.GetDeliveryServicesAsync();
            Console.WriteLine($"  Services: {result.DeliveryServices?.Count ?? 0}");
            foreach (var s in result.DeliveryServices?.Take(8) ?? Enumerable.Empty<AllegroApi.Models.ShipmentManagement.DeliveryServiceDto>())
                Console.WriteLine($"  {s.Id?.Truncate(30),-30} carrier: {s.Carrier,-12} type: {s.Type}");
        });

        await RunDemo("11b. Delivery Proposals (first order)", async () =>
        {
            var orders = await c.Orders.GetOrdersAsync(new OrderSearchParams { Limit = 1 });
            var orderId = orders.CheckoutForms.FirstOrDefault()?.Id;
            if (orderId is null) { Console.WriteLine("  No orders available"); return; }

            var proposals = await c.ShipmentManagement.GetDeliveryProposalsAsync(orderId);
            Console.WriteLine($"  Order: {orderId[..8]}…");
            Console.WriteLine($"  Delivery options : {proposals.DeliveryOptions?.Count ?? 0}");
            foreach (var opt in proposals.DeliveryOptions?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.ShipmentManagement.DeliveryOptionDto>())
                Console.WriteLine($"  {opt.Owner?.Truncate(20),-20}  {opt.PickupDate}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 12 — Sale Extensions & Promotions
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group12_SaleExtensions(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 12 — Sale Extensions & Promotions");

        await RunDemo("12a. Offer Tags", async () =>
        {
            var result = await c.SaleExtensions.GetOfferTagsAsync();
            Console.WriteLine($"  Tags: {result.Tags?.Count ?? 0}");
            foreach (var t in result.Tags?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.OfferTag>())
                Console.WriteLine($"  {t.Id?[..8]}…  {t.Name?.Truncate(25),-25}  offers: {t.OffersCount ?? 0}");
        });

        await RunDemo("12b. Loyalty Promotions", async () =>
        {
            var result = await c.SaleExtensions.GetLoyaltyPromotionsAsync();
            Console.WriteLine($"  Promotions: {result.Promotions?.Count ?? 0}");
            foreach (var p in result.Promotions?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.LoyaltyPromotion>())
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(28),-28}  type: {p.Type}  " +
                                  $"{p.StartDate:yyyy-MM-dd}–{p.EndDate:yyyy-MM-dd}");
        });

        await RunDemo("12c. Offer Bundles", async () =>
        {
            var result = await c.SaleExtensions.GetBundlesAsync();
            Console.WriteLine($"  Bundles: {result.Bundles?.Count ?? 0}");
            foreach (var b in result.Bundles?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.Bundle>())
                Console.WriteLine($"  {b.Id?[..8]}…  {b.Name?.Truncate(28),-28}  " +
                                  $"offers: {b.Offers?.Count ?? 0}  discount: {b.Discount?.Percentage}%");
        });

        await RunDemo("12d. Turnover Discounts", async () =>
        {
            var result = await c.SaleExtensions.GetTurnoverDiscountsAsync();
            Console.WriteLine($"  Discount definitions: {result?.Count ?? 0}");
            foreach (var d in result?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.TurnoverDiscountDto>())
                Console.WriteLine($"  Marketplace: {d.MarketplaceId,-22} status: {d.Status}  " +
                                  $"thresholds: {d.Definitions?.FirstOrDefault()?.Thresholds?.Count ?? 0}");
        });

        await RunDemo("12e. Promotion Packages", async () =>
        {
            var result = await c.SaleExtensions.GetPromotionPackagesAsync();
            Console.WriteLine($"  Base  : {result.BasePackages?.Count ?? 0}  Extra: {result.ExtraPackages?.Count ?? 0}");
            foreach (var p in result.BasePackages?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.AvailablePromotionPackage>())
                Console.WriteLine($"  {p.Id?.Truncate(30),-30} {p.Name?.Truncate(20),-20} cycle: {p.CycleDuration}");
        });

        await RunDemo("12f. Additional Services Categories", async () =>
        {
            var result = await c.SaleExtensions.GetAdditionalServicesCategoriesAsync();
            Console.WriteLine($"  Categories: {result.Categories?.Count ?? 0}");
            foreach (var cat in result.Categories?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.CategoryResponse>())
                Console.WriteLine($"  {cat.Name?.Truncate(30),-30}  definitions: {cat.Definitions?.Count ?? 0}");
        });

        await RunDemo("12g. Promo Options for Seller Offers (first 3)", async () =>
        {
            var result = await c.SaleExtensions.GetPromoOptionsForSellerOffersAsync(limit: 3);
            Console.WriteLine($"  Total offers with promo options: {result.TotalCount ?? 0}");
            foreach (var o in result.PromoOptions?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.OfferPromoOptions>())
                Console.WriteLine($"  offer: {o.OfferId?[..8]}…  marketplace: {o.MarketplaceId}  " +
                                  $"base package: {o.BasePackage?.Id?.Truncate(20)}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 13 — Allegro Prices & Price Automation
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group13_AllegroPricesAndAutomation(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 13 — Allegro Prices & Price Automation");

        await RunDemo("13a. Allegro Prices Eligibility", async () =>
        {
            var r = await c.AllegroPrices.GetAccountEligibilityAsync();
            Console.WriteLine($"  Eligible   : {r.Eligible}");
            Console.WriteLine($"  Reason code: {r.ReasonCode ?? "—"}");
            Console.WriteLine($"  Message    : {r.Message?.Truncate(80) ?? "—"}");
        });

        await RunDemo("13b. Allegro Prices Account Consent", async () =>
        {
            var r = await c.AllegroPrices.GetAccountConsentAsync();
            Console.WriteLine($"  Global consent: {r.Status}");
            if (r.Marketplaces != null)
                foreach (var kv in r.Marketplaces.Take(5))
                    Console.WriteLine($"    {kv.Key,-22} {kv.Value?.Status}");
        });

        await RunDemo("13c. Alle Discount Campaigns", async () =>
        {
            var r = await c.AllegroPrices.GetCampaignsAsync();
            Console.WriteLine($"  Campaigns: {r.Campaigns?.Count ?? 0}");
            foreach (var camp in r.Campaigns?.Take(5) ?? Enumerable.Empty<AlleDiscountCampaign>())
                Console.WriteLine($"  {camp.Id?[..8]}…  [{camp.Status,-10}]  {camp.Name?.Truncate(35),-35}  {camp.StartDate}");
        });

        await RunDemo("13d. Allegro Prices Account Participation", async () =>
        {
            var r = await c.AllegroPrices.GetAccountParticipationAsync();
            Console.WriteLine($"  Marketplace participation entries: {r.Marketplaces?.Count ?? 0}");
            foreach (var m in r.Marketplaces?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Pricing.AccountParticipationMarketplace>())
                Console.WriteLine($"  {m.MarketplaceId,-22} status: {m.ParticipationStatus}");
        });

        await RunDemo("13e. Price Automation Rules", async () =>
        {
            var r = await c.PriceAutomation.GetRulesAsync();
            Console.WriteLine($"  Rules: {r.Rules?.Count ?? 0}");
            foreach (var rule in r.Rules?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.PriceAutomation.AutomaticPricingRuleResponse>())
                Console.WriteLine($"  {rule.Id?[..8]}…  [{rule.Type?.Truncate(18),-18}]  {rule.Name?.Truncate(35),-35}  default: {rule.Default}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 14 — Infrastructure Clients
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group14_InfrastructureClients(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 14 — Infrastructure");

        await RunDemo("14a. Contacts", async () =>
        {
            var r = await c.Contacts.GetContactsAsync();
            Console.WriteLine($"  Contacts: {r.Contacts?.Count ?? 0}");
            foreach (var ct in r.Contacts?.Take(5) ?? Enumerable.Empty<ContactResponse>())
                Console.WriteLine($"  {ct.Id?[..8]}…  {ct.Name?.Truncate(25),-25}  {ct.PhoneNumber}  {ct.Email}");
        });

        await RunDemo("14b. Additional Emails", async () =>
        {
            var r = await c.AdditionalEmails.GetAdditionalEmailsAsync();
            Console.WriteLine($"  Additional emails: {r.AdditionalEmails?.Count ?? 0}");
            foreach (var e in r.AdditionalEmails?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Account.AdditionalEmail>())
                Console.WriteLine($"  {e.Id,-36}  {e.Email}");
        });

        await RunDemo("14c. Points of Service", async () =>
        {
            var r = await c.PointsOfService.GetPointsOfServiceAsync();
            Console.WriteLine($"  POS locations: {r.PointsOfService?.Count ?? 0}");
            foreach (var p in r.PointsOfService?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.PointsOfService.PointOfService>())
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(28),-28}  {p.Address?.City}  type: {p.Type}");
        });

        await RunDemo("14d. Size Tables", async () =>
        {
            var r = await c.SizeTables.GetSizeTablesAsync();
            Console.WriteLine($"  Size tables: {r.Tables?.Count ?? 0}");
            foreach (var t in r.Tables?.Take(5) ?? Enumerable.Empty<PublicTableDto>())
                Console.WriteLine($"  {t.Id?[..8]}…  {t.Name?.Truncate(28),-28}  rows: {t.Rows?.Count ?? 0}");
        });

        await RunDemo("14e. Size Table Templates", async () =>
        {
            var r = await c.SizeTables.GetSizeTableTemplatesAsync();
            Console.WriteLine($"  Templates: {r.Templates?.Count ?? 0}");
            foreach (var t in r.Templates.Take(5))
                Console.WriteLine($"  {t.Id?[..8]}…  {t.Name?.Truncate(28),-28}  headers: {t.Headers?.Count ?? 0}");
        });

        await RunDemo("14f. Responsible Persons (EU GPSR)", async () =>
        {
            var r = await c.ResponsiblePersons.GetResponsiblePersonsAsync(limit: 5);
            Console.WriteLine($"  Persons: {r.ResponsiblePersons?.Count ?? 0}");
            foreach (var p in r.ResponsiblePersons?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.ResponsiblePersons.ResponsiblePersonResponse>())
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(25),-25}  {p.PersonalData?.Contact?.Email}");
        });

        await RunDemo("14g. Responsible Producers (EU GPSR)", async () =>
        {
            var r = await c.ResponsibleProducers.GetResponsibleProducersAsync(limit: 5);
            Console.WriteLine($"  Producers: {r.ResponsibleProducers?.Count ?? 0}");
            foreach (var p in r.ResponsibleProducers?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.ResponsiblePersons.ResponsibleProducerResponse>())
                Console.WriteLine($"  {p.Id?[..8]}…  {p.Name?.Truncate(28),-28}  trade name: {p.ProducerData?.TradeName?.Truncate(25)}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 15 — Miscellaneous & Discovery
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group15_MiscellaneousAndDiscovery(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 15 — Miscellaneous & Discovery");

        await RunDemo("15a. Bidding Offers (active auctions)", async () =>
        {
            var r = await c.Miscellaneous.GetBiddingOffersAsync();
            Console.WriteLine($"  Bidding offers: {r.Count ?? 0}");
            foreach (var o in r.Offers?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Miscellaneous.BiddingOffer>())
                Console.WriteLine($"  {o.Id?[..8]}…  {o.Name?.Truncate(35),-35}  bid: {o.CurrentBid}  ends: {o.EndTime:HH:mm dd-MM}");
        });

        await RunDemo("15b. Deposit Types", async () =>
        {
            var r = await c.Miscellaneous.GetDepositTypesAsync();
            Console.WriteLine($"  Types: {r.Deposits?.Count ?? 0}");
            foreach (var d in r.Deposits?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Miscellaneous.DepositType>())
                Console.WriteLine($"  {d.Id?.Truncate(30),-30} {d.Name?.Truncate(20),-20} {d.Price?.Amount} {d.Price?.Currency}");
        });

        await RunDemo("15c. Deposit Account Info", async () =>
        {
            var info = await c.Miscellaneous.GetDepositInfoAsync();
            Console.WriteLine($"  Balance  : {info.Balance} {info.Currency}");
            Console.WriteLine($"  Available: {info.Available} {info.Currency}");
            Console.WriteLine($"  Reserved : {info.Reserved} {info.Currency}");
        });

        await RunDemo("15d. Charity Campaigns (5)", async () =>
        {
            var r = await c.Miscellaneous.GetCharityCampaignsAsync();
            Console.WriteLine($"  Campaigns: {r.Campaigns?.Count ?? 0}");
            foreach (var camp in r.Campaigns?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Miscellaneous.CharityCampaign>())
                Console.WriteLine($"  {camp.Id?[..8]}…  {camp.Name?.Truncate(35),-35}  org: {camp.Organization?.Truncate(20)}");
        });

        await RunDemo("15e. Offer Events Feed (5)", async () =>
        {
            var r = await c.Miscellaneous.GetOfferEventsAsync(limit: 5);
            Console.WriteLine($"  Offer events: {r.OfferEvents?.Count ?? 0}");
            foreach (var ev in r.OfferEvents?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Miscellaneous.SellerOfferEvent>())
                Console.WriteLine($"  {ev.OccurredAt:HH:mm:ss}  {ev.Type?.Truncate(30),-30}  offer: {ev.OfferId?[..8]}…");
        });

        await RunDemo("15f. Compatibility Supported Categories", async () =>
        {
            var r = await c.Miscellaneous.GetCompatibilityListSupportedCategoriesAsync();
            Console.WriteLine($"  Categories: {r.SupportedCategories?.Count ?? 0}");
            foreach (var cat in r.SupportedCategories?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.Miscellaneous.CompatibilitySupportedCategory>())
                Console.WriteLine($"  {cat.CategoryId,-12} {cat.Name?.Truncate(28),-28} type: {cat.ItemsType}");
        });

        await RunDemo("15g. Badge Campaigns", async () =>
        {
            var r = await c.Badges.GetBadgeCampaignsAsync();
            Console.WriteLine($"  Campaigns: {r.Campaigns?.Count ?? 0}");
            foreach (var camp in r.Campaigns?.Take(5) ?? Enumerable.Empty<AllegroApi.Models.SaleExtensions.BadgeCampaign>())
                Console.WriteLine($"  {camp.Id?[..8]}…  [{camp.Status,-10}]  {camp.Name?.Truncate(35)}");
        });

        await RunDemo("15h. Classifieds Package Configs (first category)", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            var catId = cats?.Categories?.FirstOrDefault()?.Id;
            if (catId is null) { Console.WriteLine("  No category found"); return; }

            var r = await c.Classifieds.GetClassifiedPackageConfigurationsAsync(catId);
            Console.WriteLine($"  Packages for category '{catId}': {r.Packages?.Count ?? 0}");
            foreach (var p in r.Packages?.Take(3) ?? Enumerable.Empty<AllegroApi.Models.Classifieds.ClassifiedPackageConfig>())
                Console.WriteLine($"  {p.Id?.Truncate(30),-30}  {p.Name}");
        });

        await RunDemo("15i. Affiliate Program Info", async () =>
        {
            var info = await c.Miscellaneous.GetAffiliateProgramInfoAsync();
            Console.WriteLine($"  Name           : {info.Name ?? "—"}");
            Console.WriteLine($"  Active         : {info.Active}");
            Console.WriteLine($"  Commission rate: {info.CommissionRate}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 16 — Public Listing & Search
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group16_PublicListing(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 16 — Public Listing & Search");

        await RunDemo("16a. Search by Phrase ('gaming laptop')", async () =>
        {
            var r = await c.Listing.SearchByPhraseAsync("gaming laptop", limit: 5);
            Console.WriteLine($"  Promoted: {r.Items?.Promoted?.Count ?? 0}  Regular: {r.Items?.Regular?.Count ?? 0}");
            foreach (var o in r.Items?.Promoted?.Take(3) ?? r.Items?.Regular?.Take(3) ?? Enumerable.Empty<ListingOffer>())
                Console.WriteLine($"  {o.Id?[..8]}…  {o.Name?.Truncate(40),-40} {o.SellingMode?.Price?.Amount,8} {o.SellingMode?.Price?.Currency}");
        });

        await RunDemo("16b. Search by Category (first root cat)", async () =>
        {
            var cats = await c.Categories.GetCategoriesAsync();
            var catId = cats?.Categories?.FirstOrDefault()?.Id ?? "1";
            var r = await c.Listing.SearchByCategoryAsync(catId, limit: 5);
            Console.WriteLine($"  Category '{catId}' — promoted: {r.Items?.Promoted?.Count ?? 0}  " +
                              $"regular: {r.Items?.Regular?.Count ?? 0}");
            foreach (var o in r.Items?.Regular?.Take(3) ?? Enumerable.Empty<ListingOffer>())
                Console.WriteLine($"  {o.Id?[..8]}…  {o.Name?.Truncate(40),-40} {o.SellingMode?.Price?.Amount,8} {o.SellingMode?.Price?.Currency}");
        });

        await RunDemo("16c. Search by Seller (self)", async () =>
        {
            var me = await c.Account.GetAccountInfoAsync();
            if (me.Id is null) { Console.WriteLine("  No account ID"); return; }

            var r = await c.Listing.SearchBySellerAsync(me.Id, limit: 5);
            Console.WriteLine($"  My listings — promoted: {r.Items?.Promoted?.Count ?? 0}  regular: {r.Items?.Regular?.Count ?? 0}");
            var items = r.Items?.Promoted ?? r.Items?.Regular ?? new List<ListingOffer>();
            foreach (var o in items.Take(3))
                Console.WriteLine($"  {o.Id?[..8]}…  {o.Name?.Truncate(40),-40} {o.SellingMode?.Price?.Amount,8} {o.SellingMode?.Price?.Currency}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 17 — Image Upload
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group17_ImageUpload(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 17 — Image Upload");

        await RunDemo("17a. Upload Image from URL", async () =>
        {
            var url = "https://via.placeholder.com/800x600.png?text=AllegroSDK";
            Console.WriteLine($"  Uploading: {url}");
            var r = await c.Images.UploadImageFromUrlAsync(url);
            Console.WriteLine($"  Location : {r?.Location}");
            Console.WriteLine($"  Expires  : {r?.ExpiresAt:yyyy-MM-dd HH:mm}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // GROUP 18 — Error Handling
    // ══════════════════════════════════════════════════════════════════════════
    static async Task Group18_ErrorHandling(AllegroApiClient c)
    {
        PrintGroupHeader("GROUP 18 — Error Handling");

        await RunDemo("18a. 404 Not Found", async () =>
        {
            try
            {
                await c.Offers.GetProductOfferAsync("00000000-0000-0000-0000-000000000000");
                Console.WriteLine("  (no exception — unexpected)");
            }
            catch (AllegroNotFoundException ex)
            {
                Console.WriteLine($"  ✓ AllegroNotFoundException: {ex.Message.Truncate(80)}");
            }
        });

        await RunDemo("18b. Bad Category ID", async () =>
        {
            try
            {
                await c.Categories.GetCategoryParametersAsync("INVALID_CATEGORY_99999999");
                Console.WriteLine("  (no exception)");
            }
            catch (AllegroNotFoundException ex)
            {
                Console.WriteLine($"  ✓ AllegroNotFoundException: {ex.Message.Truncate(80)}");
            }
            catch (AllegroApiException ex)
            {
                Console.WriteLine($"  ✓ {ex.GetType().Name}: {ex.Message.Truncate(80)}");
            }
        });

        await RunDemo("18c. Exception Hierarchy Reference", async () =>
        {
            var hierarchy = new[]
            {
                "AllegroApiException          — base; wraps all SDK errors",
                "AllegroAuthenticationException  — 401 Unauthorized",
                "AllegroAuthorizationException   — 403 Forbidden",
                "AllegroNotFoundException         — 404 Not Found",
                "AllegroBadRequestException        — 400 + validation detail list",
                "AllegroConflictException          — 409 Conflict",
                "AllegroUnprocessableEntityException — 422",
                "AllegroRateLimitException         — 429 + RetryAfterSeconds",
                "AllegroServerException            — 5xx server errors",
                "AllegroNetworkException           — transport / DNS failures",
                "AllegroTimeoutException           — request exceeded TimeoutSeconds",
            };
            foreach (var line in hierarchy)
                Console.WriteLine($"  {line}");
        });
    }

    // ══════════════════════════════════════════════════════════════════════════
    // HELPERS
    // ══════════════════════════════════════════════════════════════════════════

    static void PrintGroupHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine("╔" + new string('═', 64) + "╗");
        Console.WriteLine($"║  {title.PadRight(62)}║");
        Console.WriteLine("╚" + new string('═', 64) + "╝");
    }

    static async Task RunDemo(string name, Func<Task> body)
    {
        Console.WriteLine($"\n  ▶ {name}");
        try
        {
            await body();
            Console.WriteLine("  ✓ done");
        }
        catch (AllegroAuthenticationException ex)
        {
            Console.WriteLine($"  ✗ Auth: {ex.Message.Truncate(90)}");
        }
        catch (AllegroAuthorizationException ex)
        {
            Console.WriteLine($"  ✗ No permission: {ex.Message.Truncate(85)}");
        }
        catch (AllegroNotFoundException ex)
        {
            Console.WriteLine($"  ✗ Not found: {ex.Message.Truncate(88)}");
        }
        catch (AllegroApiException ex)
        {
            Console.WriteLine($"  ✗ API ({ex.GetType().Name}): {ex.Message.Truncate(80)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ {ex.GetType().Name}: {ex.Message.Truncate(90)}");
        }
    }

    static void PrintSetupInstructions()
    {
        Console.WriteLine("⚠️  API Token Not Found!\n");
        Console.WriteLine("Quick start:");
        Console.WriteLine("  export ALLEGRO_API_TOKEN=\"your-access-token\"");
        Console.WriteLine("  export USE_SANDBOX=\"true\"             # use sandbox");
        Console.WriteLine("  export DOTNET_ENVIRONMENT=Development  # verbose logging");
        Console.WriteLine("  dotnet run\n");
        Console.WriteLine("Credentials: https://developer.allegro.pl/");
    }
}

// ══════════════════════════════════════════════════════════════════════════════
// String helper — truncate for console display
// ══════════════════════════════════════════════════════════════════════════════
internal static class StringExtensions
{
    internal static string Truncate(this string? value, int max)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length <= max ? value : value[..max] + "…";
    }
}
