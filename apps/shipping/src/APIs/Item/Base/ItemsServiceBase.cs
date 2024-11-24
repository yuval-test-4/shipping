using Microsoft.EntityFrameworkCore;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;
using Shipping.APIs.Extensions;
using Shipping.Infrastructure;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs;

public abstract class ItemsServiceBase : IItemsService
{
    protected readonly ShippingDbContext _context;

    public ItemsServiceBase(ShippingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Item
    /// </summary>
    public async Task<Item> CreateItem(ItemCreateInput createDto)
    {
        var item = new ItemDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Name = createDto.Name,
            Quantity = createDto.Quantity,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            item.Id = createDto.Id;
        }
        if (createDto.Destination != null)
        {
            item.Destination = await _context
                .Destinations.Where(destination => createDto.Destination.Id == destination.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Shipment != null)
        {
            item.Shipment = await _context
                .Shipments.Where(shipment => createDto.Shipment.Id == shipment.Id)
                .FirstOrDefaultAsync();
        }

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ItemDbModel>(item.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Item
    /// </summary>
    public async Task DeleteItem(ItemWhereUniqueInput uniqueId)
    {
        var item = await _context.Items.FindAsync(uniqueId.Id);
        if (item == null)
        {
            throw new NotFoundException();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Items
    /// </summary>
    public async Task<List<Item>> Items(ItemFindManyArgs findManyArgs)
    {
        var items = await _context
            .Items.Include(x => x.Shipment)
            .Include(x => x.Destination)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return items.ConvertAll(item => item.ToDto());
    }

    /// <summary>
    /// Meta data about Item records
    /// </summary>
    public async Task<MetadataDto> ItemsMeta(ItemFindManyArgs findManyArgs)
    {
        var count = await _context.Items.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Item
    /// </summary>
    public async Task<Item> Item(ItemWhereUniqueInput uniqueId)
    {
        var items = await this.Items(
            new ItemFindManyArgs { Where = new ItemWhereInput { Id = uniqueId.Id } }
        );
        var item = items.FirstOrDefault();
        if (item == null)
        {
            throw new NotFoundException();
        }

        return item;
    }

    /// <summary>
    /// Update one Item
    /// </summary>
    public async Task UpdateItem(ItemWhereUniqueInput uniqueId, ItemUpdateInput updateDto)
    {
        var item = updateDto.ToModel(uniqueId);

        if (updateDto.Destination != null)
        {
            item.Destination = await _context
                .Destinations.Where(destination => updateDto.Destination == destination.Id)
                .FirstOrDefaultAsync();
        }

        if (updateDto.Shipment != null)
        {
            item.Shipment = await _context
                .Shipments.Where(shipment => updateDto.Shipment == shipment.Id)
                .FirstOrDefaultAsync();
        }

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Items.Any(e => e.Id == item.Id))
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
    /// Get a Destination record for Item
    /// </summary>
    public async Task<Destination> GetDestination(ItemWhereUniqueInput uniqueId)
    {
        var item = await _context
            .Items.Where(item => item.Id == uniqueId.Id)
            .Include(item => item.Destination)
            .FirstOrDefaultAsync();
        if (item == null)
        {
            throw new NotFoundException();
        }
        return item.Destination.ToDto();
    }

    /// <summary>
    /// Get a Shipment record for Item
    /// </summary>
    public async Task<Shipment> GetShipment(ItemWhereUniqueInput uniqueId)
    {
        var item = await _context
            .Items.Where(item => item.Id == uniqueId.Id)
            .Include(item => item.Shipment)
            .FirstOrDefaultAsync();
        if (item == null)
        {
            throw new NotFoundException();
        }
        return item.Shipment.ToDto();
    }
}
