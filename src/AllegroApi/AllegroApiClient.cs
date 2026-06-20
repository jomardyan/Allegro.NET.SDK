using AllegroApi.Clients;
using AllegroApi.Configuration;
using AllegroApi.Http;
using Microsoft.Extensions.Logging;

namespace AllegroApi;

/// <summary>
/// Main client for interacting with Allegro API
/// Provides access to all API endpoints through specialized clients
/// </summary>
public class AllegroApiClient : IDisposable
{
    private readonly AllegroHttpClient _httpClient;
    private readonly ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Client for offer management operations
    /// </summary>
    public OfferManagementClient Offers { get; }

    /// <summary>
    /// Client for dispute attachments (upload/download)
    /// </summary>
    public DisputeAttachmentsClient DisputeAttachments { get; }

    /// <summary>
    /// Client for product operations
    /// </summary>
    public ProductClient Products { get; }

    /// <summary>
    /// Client for order management operations
    /// </summary>
    public OrderManagementClient Orders { get; }

    /// <summary>
    /// Client for category operations
    /// </summary>
    public CategoryClient Categories { get; }

    /// <summary>
    /// Client for image upload operations
    /// </summary>
    public ImageClient Images { get; }

    /// <summary>
    /// Client for pricing and fee calculation operations
    /// </summary>
    public PricingClient Pricing { get; }

    /// <summary>
    /// Client for shipping rates and delivery settings operations
    /// </summary>
    public ShippingClient Shipping { get; }

    /// <summary>
    /// Client for after-sales service conditions (return policies, warranties)
    /// </summary>
    public AfterSalesClient AfterSales { get; }

    /// <summary>
    /// Client for account operations
    /// </summary>
    public AccountClient Account { get; }

    /// <summary>
    /// Client for points of service (pickup locations) operations
    /// </summary>
    public PointsOfServiceClient PointsOfService { get; }

    /// <summary>
    /// Client for payment operations
    /// </summary>
    public PaymentsClient Payments { get; }

    /// <summary>
    /// Client for buyer-seller messaging
    /// </summary>
    public MessagingClient Messaging { get; }

    /// <summary>
    /// Client for billing operations and invoices
    /// </summary>
    public BillingClient Billing { get; }

    /// <summary>
    /// Client for user ratings and reputation management
    /// </summary>
    public UsersClient Users { get; }

    /// <summary>
    /// Client for disputes and issue management
    /// </summary>
    public DisputesClient Disputes { get; }

    /// <summary>
    /// Client for sale extensions (bundles, loyalty promotions, offer tags)
    /// </summary>
    public SaleExtensionsClient SaleExtensions { get; }

    /// <summary>
    /// Client for advanced offer features (variants, attachments, smart offers)
    /// </summary>
    public AdvancedOffersClient AdvancedOffers { get; }

    /// <summary>
    /// Client for miscellaneous operations (charity, bidding, affiliate, deposits)
    /// </summary>
    public MiscellaneousClient Miscellaneous { get; }

    /// <summary>
    /// Client for shipment management (create shipments, labels, protocols, pickups)
    /// </summary>
    public ShipmentManagementClient ShipmentManagement { get; }

    /// <summary>
    /// Client for customer returns management
    /// </summary>
    public CustomerReturnsClient CustomerReturns { get; }

    /// <summary>
    /// Client for batch offer operations (bulk price/quantity/modification changes)
    /// </summary>
    public BatchOperationsClient BatchOperations { get; }

    /// <summary>
    /// Client for post-purchase issues (disputes and claims management)
    /// </summary>
    public PostPurchaseIssuesClient PostPurchaseIssues { get; }

    /// <summary>
    /// Client for commission refund claims management
    /// </summary>
    public RefundClaimsClient RefundClaims { get; }

    /// <summary>
    /// Client for managing additional email addresses
    /// </summary>
    public AdditionalEmailsClient AdditionalEmails { get; }

    /// <summary>
    /// Client for managing offer contacts
    /// </summary>
    public ContactsClient Contacts { get; }

    /// <summary>
    /// Client for managing size tables
    /// </summary>
    public SizeTablesClient SizeTables { get; }

    /// <summary>
    /// Client for managing responsible persons for EU GPSR compliance
    /// </summary>
    public ResponsiblePersonsClient ResponsiblePersons { get; }

    /// <summary>
    /// Client for managing responsible producers for EU GPSR compliance
    /// </summary>
    public ResponsibleProducersClient ResponsibleProducers { get; }

    /// <summary>
    /// Client for marketplace information operations
    /// </summary>
    public MarketplacesClient Marketplaces { get; }

    /// <summary>
    /// Client for Allegro Prices and Alle Discount operations
    /// </summary>
    public AllegroPricesClient AllegroPrices { get; }

    /// <summary>
    /// Client for Allegro Fulfillment services (warehouse management, ASN, stock)
    /// </summary>
    public FulfillmentClient Fulfillment { get; }

    /// <summary>
    /// Client for searching and browsing public offers
    /// </summary>
    public ListingClient Listing { get; }

    /// <summary>
    /// Client for badge campaigns and applications management
    /// </summary>
    public BadgesClient Badges { get; }

    /// <summary>
    /// Client for classifieds packages and promotions management
    /// </summary>
    public ClassifiedsClient Classifieds { get; }

    /// <summary>
    /// Client for automatic pricing (price automation) rules
    /// </summary>
    public PriceAutomationClient PriceAutomation { get; }

    /// <summary>
    /// Create a new instance of AllegroApiClient
    /// </summary>
    public AllegroApiClient(AllegroApiOptions options, ILoggerFactory? loggerFactory = null)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        options.Validate();

        _loggerFactory = loggerFactory;

        var httpClient = new HttpClient();
        _httpClient = new AllegroHttpClient(
            httpClient,
            options,
            loggerFactory?.CreateLogger<AllegroHttpClient>());

        // Initialize all clients
        Offers = new OfferManagementClient(_httpClient, loggerFactory?.CreateLogger<OfferManagementClient>());
        Products = new ProductClient(_httpClient, loggerFactory?.CreateLogger<ProductClient>());
        Orders = new OrderManagementClient(_httpClient, loggerFactory?.CreateLogger<OrderManagementClient>());
        Categories = new CategoryClient(_httpClient);
        Pricing = new PricingClient(_httpClient);
        Shipping = new ShippingClient(_httpClient);
        AfterSales = new AfterSalesClient(_httpClient);
        Account = new AccountClient(_httpClient);
        PointsOfService = new PointsOfServiceClient(_httpClient);
        Payments = new PaymentsClient(_httpClient);
        Messaging = new MessagingClient(_httpClient);
        Billing = new BillingClient(_httpClient);
        Users = new UsersClient(_httpClient);
        Disputes = new DisputesClient(_httpClient);
        SaleExtensions = new SaleExtensionsClient(_httpClient);
        AdvancedOffers = new AdvancedOffersClient(_httpClient);
        Miscellaneous = new MiscellaneousClient(_httpClient);
        ShipmentManagement = new ShipmentManagementClient(_httpClient);
        CustomerReturns = new CustomerReturnsClient(_httpClient);
        BatchOperations = new BatchOperationsClient(_httpClient);
        PostPurchaseIssues = new PostPurchaseIssuesClient(_httpClient);
        RefundClaims = new RefundClaimsClient(_httpClient);
        AdditionalEmails = new AdditionalEmailsClient(_httpClient);
        Contacts = new ContactsClient(_httpClient);
        SizeTables = new SizeTablesClient(_httpClient);
        ResponsiblePersons = new ResponsiblePersonsClient(_httpClient);
        ResponsibleProducers = new ResponsibleProducersClient(_httpClient);
        Marketplaces = new MarketplacesClient(_httpClient);
    AllegroPrices = new AllegroPricesClient(_httpClient);
    Fulfillment = new FulfillmentClient(_httpClient);
    Listing = new ListingClient(_httpClient);
    Badges = new BadgesClient(_httpClient);
    Classifieds = new ClassifiedsClient(_httpClient);
    DisputeAttachments = new DisputeAttachmentsClient(_httpClient);
    PriceAutomation = new PriceAutomationClient(_httpClient);

        // Image upload uses a different base URL (upload.allegro.pl)
        var uploadBaseUrl = options.BaseUrl.Contains("sandbox") 
            ? "https://upload.allegro.pl.allegrosandbox.pl" 
            : "https://upload.allegro.pl";
        Images = new ImageClient(_httpClient, uploadBaseUrl);
    }

    /// <summary>
    /// Create a client for production environment
    /// </summary>
    public static AllegroApiClient CreateProduction(string accessToken, ILoggerFactory? loggerFactory = null)
    {
        var options = new AllegroApiOptions
        {
            AccessToken = accessToken
        }.ForEnvironment(AllegroEnvironment.Production);

        return new AllegroApiClient(options, loggerFactory);
    }

    /// <summary>
    /// Create a client for sandbox environment
    /// </summary>
    public static AllegroApiClient CreateSandbox(string accessToken, ILoggerFactory? loggerFactory = null)
    {
        var options = new AllegroApiOptions
        {
            AccessToken = accessToken
        }.ForEnvironment(AllegroEnvironment.Sandbox);

        return new AllegroApiClient(options, loggerFactory);
    }

    /// <summary>
    /// Disposes the HTTP client resources.
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
