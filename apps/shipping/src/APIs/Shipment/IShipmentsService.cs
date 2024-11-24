using Shipping.APIs.Common;
using Shipping.APIs.Dtos;

namespace Shipping.APIs;

public interface IShipmentsService
{
    /// <summary>
    /// Create one Shipment
    /// </summary>
    public Task<Shipment> CreateShipment(ShipmentCreateInput shipment);

    /// <summary>
    /// Delete one Shipment
    /// </summary>
    public Task DeleteShipment(ShipmentWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Shipments
    /// </summary>
    public Task<List<Shipment>> Shipments(ShipmentFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Shipment records
    /// </summary>
    public Task<MetadataDto> ShipmentsMeta(ShipmentFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Shipment
    /// </summary>
    public Task<Shipment> Shipment(ShipmentWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Shipment
    /// </summary>
    public Task UpdateShipment(ShipmentWhereUniqueInput uniqueId, ShipmentUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Items records to Shipment
    /// </summary>
    public Task ConnectItems(ShipmentWhereUniqueInput uniqueId, ItemWhereUniqueInput[] itemsId);

    /// <summary>
    /// Disconnect multiple Items records from Shipment
    /// </summary>
    public Task DisconnectItems(ShipmentWhereUniqueInput uniqueId, ItemWhereUniqueInput[] itemsId);

    /// <summary>
    /// Find multiple Items records for Shipment
    /// </summary>
    public Task<List<Item>> FindItems(
        ShipmentWhereUniqueInput uniqueId,
        ItemFindManyArgs ItemFindManyArgs
    );

    /// <summary>
    /// Update multiple Items records for Shipment
    /// </summary>
    public Task UpdateItems(ShipmentWhereUniqueInput uniqueId, ItemWhereUniqueInput[] itemsId);
}
