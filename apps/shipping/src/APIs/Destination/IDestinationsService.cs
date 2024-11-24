using Shipping.APIs.Common;
using Shipping.APIs.Dtos;

namespace Shipping.APIs;

public interface IDestinationsService
{
    /// <summary>
    /// Create one Destination
    /// </summary>
    public Task<Destination> CreateDestination(DestinationCreateInput destination);

    /// <summary>
    /// Delete one Destination
    /// </summary>
    public Task DeleteDestination(DestinationWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Destinations
    /// </summary>
    public Task<List<Destination>> Destinations(DestinationFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Destination records
    /// </summary>
    public Task<MetadataDto> DestinationsMeta(DestinationFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Destination
    /// </summary>
    public Task<Destination> Destination(DestinationWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Destination
    /// </summary>
    public Task UpdateDestination(
        DestinationWhereUniqueInput uniqueId,
        DestinationUpdateInput updateDto
    );

    /// <summary>
    /// Connect multiple Packages records to Destination
    /// </summary>
    public Task ConnectPackages(
        DestinationWhereUniqueInput uniqueId,
        PackageModelWhereUniqueInput[] packageModelsId
    );

    /// <summary>
    /// Disconnect multiple Packages records from Destination
    /// </summary>
    public Task DisconnectPackages(
        DestinationWhereUniqueInput uniqueId,
        PackageModelWhereUniqueInput[] packageModelsId
    );

    /// <summary>
    /// Find multiple Packages records for Destination
    /// </summary>
    public Task<List<PackageModel>> FindPackages(
        DestinationWhereUniqueInput uniqueId,
        PackageModelFindManyArgs PackageModelFindManyArgs
    );

    /// <summary>
    /// Update multiple Packages records for Destination
    /// </summary>
    public Task UpdatePackages(
        DestinationWhereUniqueInput uniqueId,
        PackageModelWhereUniqueInput[] packageModelsId
    );
}
