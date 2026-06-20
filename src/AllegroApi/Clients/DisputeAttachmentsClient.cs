using AllegroApi.Http;
using AllegroApi.Models.Disputes;
using System.Net.Http;
using System.Text;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing dispute attachments (upload and download).
/// </summary>
public class DisputeAttachmentsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the DisputeAttachmentsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public DisputeAttachmentsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Creates a dispute attachment declaration.
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="declaration">Attachment declaration (fileName, size).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Attachment identifier and upload URL (from Location header).</returns>
    /// <exception cref="AllegroBadRequestException">Validation error.</exception>
    /// <exception cref="AllegroAuthorizationException">Forbidden.</exception>
    /// <exception cref="AllegroAuthenticationException">Unauthorized.</exception>
    public async System.Threading.Tasks.Task<(DisputeAttachmentId Id, string UploadUrl)> CreateAttachmentDeclarationAsync(
        AttachmentDeclaration declaration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(declaration);
        var response = await _httpClient.PostRawAsync(
            "/sale/dispute-attachments",
            declaration,
            null,
            cancellationToken);
        var id = await _httpClient.ReadJsonAsync<DisputeAttachmentId>(response).ConfigureAwait(false);
        var uploadUrl = response.Headers.Location?.ToString() ?? string.Empty;
        return (id, uploadUrl);
    }

    /// <summary>
    /// Uploads a dispute attachment file (binary).
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="uploadUrl">Upload URL from Location header.</param>
    /// <param name="fileBytes">File content as byte array.</param>
    /// <param name="contentType">MIME type (e.g. image/jpeg, application/pdf).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if upload succeeded.</returns>
    /// <exception cref="AllegroBadRequestException">Validation error.</exception>
    /// <exception cref="AllegroAuthorizationException">Forbidden.</exception>
    /// <exception cref="AllegroAuthenticationException">Unauthorized.</exception>
    public async System.Threading.Tasks.Task<bool> UploadAttachmentAsync(
        string uploadUrl,
        byte[] fileBytes,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(uploadUrl);
        ArgumentNullException.ThrowIfNull(fileBytes);
        ArgumentNullException.ThrowIfNull(contentType);
        var result = await _httpClient.PutRawAsync(uploadUrl, fileBytes, contentType, cancellationToken).ConfigureAwait(false);
        return result.IsSuccessStatusCode;
    }

    /// <summary>
    /// Downloads a dispute attachment file (binary).
    /// Rate limit: 100 requests per 60 seconds.
    /// </summary>
    /// <param name="attachmentId">Attachment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>File content as byte array.</returns>
    /// <exception cref="AllegroNotFoundException">Attachment not found.</exception>
    /// <exception cref="AllegroAuthorizationException">Forbidden.</exception>
    /// <exception cref="AllegroAuthenticationException">Unauthorized.</exception>
    public async System.Threading.Tasks.Task<byte[]> DownloadAttachmentAsync(
        string attachmentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(attachmentId);
        // GetRawAsync already maps non-success responses to the appropriate Allegro exception types.
        var response = await _httpClient.GetRawAsync($"/sale/dispute-attachments/{attachmentId}", null, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }
}
