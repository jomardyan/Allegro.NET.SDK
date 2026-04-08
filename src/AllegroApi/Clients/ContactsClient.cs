using AllegroApi.Http;
using AllegroApi.Models.Common;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing offer contacts.
/// </summary>
public class ContactsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ContactsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public ContactsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of user's contacts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of contacts.</returns>
    public System.Threading.Tasks.Task<ContactResponseList> GetContactsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<ContactResponseList>(
            "/sale/offer-contacts",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets contact details by ID.
    /// </summary>
    /// <param name="contactId">Contact identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Contact details.</returns>
    public System.Threading.Tasks.Task<ContactResponse> GetContactAsync(
        string contactId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contactId);
        return _httpClient.GetAsync<ContactResponse>(
            $"/sale/offer-contacts/{contactId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="request">Contact details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created contact.</returns>
    public System.Threading.Tasks.Task<ContactResponse> CreateContactAsync(
        ContactRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<ContactRequest, ContactResponse>(
            "/sale/offer-contacts",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="contactId">Contact identifier.</param>
    /// <param name="request">Updated contact details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated contact.</returns>
    public System.Threading.Tasks.Task<ContactResponse> UpdateContactAsync(
        string contactId,
        ContactRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contactId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<ContactRequest, ContactResponse>(
            $"/sale/offer-contacts/{contactId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a contact.
    /// </summary>
    /// <param name="contactId">Contact identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    public System.Threading.Tasks.Task DeleteContactAsync(
        string contactId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contactId);
        return _httpClient.DeleteAsync(
            $"/sale/offer-contacts/{contactId}",
            null,
            cancellationToken);
    }
}
