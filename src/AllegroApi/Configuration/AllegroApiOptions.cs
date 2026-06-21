namespace AllegroApi.Configuration;

/// <summary>
/// Configuration settings for Allegro API client
/// </summary>
public class AllegroApiOptions
{
    /// <summary>
    /// Base URL for Allegro API (default: https://api.allegro.pl)
    /// For sandbox environment use: https://api.allegro.pl.allegrosandbox.pl
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.allegro.pl";

    /// <summary>
    /// OAuth2 Bearer token for authentication
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Client ID for OAuth2 authentication
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Client Secret for OAuth2 authentication
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// OAuth2 token endpoint (default: https://allegro.pl/auth/oauth/token)
    /// For sandbox: https://allegro.pl.allegrosandbox.pl/auth/oauth/token
    /// </summary>
    public string TokenEndpoint { get; set; } = "https://allegro.pl/auth/oauth/token";

    /// <summary>
    /// Request timeout in seconds (default: 100)
    /// </summary>
    public int TimeoutSeconds { get; set; } = 100;

    /// <summary>
    /// Maximum number of retry attempts for failed requests (default: 3)
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Delay between retry attempts in milliseconds (default: 1000)
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Enable automatic token refresh (default: true)
    /// </summary>
    public bool EnableAutoTokenRefresh { get; set; } = true;

    /// <summary>
    /// The SDK version, derived from the assembly version so the User-Agent never drifts.
    /// </summary>
    private static readonly string SdkVersion =
        typeof(AllegroApiOptions).Assembly.GetName().Version?.ToString(3) ?? "2.3.0";

    /// <summary>
    /// User-Agent header value (default: AllegroApi-CSharp/{assembly version})
    /// </summary>
    public string UserAgent { get; set; } = $"AllegroApi-CSharp/{SdkVersion}";

    /// <summary>
    /// Accept-Language header value (default: en-US)
    /// Supported values: en-US, pl-PL, uk-UA, sk-SK, cs-CZ, hu-HU
    /// </summary>
    public string AcceptLanguage { get; set; } = "en-US";

    /// <summary>
    /// Enable request/response logging (default: false)
    /// </summary>
    public bool EnableLogging { get; set; } = false;

    /// <summary>
    /// Validate configuration
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
            throw new ArgumentException("BaseUrl is required", nameof(BaseUrl));

        if (string.IsNullOrWhiteSpace(AccessToken) && 
            (string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(ClientSecret)))
        {
            throw new ArgumentException(
                "Either AccessToken or both ClientId and ClientSecret must be provided");
        }

        if (TimeoutSeconds <= 0)
            throw new ArgumentException("TimeoutSeconds must be greater than 0", nameof(TimeoutSeconds));

        if (MaxRetryAttempts < 0)
            throw new ArgumentException("MaxRetryAttempts must be non-negative", nameof(MaxRetryAttempts));

        if (RetryDelayMilliseconds < 0)
            throw new ArgumentException("RetryDelayMilliseconds must be non-negative", nameof(RetryDelayMilliseconds));
    }
}

/// <summary>
/// Supported environments
/// </summary>
public enum AllegroEnvironment
{
    /// <summary>
    /// Production environment (api.allegro.pl)
    /// </summary>
    Production,
    
    /// <summary>
    /// Sandbox environment for testing (api.allegro.pl.allegrosandbox.pl)
    /// </summary>
    Sandbox
}

/// <summary>
/// Helper class to create configuration for different environments
/// </summary>
public static class AllegroApiOptionsExtensions
{
    /// <summary>
    /// Configures the API options for a specific environment.
    /// </summary>
    /// <param name="options">The API options to configure.</param>
    /// <param name="environment">The target environment (Production or Sandbox).</param>
    /// <returns>The configured API options.</returns>
    public static AllegroApiOptions ForEnvironment(
        this AllegroApiOptions options, 
        AllegroEnvironment environment)
    {
        switch (environment)
        {
            case AllegroEnvironment.Production:
                options.BaseUrl = "https://api.allegro.pl";
                options.TokenEndpoint = "https://allegro.pl/auth/oauth/token";
                break;
            case AllegroEnvironment.Sandbox:
                options.BaseUrl = "https://api.allegro.pl.allegrosandbox.pl";
                options.TokenEndpoint = "https://allegro.pl.allegrosandbox.pl/auth/oauth/token";
                break;
        }
        return options;
    }
}
