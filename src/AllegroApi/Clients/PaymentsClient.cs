using AllegroApi.Http;
using AllegroApi.Models.Payments;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing payment operations.
/// </summary>
public class PaymentsClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the PaymentsClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public PaymentsClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets a list of payment operations for an order.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of payment operations.</returns>
    public System.Threading.Tasks.Task<PaymentOperationsList> GetPaymentOperationsAsync(
        string orderId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orderId);
        return _httpClient.GetAsync<PaymentOperationsList>(
            $"/order/checkout-forms/{orderId}/payments",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets detailed payment information.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment details.</returns>
    public System.Threading.Tasks.Task<PaymentDetailsResponse> GetPaymentAsync(
        string paymentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(paymentId);
        return _httpClient.GetAsync<PaymentDetailsResponse>(
            $"/payments/{paymentId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a payment refund.
    /// </summary>
    /// <param name="request">Refund request with payment ID, amount, and reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created refund details.</returns>
    public System.Threading.Tasks.Task<PaymentRefundResponse> CreatePaymentRefundAsync(
        CreatePaymentRefundRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CreatePaymentRefundRequest, PaymentRefundResponse>(
            "/payments/refunds",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets payment operations history for the seller.
    /// </summary>
    /// <param name="walletType">Wallet type (AVAILABLE, WAITING).</param>
    /// <param name="walletPaymentOperator">Payment operator (PAYU, P24, AF, AF_P24, AF_PAYU).</param>
    /// <param name="paymentId">Filter by payment ID.</param>
    /// <param name="participantLogin">Filter by participant login.</param>
    /// <param name="occurredAtGte">Minimum date of operation occurrence.</param>
    /// <param name="occurredAtLte">Maximum date of operation occurrence.</param>
    /// <param name="groups">Operation groups (INCOME, OUTCOME, REFUND, BLOCKADES).</param>
    /// <param name="marketplaceId">Marketplace identifier (allegro-pl, allegro-cz).</param>
    /// <param name="currency">Currency of operations (e.g., PLN).</param>
    /// <param name="limit">Number of operations to return (1-50, default: 50).</param>
    /// <param name="offset">Index of first operation (default: 0).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment operations history.</returns>
    public System.Threading.Tasks.Task<PaymentOperationsHistory> GetPaymentOperationsHistoryAsync(
        string? walletType = null,
        string? walletPaymentOperator = null,
        string? paymentId = null,
        string? participantLogin = null,
        DateTime? occurredAtGte = null,
        DateTime? occurredAtLte = null,
        List<string>? groups = null,
        string? marketplaceId = null,
        string? currency = null,
        int limit = 50,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString(),
            ["offset"] = offset.ToString()
        };

        if (!string.IsNullOrEmpty(walletType))
            queryParams["wallet.type"] = walletType;
        if (!string.IsNullOrEmpty(walletPaymentOperator))
            queryParams["wallet.paymentOperator"] = walletPaymentOperator;
        if (!string.IsNullOrEmpty(paymentId))
            queryParams["payment.id"] = paymentId;
        if (!string.IsNullOrEmpty(participantLogin))
            queryParams["participant.login"] = participantLogin;
        if (occurredAtGte.HasValue)
            queryParams["occurredAt.gte"] = occurredAtGte.Value.ToString("O");
        if (occurredAtLte.HasValue)
            queryParams["occurredAt.lte"] = occurredAtLte.Value.ToString("O");
        if (groups != null && groups.Count > 0)
        {
            for (int i = 0; i < groups.Count; i++)
                queryParams[$"group[{i}]"] = groups[i];
        }
        if (!string.IsNullOrEmpty(marketplaceId))
            queryParams["marketplaceId"] = marketplaceId;
        if (!string.IsNullOrEmpty(currency))
            queryParams["currency"] = currency;

        return _httpClient.GetAsync<PaymentOperationsHistory>(
            "/payments/payment-operations",
            queryParams,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific payment operation by ID.
    /// </summary>
    /// <param name="operationId">Operation identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment operation details.</returns>
    public System.Threading.Tasks.Task<PaymentOperationHistory> GetPaymentOperationAsync(
        string operationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operationId);
        return _httpClient.GetAsync<PaymentOperationHistory>(
            $"/payments/payment-operations/{operationId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a refund for a payment operation.
    /// </summary>
    /// <param name="operationId">Operation identifier.</param>
    /// <param name="request">Refund request details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created refund.</returns>
    public System.Threading.Tasks.Task<PaymentRefundResponse> RefundPaymentOperationAsync(
        string operationId,
        CreatePaymentRefundRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operationId);
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<CreatePaymentRefundRequest, PaymentRefundResponse>(
            $"/payments/payment-operations/{operationId}/refund",
            request,
            null,
            cancellationToken);
    }
}
