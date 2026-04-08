using AllegroApi.Http;
using AllegroApi.Models.AdvancedOffers;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing advanced offer features (variants, attachments, smart offers).
/// </summary>
public class AdvancedOffersClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AdvancedOffersClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public AdvancedOffersClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Offer Variants

    /// <summary>
    /// Gets offer variants for a specific offer.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Offer variant set.</returns>
    public System.Threading.Tasks.Task<OfferVariantSet> GetOfferVariantsAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<OfferVariantSet>(
            $"/sale/offer-variants/{offerId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new offer variant set.
    /// </summary>
    /// <param name="variantSet">Variant set details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created variant set.</returns>
    public System.Threading.Tasks.Task<OfferVariantSet> CreateOfferVariantSetAsync(
        OfferVariantSet variantSet,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(variantSet);
        return _httpClient.PostAsync<OfferVariantSet, OfferVariantSet>(
            "/sale/offer-variants",
            variantSet,
            null,
            cancellationToken);
    }

    #endregion

    #region Offer Attachments

    /// <summary>
    /// Gets attachments for a specific offer.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of offer attachments.</returns>
    public System.Threading.Tasks.Task<OfferAttachmentsList> GetOfferAttachmentsAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<OfferAttachmentsList>(
            $"/sale/offers/{offerId}/attachments",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new offer attachment.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="request">Attachment details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created attachment.</returns>
    public System.Threading.Tasks.Task<OfferAttachment> CreateOfferAttachmentAsync(
        string offerId,
        CreateOfferAttachmentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CreateOfferAttachmentRequest, OfferAttachment>(
            $"/sale/offers/{offerId}/attachments",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a standalone offer attachment to receive an upload URL.
    /// This is step 1 of the attachment upload flow. After creation, use UploadOfferAttachmentAsync to submit the file.
    /// Supports PDF, JPEG, and PNG files depending on attachment type.
    /// </summary>
    /// <param name="request">Attachment creation request with type and filename.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created attachment with ID (use for upload in step 2).</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    public System.Threading.Tasks.Task<OfferAttachmentResponse> CreateStandaloneAttachmentAsync(
        OfferAttachmentCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<OfferAttachmentCreateRequest, OfferAttachmentResponse>(
            "/sale/offer-attachments",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Uploads file content for a standalone attachment (step 2 after CreateStandaloneAttachmentAsync).
    /// Use the attachment ID from step 1. The upload URL is provided in the Location header of the POST response.
    /// This method uploads binary file content to the attachment.
    /// </summary>
    /// <param name="attachmentId">The attachment ID from CreateStandaloneAttachmentAsync.</param>
    /// <param name="fileContent">Binary file content.</param>
    /// <param name="contentType">Content type (application/pdf, image/jpeg, or image/png).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated attachment with file URL.</returns>
    /// <exception cref="ArgumentNullException">Thrown when attachmentId or fileContent is null.</exception>
    public async System.Threading.Tasks.Task<OfferAttachmentResponse> UploadOfferAttachmentAsync(
        string attachmentId,
        byte[] fileContent,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attachmentId);
        ArgumentNullException.ThrowIfNull(fileContent);
        ArgumentNullException.ThrowIfNull(contentType);

        // Note: This requires special handling as it uses upload.allegro.pl domain
        // The actual upload URL should come from the Location header of POST /sale/offer-attachments
        var content = new ByteArrayContent(fileContent);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        var response = await _httpClient.PutAsync<ByteArrayContent, OfferAttachmentResponse>(
            $"/sale/offer-attachments/{attachmentId}",
            content,
            null,
            cancellationToken);

        return response;
    }

    /// <summary>
    /// Gets details of an offer attachment by ID.
    /// Retrieves attachment information including download link.
    /// </summary>
    /// <param name="attachmentId">The attachment ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Attachment details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when attachmentId is null.</exception>
    public System.Threading.Tasks.Task<OfferAttachmentResponse> GetOfferAttachmentDetailsAsync(
        string attachmentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attachmentId);
        return _httpClient.GetAsync<OfferAttachmentResponse>(
            $"/sale/offer-attachments/{attachmentId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Smart Offers

    /// <summary>
    /// Gets smart offer configuration for a specific offer.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Smart offer configuration.</returns>
    public System.Threading.Tasks.Task<SmartOfferConfig> GetSmartConfigAsync(
        string offerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        return _httpClient.GetAsync<SmartOfferConfig>(
            $"/sale/offers/{offerId}/smart",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Enables or updates smart features for an offer.
    /// </summary>
    /// <param name="offerId">The offer identifier.</param>
    /// <param name="config">Smart offer configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated smart offer configuration.</returns>
    public System.Threading.Tasks.Task<SmartOfferConfig> UpdateSmartConfigAsync(
        string offerId,
        SmartOfferConfig config,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(offerId);
        ArgumentNullException.ThrowIfNull(config);
        return _httpClient.PutAsync<SmartOfferConfig, SmartOfferConfig>(
            $"/sale/offers/{offerId}/smart",
            config,
            null,
            cancellationToken);
    }

    #endregion
}
