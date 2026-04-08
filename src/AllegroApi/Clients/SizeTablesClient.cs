using AllegroApi.Http;
using AllegroApi.Models.Common;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing size tables.
/// </summary>
public class SizeTablesClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the SizeTablesClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public SizeTablesClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets all size tables assigned to the seller account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of size tables.</returns>
    public System.Threading.Tasks.Task<PublicTablesDto> GetSizeTablesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<PublicTablesDto>(
            "/sale/size-tables",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific size table by ID.
    /// </summary>
    /// <param name="tableId">Table identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Size table details.</returns>
    public System.Threading.Tasks.Task<PublicTableDto> GetSizeTableAsync(
        string tableId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tableId);
        return _httpClient.GetAsync<PublicTableDto>(
            $"/sale/size-tables/{tableId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new size table.
    /// </summary>
    /// <param name="request">Size table details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created size table.</returns>
    public System.Threading.Tasks.Task<PublicTableDto> CreateSizeTableAsync(
        SizeTablePutRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<SizeTablePutRequest, PublicTableDto>(
            "/sale/size-tables",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing size table.
    /// </summary>
    /// <param name="tableId">Table identifier.</param>
    /// <param name="request">Updated size table details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated size table.</returns>
    public System.Threading.Tasks.Task<PublicTableDto> UpdateSizeTableAsync(
        string tableId,
        SizeTablePutRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tableId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PutAsync<SizeTablePutRequest, PublicTableDto>(
            $"/sale/size-tables/{tableId}",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets all available size table templates.
    /// Templates define the structure (headers and values) that can be used when creating size tables.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of size table templates.</returns>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthenticationException">Unauthorized.</exception>
    /// <exception cref="AllegroApi.Exceptions.AllegroAuthorizationException">Forbidden.</exception>
    public System.Threading.Tasks.Task<SizeTableTemplatesResponse> GetSizeTableTemplatesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<SizeTableTemplatesResponse>(
            "/sale/size-tables-templates",
            null,
            cancellationToken);
    }
}
