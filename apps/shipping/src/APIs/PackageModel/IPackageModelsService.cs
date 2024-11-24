using Shipping.APIs.Common;
using Shipping.APIs.Dtos;

namespace Shipping.APIs;

public interface IPackageModelsService
{
    /// <summary>
    /// Create one Package
    /// </summary>
    public Task<PackageModel> CreatePackageModel(PackageModelCreateInput packagemodel);

    /// <summary>
    /// Delete one Package
    /// </summary>
    public Task DeletePackageModel(PackageModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Packages
    /// </summary>
    public Task<List<PackageModel>> PackageModels(PackageModelFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Package records
    /// </summary>
    public Task<MetadataDto> PackageModelsMeta(PackageModelFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Package
    /// </summary>
    public Task<PackageModel> PackageModel(PackageModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Package
    /// </summary>
    public Task UpdatePackageModel(
        PackageModelWhereUniqueInput uniqueId,
        PackageModelUpdateInput updateDto
    );

    /// <summary>
    /// Get a Destination record for Package
    /// </summary>
    public Task<Destination> GetDestination(PackageModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Shipments records to Package
    /// </summary>
    public Task ConnectShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] shipmentsId
    );

    /// <summary>
    /// Disconnect multiple Shipments records from Package
    /// </summary>
    public Task DisconnectShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] shipmentsId
    );

    /// <summary>
    /// Find multiple Shipments records for Package
    /// </summary>
    public Task<List<Shipment>> FindShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentFindManyArgs ShipmentFindManyArgs
    );

    /// <summary>
    /// Update multiple Shipments records for Package
    /// </summary>
    public Task UpdateShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] shipmentsId
    );
}
