using AllegroApi.Http;
using AllegroApi.Models.ResponsiblePersons;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing responsible producers for EU GPSR compliance.
/// Responsible producers provide contact information for the company responsible for producing the product.
/// Read more: https://developer.allegro.pl/tutorials/account-and-user-data-management-jn9vBjqjnsw#responsible-producers-contact-information
/// </summary>
public class ResponsibleProducersClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ResponsibleProducersClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public ResponsibleProducersClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of responsible producers for EU GPSR compliance.
    /// These producers provide contact information for companies responsible for producing products.
    /// </summary>
    /// <param name="offset">Index of first returned responsible producer (0-50000, default: 0).</param>
    /// <param name="limit">Number of returned responsible producers (1-1000, default: 1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of responsible producers with count information.</returns>
    /// <exception cref="AllegroBadRequestException">Invalid request parameters.</exception>
    /// <exception cref="AllegroAuthenticationException">Authentication failed.</exception>
    /// <exception cref="AllegroAuthorizationException">Not authorized to access responsible producers.</exception>
    public System.Threading.Tasks.Task<ResponsibleProducersListResponse> GetResponsibleProducersAsync(
        int offset = 0,
        int limit = 1000,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };

        return _httpClient.GetAsync<ResponsibleProducersListResponse>(
            "/sale/responsible-producers",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new responsible producer for EU GPSR compliance.
    /// </summary>
    /// <param name="request">Responsible producer details including trade name, address, and contact information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created responsible producer details with generated ID.</returns>
    /// <exception cref="ArgumentNullException">Request is null.</exception>
    /// <exception cref="AllegroBadRequestException">Invalid request data.</exception>
    /// <exception cref="AllegroUnprocessableEntityException">Request data cannot be processed.</exception>
    public System.Threading.Tasks.Task<ResponsibleProducerResponse> CreateResponsibleProducerAsync(
        CreateResponsibleProducerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PostAsync<CreateResponsibleProducerRequest, ResponsibleProducerResponse>(
            "/sale/responsible-producers",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing responsible producer.
    /// </summary>
    /// <param name="id">Responsible producer identifier (UUID).</param>
    /// <param name="request">Updated responsible producer details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated responsible producer details.</returns>
    /// <exception cref="ArgumentNullException">Id or request is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible producer not found.</exception>
    /// <exception cref="AllegroBadRequestException">Invalid request data.</exception>
    public System.Threading.Tasks.Task<ResponsibleProducerResponse> UpdateResponsibleProducerAsync(
        string id,
        UpdateResponsibleProducerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PutAsync<UpdateResponsibleProducerRequest, ResponsibleProducerResponse>(
            $"/sale/responsible-producers/{id}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a responsible producer.
    /// </summary>
    /// <param name="id">Responsible producer identifier (UUID).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    /// <exception cref="ArgumentNullException">Id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible producer not found.</exception>
    public System.Threading.Tasks.Task DeleteResponsibleProducerAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.DeleteAsync(
            $"/sale/responsible-producers/{id}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific responsible producer by ID.
    /// </summary>
    /// <param name="id">Responsible producer identifier (UUID).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Responsible producer details.</returns>
    /// <exception cref="ArgumentNullException">Id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible producer not found.</exception>
    public System.Threading.Tasks.Task<ResponsibleProducerResponse> GetResponsibleProducerAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.GetAsync<ResponsibleProducerResponse>(
            $"/sale/responsible-producers/{id}",
            null,
            cancellationToken);
    }
}
