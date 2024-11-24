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
    /// Connect multiple Items records to Destination
    /// </summary>
    public Task ConnectItems(DestinationWhereUniqueInput uniqueId, ItemWhereUniqueInput[] itemsId);

    /// <summary>
    /// Disconnect multiple Items records from Destination
    /// </summary>
    public Task DisconnectItems(
        DestinationWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] itemsId
    );

    /// <summary>
    /// Find multiple Items records for Destination
    /// </summary>
    public Task<List<Item>> FindItems(
        DestinationWhereUniqueInput uniqueId,
        ItemFindManyArgs ItemFindManyArgs
    );

    /// <summary>
    /// Update multiple Items records for Destination
    /// </summary>
    public Task UpdateItems(DestinationWhereUniqueInput uniqueId, ItemWhereUniqueInput[] itemsId);
}
