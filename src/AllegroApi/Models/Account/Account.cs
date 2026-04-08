using System.Text.Json.Serialization;

namespace AllegroApi.Models.Account;

/// <summary>
/// Account information response.
/// </summary>
public record AccountInfoResponse
{
    /// <summary>
    /// Account identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Account login/username.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; init; }

    /// <summary>
    /// Account email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Company name (for business accounts).
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// User first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// User last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    /// <summary>
    /// Account status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Account type (e.g., "BUSINESS", "INDIVIDUAL").
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Account creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Account preferences.
    /// </summary>
    [JsonPropertyName("preferences")]
    public AccountPreferences? Preferences { get; init; }

    /// <summary>
    /// Base marketplace information.
    /// </summary>
    [JsonPropertyName("baseMarketplace")]
    public BaseMarketplace? BaseMarketplace { get; init; }

    /// <summary>
    /// Company information (for business accounts).
    /// </summary>
    [JsonPropertyName("company")]
    public Company? Company { get; init; }

    /// <summary>
    /// User's features list (e.g., SUPER_SELLER, ONE_FULFILLMENT).
    /// </summary>
    [JsonPropertyName("features")]
    public List<string>? Features { get; init; }
}

/// <summary>
/// Account preferences.
/// </summary>
public record AccountPreferences
{
    /// <summary>
    /// Preferred language code.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }

    /// <summary>
    /// Preferred currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}

/// <summary>
/// Account settings.
/// </summary>
public record AccountSettings
{
    /// <summary>
    /// Indicates if notifications are enabled.
    /// </summary>
    [JsonPropertyName("notificationsEnabled")]
    public bool NotificationsEnabled { get; init; }

    /// <summary>
    /// Email notification preferences.
    /// </summary>
    [JsonPropertyName("emailNotifications")]
    public bool? EmailNotifications { get; init; }

    /// <summary>
    /// SMS notification preferences.
    /// </summary>
    [JsonPropertyName("smsNotifications")]
    public bool? SmsNotifications { get; init; }
}

/// <summary>
/// Base marketplace information.
/// </summary>
public record BaseMarketplace
{
    /// <summary>
    /// Base marketplace identifier (e.g., "allegro-pl").
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

/// <summary>
/// Company information for business accounts.
/// </summary>
public record Company
{
    /// <summary>
    /// Company name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Tax identification number.
    /// </summary>
    [JsonPropertyName("taxId")]
    public string? TaxId { get; init; }
}
