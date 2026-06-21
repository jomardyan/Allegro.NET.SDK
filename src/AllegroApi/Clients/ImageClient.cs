using AllegroApi.Http;
using AllegroApi.Models.Images;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Task = System.Threading.Tasks.Task;

namespace AllegroApi.Clients;

/// <summary>
/// Client for image upload and management operations.
/// Images are hosted on a separate domain (upload.allegro.pl).
/// </summary>
public class ImageClient
{
    private readonly AllegroHttpClient _httpClient;
    private readonly string _uploadBaseUrl;

    /// <summary>
    /// Initializes a new instance of the ImageClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    /// <param name="uploadBaseUrl">The base URL for image uploads.</param>
    public ImageClient(AllegroHttpClient httpClient, string uploadBaseUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _uploadBaseUrl = uploadBaseUrl ?? throw new ArgumentNullException(nameof(uploadBaseUrl));
    }

    /// <summary>
    /// Upload an image from a URL. Allegro will download the image from the provided URL.
    /// </summary>
    /// <param name="imageUrl">URL of the image to upload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload response with image location and expiration date</returns>
    public async Task<ImageUploadResponse?> UploadImageFromUrlAsync(
        string imageUrl,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be null or empty.", nameof(imageUrl));

        var request = new ImageUploadByUrlRequest { Url = imageUrl };
        
        // Use AllegroHttpClient's PostAsync which handles authentication and error handling
        return await _httpClient.PostAsync<ImageUploadByUrlRequest, ImageUploadResponse>(
            "/sale/images",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Upload an image from binary data (byte array).
    /// </summary>
    /// <param name="imageData">The image file as byte array</param>
    /// <param name="contentType">MIME type of the image (e.g., "image/jpeg", "image/png")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload response with image location and expiration date</returns>
    public async Task<ImageUploadResponse?> UploadImageAsync(
        byte[] imageData,
        string contentType = "image/jpeg",
        CancellationToken cancellationToken = default)
    {
        if (imageData == null || imageData.Length == 0)
            throw new ArgumentException("Image data cannot be null or empty.", nameof(imageData));
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be null or empty.", nameof(contentType));

        // Binary image upload uses a dedicated host (upload.allegro.pl) and expects the raw
        // image bytes with the appropriate image/* content type, not a JSON payload.
        var url = $"{_uploadBaseUrl.TrimEnd('/')}/sale/images";
        var response = await _httpClient.PostRawBytesAsync(url, imageData, contentType, cancellationToken).ConfigureAwait(false);
        return await _httpClient.ReadJsonAsync<ImageUploadResponse>(response).ConfigureAwait(false);
    }

    /// <summary>
    /// Upload an image from a file stream.
    /// </summary>
    /// <param name="stream">The image file stream</param>
    /// <param name="contentType">MIME type of the image (e.g., "image/jpeg", "image/png")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Upload response with image location and expiration date</returns>
    public async Task<ImageUploadResponse?> UploadImageFromStreamAsync(
        Stream stream,
        string contentType = "image/jpeg",
        CancellationToken cancellationToken = default)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
        return await UploadImageAsync(memoryStream.ToArray(), contentType, cancellationToken).ConfigureAwait(false);
    }
}
