using AllegroApi.Http;
using AllegroApi.Models.ShipmentManagement;

namespace AllegroApi.Clients;

/// <summary>
/// Client for managing shipments through Allegro's Ship with Allegro service.
/// Provides functionality for creating shipments, managing labels, protocols, and pickups.
/// </summary>
public class ShipmentManagementClient
{
    private readonly AllegroHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ShipmentManagementClient class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    public ShipmentManagementClient(AllegroHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets available delivery services for the user.
    /// Returns services provided by Allegro and user's configured carrier contracts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available delivery services.</returns>
    public System.Threading.Tasks.Task<DeliveryServicesDto> GetDeliveryServicesAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<DeliveryServicesDto>(
            "/shipment-management/delivery-services",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new shipment for delivery.
    /// Returns a command that can be polled for status.
    /// </summary>
    /// <param name="command">Shipment creation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Shipment creation command with identifier.</returns>
    public System.Threading.Tasks.Task<ShipmentCreateCommandDto> CreateShipmentAsync(
        ShipmentCreateCommandDto command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PostAsync<ShipmentCreateCommandDto, ShipmentCreateCommandDto>(
            "/shipment-management/shipments/create-commands",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status of a shipment creation command.
    /// Poll this endpoint to check if shipment creation has completed.
    /// </summary>
    /// <param name="commandId">Command UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current status of the shipment creation command.</returns>
    public System.Threading.Tasks.Task<CreateShipmentCommandStatusDto> GetShipmentCreationStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<CreateShipmentCommandStatusDto>(
            $"/shipment-management/shipments/create-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Cancels an existing shipment.
    /// Returns a command that can be polled for status.
    /// </summary>
    /// <param name="command">Shipment cancellation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cancellation command with identifier.</returns>
    public System.Threading.Tasks.Task<ShipmentCancelCommandDto> CancelShipmentAsync(
        ShipmentCancelCommandDto command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PostAsync<ShipmentCancelCommandDto, ShipmentCancelCommandDto>(
            "/shipment-management/shipments/cancel-commands",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status of a shipment cancellation command.
    /// Poll this endpoint to check if cancellation has completed.
    /// </summary>
    /// <param name="commandId">Command UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current status of the cancellation command.</returns>
    public System.Threading.Tasks.Task<CancelShipmentCommandStatusDto> GetShipmentCancellationStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<CancelShipmentCommandStatusDto>(
            $"/shipment-management/shipments/cancel-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets detailed information about a specific shipment.
    /// </summary>
    /// <param name="shipmentId">Shipment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Full shipment details including tracking information.</returns>
    public System.Threading.Tasks.Task<ShipmentDetailsDto> GetShipmentDetailsAsync(
        string shipmentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(shipmentId);
        return _httpClient.GetAsync<ShipmentDetailsDto>(
            $"/shipment-management/shipments/{shipmentId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets shipping labels for one or more shipments.
    /// Returns labels in binary format (PDF or ZPL depending on the request).
    /// </summary>
    /// <param name="request">Label request with shipment IDs and format.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Label file in binary format.</returns>
    public System.Threading.Tasks.Task<byte[]> GetShipmentLabelsAsync(
        LabelRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<LabelRequestDto, byte[]>(
            "/shipment-management/label",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a shipment protocol document for multiple shipments.
    /// Protocol availability depends on the carrier.
    /// </summary>
    /// <param name="request">Request containing shipment identifiers.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Protocol document in binary format.</returns>
    public System.Threading.Tasks.Task<byte[]> GetShipmentProtocolAsync(
        ShipmentIdsDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<ShipmentIdsDto, byte[]>(
            "/shipment-management/protocol",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets proposed pickup dates for shipments.
    /// Used to determine when courier can collect parcels.
    /// </summary>
    /// <param name="request">Request with shipment IDs and pickup address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available pickup date proposals.</returns>
    public System.Threading.Tasks.Task<List<PickupProposalsResponseDto>> GetPickupProposalsAsync(
        PickupProposalsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _httpClient.PostAsync<PickupProposalsRequestDto, List<PickupProposalsResponseDto>>(
            "/shipment-management/pickup-proposals",
            request,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Requests a courier pickup for shipments.
    /// Returns a command that can be polled for status.
    /// </summary>
    /// <param name="command">Pickup request details including date, time, and address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Pickup creation command with identifier.</returns>
    public System.Threading.Tasks.Task<PickupCreateCommandDto> CreatePickupAsync(
        PickupCreateCommandDto command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _httpClient.PostAsync<PickupCreateCommandDto, PickupCreateCommandDto>(
            "/shipment-management/pickups/create-commands",
            command,
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the status of a pickup creation command.
    /// Poll this endpoint to check if pickup request has been processed.
    /// </summary>
    /// <param name="commandId">Command UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current status of the pickup creation command.</returns>
    public System.Threading.Tasks.Task<CreatePickupCommandStatusDto> GetPickupCreationStatusAsync(
        string commandId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(commandId);
        return _httpClient.GetAsync<CreatePickupCommandStatusDto>(
            $"/shipment-management/pickups/create-commands/{commandId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets detailed information about a specific pickup request.
    /// </summary>
    /// <param name="pickupId">Pickup identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Full pickup details including status and scheduled time.</returns>
    public System.Threading.Tasks.Task<PickupDto> GetPickupDetailsAsync(
        string pickupId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pickupId);
        return _httpClient.GetAsync<PickupDto>(
            $"/shipment-management/pickups/{pickupId}",
            null,
            cancellationToken);
    }
}
