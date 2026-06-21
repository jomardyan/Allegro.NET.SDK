using AllegroApi;
using AllegroApi.Authentication;
using AllegroApi.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency-injection registration helpers for the Allegro API client.
/// </summary>
public static class AllegroApiServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="AllegroApiClient"/> as a typed client backed by
    /// <see cref="System.Net.Http.IHttpClientFactory"/>, wiring up OAuth2
    /// authentication (static token, or the client-credentials grant when only
    /// ClientId/ClientSecret are configured).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Delegate used to configure <see cref="AllegroApiOptions"/>.</param>
    /// <returns>
    /// The <see cref="IHttpClientBuilder"/> for the typed client, so callers can
    /// further customize the HttpClient (e.g. add Polly policies or extra handlers).
    /// </returns>
    public static IHttpClientBuilder AddAllegroApi(this IServiceCollection services, Action<AllegroApiOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.TryAddSingleton(_ =>
        {
            var options = new AllegroApiOptions();
            configure(options);
            options.Validate();
            return options;
        });

        services.TryAddSingleton<IAllegroTokenProvider>(sp =>
        {
            var options = sp.GetRequiredService<AllegroApiOptions>();
            if (!string.IsNullOrEmpty(options.AccessToken))
                return new StaticTokenProvider(options.AccessToken!);

            return new ClientCredentialsTokenProvider(
                options.TokenEndpoint, options.ClientId!, options.ClientSecret!);
        });

        services.TryAddTransient(sp =>
        {
            var options = sp.GetRequiredService<AllegroApiOptions>();
            var provider = sp.GetRequiredService<IAllegroTokenProvider>();
            return new AllegroAuthenticationHandler(provider, options.EnableAutoTokenRefresh);
        });

        return services
            .AddHttpClient<AllegroApiClient>()
            .AddHttpMessageHandler<AllegroAuthenticationHandler>();
    }
}
