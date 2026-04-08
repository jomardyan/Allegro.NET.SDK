using AllegroApi.Http;
using AllegroApi.Models.ResponsiblePersons;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing responsible persons for EU GPSR compliance.
/// Responsible persons ensure that products comply with EU regulations.
/// Read more: https://developer.allegro.pl/tutorials/account-and-user-data-management-jn9vBjqjnsw#responsible-persons-for-the-compliance-of-the-product-with-eu-regulations
/// </summary>
public class ResponsiblePersonsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ResponsiblePersonsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public ResponsiblePersonsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of responsible persons for EU GPSR compliance.
    /// These persons ensure that products comply with EU regulations.
    /// </summary>
    /// <param name="offset">Index of first returned responsible person (0-16000, default: 0).</param>
    /// <param name="limit">Number of returned responsible persons (1-1000, default: 1000).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of responsible persons with count information.</returns>
    /// <exception cref="AllegroBadRequestException">Invalid request parameters.</exception>
    /// <exception cref="AllegroAuthenticationException">Authentication failed.</exception>
    /// <exception cref="AllegroAuthorizationException">Not authorized to access responsible persons.</exception>
    public System.Threading.Tasks.Task<ResponsiblePersonsListResponse> GetResponsiblePersonsAsync(
        int offset = 0,
        int limit = 1000,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["offset"] = offset.ToString(),
            ["limit"] = limit.ToString()
        };

        return _httpClient.GetAsync<ResponsiblePersonsListResponse>(
            "/sale/responsible-persons",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new responsible person for EU GPSR compliance.
    /// The responsible person ensures that the product complies with EU regulations.
    /// </summary>
    /// <param name="request">Responsible person details including name, address, and contact information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created responsible person details with generated ID.</returns>
    /// <exception cref="ArgumentNullException">Request is null.</exception>
    /// <exception cref="AllegroBadRequestException">Invalid request data.</exception>
    /// <exception cref="AllegroUnprocessableEntityException">Request data cannot be processed.</exception>
    public System.Threading.Tasks.Task<ResponsiblePersonResponse> CreateResponsiblePersonAsync(
        CreateResponsiblePersonRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PostAsync<CreateResponsiblePersonRequest, ResponsiblePersonResponse>(
            "/sale/responsible-persons",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing responsible person.
    /// </summary>
    /// <param name="id">Responsible person identifier (UUID).</param>
    /// <param name="request">Updated responsible person details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated responsible person details.</returns>
    /// <exception cref="ArgumentNullException">Id or request is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible person not found.</exception>
    /// <exception cref="AllegroBadRequestException">Invalid request data.</exception>
    public System.Threading.Tasks.Task<ResponsiblePersonResponse> UpdateResponsiblePersonAsync(
        string id,
        UpdateResponsiblePersonRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(request);

        return _httpClient.PutAsync<UpdateResponsiblePersonRequest, ResponsiblePersonResponse>(
            $"/sale/responsible-persons/{id}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a responsible person.
    /// </summary>
    /// <param name="id">Responsible person identifier (UUID).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    /// <exception cref="ArgumentNullException">Id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible person not found.</exception>
    public System.Threading.Tasks.Task DeleteResponsiblePersonAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.DeleteAsync(
            $"/sale/responsible-persons/{id}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific responsible person by ID.
    /// </summary>
    /// <param name="id">Responsible person identifier (UUID).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Responsible person details.</returns>
    /// <exception cref="ArgumentNullException">Id is null.</exception>
    /// <exception cref="AllegroNotFoundException">Responsible person not found.</exception>
    public System.Threading.Tasks.Task<ResponsiblePersonResponse> GetResponsiblePersonAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _httpClient.GetAsync<ResponsiblePersonResponse>(
            $"/sale/responsible-persons/{id}",
            null,
            cancellationToken);
    }
}
