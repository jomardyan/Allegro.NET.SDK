using AllegroApi.Http;
using AllegroApi.Models.BatchOperations;

namespace AllegroApi.Clients;

/// <summary>
/// Client for batch offer modification operations.
/// Provides functionality for modifying multiple offers at once (prices, quantities, general modifications, pricing rules).
/// Critical for high-volume sellers who need to manage large offer catalogs efficiently.
/// </summary>
public class BatchOperationsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the BatchOperationsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public BatchOperationsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Modifies multiple offers at once (names, descriptions, images, stock).
    /// Rate limit: 250,000 offer changes per hour or 9,000 offer changes per minute per user.
    /// </summary>
    /// <param name="commandId">Unique command identifier (use to track command status).</param>
    /// <param name="command">Command containing offers to modify with their new values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>General report with command status and statistics.</returns>
    public System.Threading.Tasks.Task<GeneralReport> ModifyOffersAsync(
        string commandId,
        OfferChangeCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PutAsync<OfferChangeCommand, GeneralReport>(
            $"/sale/offer-modification-commands/{commandId}",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status and summary of a batch offer modification command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Summary with number of successfully edited offers.</returns>
    public System.Threading.Tasks.Task<GeneralReport> GetModificationCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<GeneralReport>(
            $"/sale/offer-modification-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a detailed report of individual task statuses for a batch modification command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="limit">Maximum number of tasks to return (1-1000, default 100).</param>
    /// <param name="offset">Offset for pagination (default 0, max 999).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed report of task statuses.</returns>
    public System.Threading.Tasks.Task<TaskReport> GetModificationCommandTasksAsync(
        string commandId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<TaskReport>(
            $"/sale/offer-modification-commands/{commandId}/tasks",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Changes prices of multiple offers at once.
    /// Rate limit: 150,000 offer changes per hour or 9,000 offer changes per minute per user.
    /// </summary>
    /// <param name="commandId">Unique command identifier (use to track command status).</param>
    /// <param name="command">Command containing offers with new prices.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>General report with command status and statistics.</returns>
    public System.Threading.Tasks.Task<GeneralReport> ChangePricesAsync(
        string commandId,
        OfferPriceChangeCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PutAsync<OfferPriceChangeCommand, GeneralReport>(
            $"/sale/offer-price-change-commands/{commandId}",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status and summary of a batch price change command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Summary with command status and statistics.</returns>
    public System.Threading.Tasks.Task<GeneralReport> GetPriceChangeCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<GeneralReport>(
            $"/sale/offer-price-change-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a detailed report of individual task statuses for a batch price change command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="limit">Maximum number of tasks to return (1-1000, default 100).</param>
    /// <param name="offset">Offset for pagination (default 0, max 999).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed report of task statuses.</returns>
    public System.Threading.Tasks.Task<TaskReport> GetPriceChangeCommandTasksAsync(
        string commandId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<TaskReport>(
            $"/sale/offer-price-change-commands/{commandId}/tasks",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    /// <summary>
    /// Changes quantities of multiple offers at once.
    /// Rate limit: 250,000 offer changes per hour or 9,000 offer changes per minute per user.
    /// </summary>
    /// <param name="commandId">Unique command identifier (use to track command status).</param>
    /// <param name="command">Command containing offers with new quantities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>General report with command status and statistics.</returns>
    public System.Threading.Tasks.Task<GeneralReport> ChangeQuantitiesAsync(
        string commandId,
        OfferQuantityChangeCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PutAsync<OfferQuantityChangeCommand, GeneralReport>(
            $"/sale/offer-quantity-change-commands/{commandId}",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status and summary of a batch quantity change command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Summary with command status and statistics.</returns>
    public System.Threading.Tasks.Task<GeneralReport> GetQuantityChangeCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<GeneralReport>(
            $"/sale/offer-quantity-change-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a detailed report of individual task statuses for a batch quantity change command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="limit">Maximum number of tasks to return (1-1000, default 100).</param>
    /// <param name="offset">Offset for pagination (default 0, max 999).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed report of task statuses.</returns>
    public System.Threading.Tasks.Task<TaskReport> GetQuantityChangeCommandTasksAsync(
        string commandId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<TaskReport>(
            $"/sale/offer-quantity-change-commands/{commandId}/tasks",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    #region Price Automation Commands

    /// <summary>
    /// Modifies automatic pricing rules for multiple offers.
    /// Rate limit: 150,000 offer changes per hour or 9,000 per minute.
    /// </summary>
    /// <param name="command">Pricing automation command with offer criteria and rule modifications.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command report with execution status.</returns>
    public System.Threading.Tasks.Task<GeneralReport> CreatePriceAutomationCommandAsync(
        object command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PostAsync<object, GeneralReport>(
            "/sale/offer-price-automation-commands",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status and summary of an automatic pricing command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command execution report.</returns>
    public System.Threading.Tasks.Task<GeneralReport> GetPriceAutomationCommandStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<GeneralReport>(
            $"/sale/offer-price-automation-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a detailed report of individual task statuses for an automatic pricing command.
    /// Rate limit: 270,000 offer changes per minute.
    /// </summary>
    /// <param name="commandId">Command identifier.</param>
    /// <param name="limit">Maximum number of tasks to return (1-1000, default 100).</param>
    /// <param name="offset">Offset for pagination (default 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed report of task statuses.</returns>
    public System.Threading.Tasks.Task<TaskReport> GetPriceAutomationCommandTasksAsync(
        string commandId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        
        var queryParams = new Dictionary<string, string>();
        if (limit.HasValue)
            queryParams["limit"] = limit.Value.ToString();
        if (offset.HasValue)
            queryParams["offset"] = offset.Value.ToString();

        return _httpClient.GetAsync<TaskReport>(
            $"/sale/offer-price-automation-commands/{commandId}/tasks",
            queryParams.Count > 0 ? queryParams : null,
            cancellationToken);
    }

    #endregion
}
