using AllegroApi.Http;
using AllegroApi.Models.AfterSales;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing after-sales service conditions (return policies, warranties, implied warranties).
/// </summary>
public class AfterSalesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AfterSalesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public AfterSalesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Return Policies

    /// <summary>
    /// Gets the list of user's return policies.
    /// </summary>
    /// <param name="limit">The limit of elements in the response (1-60, default: 60).</param>
    /// <param name="offset">The offset of elements in the response (0-59, default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of return policies.</returns>
    public System.Threading.Tasks.Task<ReturnPoliciesList> GetReturnPoliciesAsync(
        int limit = 60,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<ReturnPoliciesList>(
            $"/after-sales-service-conditions/return-policies?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific return policy by ID.
    /// </summary>
    /// <param name="returnPolicyId">The return policy identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Return policy details.</returns>
    public System.Threading.Tasks.Task<ReturnPolicyResponse> GetReturnPolicyAsync(
        string returnPolicyId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(returnPolicyId);
        return _httpClient.GetAsync<ReturnPolicyResponse>(
            $"/after-sales-service-conditions/return-policies/{returnPolicyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new return policy.
    /// </summary>
    /// <param name="request">Return policy details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created return policy.</returns>
    public System.Threading.Tasks.Task<ReturnPolicyResponse> CreateReturnPolicyAsync(
        ReturnPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<ReturnPolicyRequest, ReturnPolicyResponse>(
            "/after-sales-service-conditions/return-policies",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing return policy.
    /// </summary>
    /// <param name="returnPolicyId">The return policy identifier.</param>
    /// <param name="request">Updated return policy details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated return policy.</returns>
    public System.Threading.Tasks.Task<ReturnPolicyResponse> UpdateReturnPolicyAsync(
        string returnPolicyId,
        ReturnPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(returnPolicyId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<ReturnPolicyRequest, ReturnPolicyResponse>(
            $"/after-sales-service-conditions/return-policies/{returnPolicyId}",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Warranties

    /// <summary>
    /// Gets the list of user's warranties.
    /// </summary>
    /// <param name="limit">The limit of elements in the response (1-60, default: 60).</param>
    /// <param name="offset">The offset of elements in the response (0-59, default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of warranties.</returns>
    public System.Threading.Tasks.Task<WarrantiesList> GetWarrantiesAsync(
        int limit = 60,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<WarrantiesList>(
            $"/after-sales-service-conditions/warranties?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific warranty by ID.
    /// </summary>
    /// <param name="warrantyId">The warranty identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Warranty details.</returns>
    public System.Threading.Tasks.Task<WarrantyResponse> GetWarrantyAsync(
        string warrantyId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(warrantyId);
        return _httpClient.GetAsync<WarrantyResponse>(
            $"/after-sales-service-conditions/warranties/{warrantyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new warranty.
    /// </summary>
    /// <param name="request">Warranty details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created warranty.</returns>
    public System.Threading.Tasks.Task<WarrantyResponse> CreateWarrantyAsync(
        WarrantyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<WarrantyRequest, WarrantyResponse>(
            "/after-sales-service-conditions/warranties",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing warranty.
    /// </summary>
    /// <param name="warrantyId">The warranty identifier.</param>
    /// <param name="request">Updated warranty details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated warranty.</returns>
    public System.Threading.Tasks.Task<WarrantyResponse> UpdateWarrantyAsync(
        string warrantyId,
        WarrantyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(warrantyId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<WarrantyRequest, WarrantyResponse>(
            $"/after-sales-service-conditions/warranties/{warrantyId}",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Implied Warranties

    /// <summary>
    /// Gets the list of user's implied warranties.
    /// </summary>
    /// <param name="limit">The limit of elements in the response (1-60, default: 60).</param>
    /// <param name="offset">The offset of elements in the response (0-59, default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of implied warranties.</returns>
    public System.Threading.Tasks.Task<ImpliedWarrantiesList> GetImpliedWarrantiesAsync(
        int limit = 60,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<ImpliedWarrantiesList>(
            $"/after-sales-service-conditions/implied-warranties?limit={limit}&offset={offset}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific implied warranty by ID.
    /// </summary>
    /// <param name="impliedWarrantyId">The implied warranty identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Implied warranty details.</returns>
    public System.Threading.Tasks.Task<ImpliedWarrantyResponse> GetImpliedWarrantyAsync(
        string impliedWarrantyId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(impliedWarrantyId);
        return _httpClient.GetAsync<ImpliedWarrantyResponse>(
            $"/after-sales-service-conditions/implied-warranties/{impliedWarrantyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new implied warranty.
    /// </summary>
    /// <param name="request">Implied warranty details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created implied warranty.</returns>
    public System.Threading.Tasks.Task<ImpliedWarrantyResponse> CreateImpliedWarrantyAsync(
        ImpliedWarrantyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<ImpliedWarrantyRequest, ImpliedWarrantyResponse>(
            "/after-sales-service-conditions/implied-warranties",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing implied warranty.
    /// </summary>
    /// <param name="impliedWarrantyId">The implied warranty identifier.</param>
    /// <param name="request">Updated implied warranty details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated implied warranty.</returns>
    public System.Threading.Tasks.Task<ImpliedWarrantyResponse> UpdateImpliedWarrantyAsync(
        string impliedWarrantyId,
        ImpliedWarrantyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(impliedWarrantyId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<ImpliedWarrantyRequest, ImpliedWarrantyResponse>(
            $"/after-sales-service-conditions/implied-warranties/{impliedWarrantyId}",
            request,
            null,
            cancellationToken);
    }

    #endregion

    #region Attachments

    /// <summary>
    /// Creates an attachment object to receive an upload URL for warranty documentation.
    /// Use the returned upload URL to submit the PDF file.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Attachment response with upload URL in Location header.</returns>
    public System.Threading.Tasks.Task<AfterSalesAttachmentResponse> CreateAttachmentAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.PostAsync<object, AfterSalesAttachmentResponse>(
            "/after-sales-service-conditions/attachments",
            new { },
            null,
            cancellationToken);
    }

    #endregion
}
