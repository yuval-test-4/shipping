using Microsoft.EntityFrameworkCore;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;
using Shipping.APIs.Extensions;
using Shipping.Infrastructure;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs;

public abstract class DestinationsServiceBase : IDestinationsService
{
    protected readonly ShippingDbContext _context;

    public DestinationsServiceBase(ShippingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Destination
    /// </summary>
    public async Task<Destination> CreateDestination(DestinationCreateInput createDto)
    {
        var destination = new DestinationDbModel
        {
            Address = createDto.Address,
            City = createDto.City,
            Country = createDto.Country,
            CreatedAt = createDto.CreatedAt,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            destination.Id = createDto.Id;
        }
        if (createDto.Items != null)
        {
            destination.Items = await _context
                .Items.Where(item => createDto.Items.Select(t => t.Id).Contains(item.Id))
                .ToListAsync();
        }

        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<DestinationDbModel>(destination.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Destination
    /// </summary>
    public async Task DeleteDestination(DestinationWhereUniqueInput uniqueId)
    {
        var destination = await _context.Destinations.FindAsync(uniqueId.Id);
        if (destination == null)
        {
            throw new NotFoundException();
        }

        _context.Destinations.Remove(destination);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Destinations
    /// </summary>
    public async Task<List<Destination>> Destinations(DestinationFindManyArgs findManyArgs)
    {
        var destinations = await _context
            .Destinations.Include(x => x.Items)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return destinations.ConvertAll(destination => destination.ToDto());
    }

    /// <summary>
    /// Meta data about Destination records
    /// </summary>
    public async Task<MetadataDto> DestinationsMeta(DestinationFindManyArgs findManyArgs)
    {
        var count = await _context.Destinations.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Destination
    /// </summary>
    public async Task<Destination> Destination(DestinationWhereUniqueInput uniqueId)
    {
        var destinations = await this.Destinations(
            new DestinationFindManyArgs { Where = new DestinationWhereInput { Id = uniqueId.Id } }
        );
        var destination = destinations.FirstOrDefault();
        if (destination == null)
        {
            throw new NotFoundException();
        }

        return destination;
    }

    /// <summary>
    /// Update one Destination
    /// </summary>
    public async Task UpdateDestination(
        DestinationWhereUniqueInput uniqueId,
        DestinationUpdateInput updateDto
    )
    {
        var destination = updateDto.ToModel(uniqueId);

        if (updateDto.Items != null)
        {
            destination.Items = await _context
                .Items.Where(item => updateDto.Items.Select(t => t).Contains(item.Id))
                .ToListAsync();
        }

        _context.Entry(destination).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Destinations.Any(e => e.Id == destination.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Connect multiple Items records to Destination
    /// </summary>
    public async Task ConnectItems(
        DestinationWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Destinations.Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Items.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Items);

        foreach (var child in childrenToConnect)
        {
            parent.Items.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Items records from Destination
    /// </summary>
    public async Task DisconnectItems(
        DestinationWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Destinations.Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Items.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Items?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Items records for Destination
    /// </summary>
    public async Task<List<Item>> FindItems(
        DestinationWhereUniqueInput uniqueId,
        ItemFindManyArgs destinationFindManyArgs
    )
    {
        var items = await _context
            .Items.Where(m => m.DestinationId == uniqueId.Id)
            .ApplyWhere(destinationFindManyArgs.Where)
            .ApplySkip(destinationFindManyArgs.Skip)
            .ApplyTake(destinationFindManyArgs.Take)
            .ApplyOrderBy(destinationFindManyArgs.SortBy)
            .ToListAsync();

        return items.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Items records for Destination
    /// </summary>
    public async Task UpdateItems(
        DestinationWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var destination = await _context
            .Destinations.Include(t => t.Items)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (destination == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Items.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        destination.Items = children;
        await _context.SaveChangesAsync();
    }
}
