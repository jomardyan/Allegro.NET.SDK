using AllegroApi.Http;
using AllegroApi.Models.Account;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing additional email addresses on user account.
/// </summary>
public class AdditionalEmailsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AdditionalEmailsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public AdditionalEmailsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of all additional email addresses assigned to the account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of additional email addresses.</returns>
    public System.Threading.Tasks.Task<AdditionalEmailsResponse> GetAdditionalEmailsAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<AdditionalEmailsResponse>(
            "/account/additional-emails",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets information about a particular additional email address.
    /// </summary>
    /// <param name="emailId">Email identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Additional email details.</returns>
    public System.Threading.Tasks.Task<AdditionalEmail> GetAdditionalEmailAsync(
        string emailId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailId);
        return _httpClient.GetAsync<AdditionalEmail>(
            $"/account/additional-emails/{emailId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Adds a new additional email address to the account.
    /// </summary>
    /// <param name="request">Email address request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created additional email.</returns>
    public System.Threading.Tasks.Task<AdditionalEmail> AddAdditionalEmailAsync(
        AdditionalEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<AdditionalEmailRequest, AdditionalEmail>(
            "/account/additional-emails",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes an additional email address from the account.
    /// </summary>
    /// <param name="emailId">Email identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the operation.</returns>
    public System.Threading.Tasks.Task DeleteAdditionalEmailAsync(
        string emailId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailId);
        return _httpClient.DeleteAsync(
            $"/account/additional-emails/{emailId}",
            null,
            cancellationToken);
    }
}
