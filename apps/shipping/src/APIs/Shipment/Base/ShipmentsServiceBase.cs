using Microsoft.EntityFrameworkCore;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;
using Shipping.APIs.Extensions;
using Shipping.Infrastructure;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs;

public abstract class ShipmentsServiceBase : IShipmentsService
{
    protected readonly ShippingDbContext _context;

    public ShipmentsServiceBase(ShippingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Shipment
    /// </summary>
    public async Task<Shipment> CreateShipment(ShipmentCreateInput createDto)
    {
        var shipment = new ShipmentDbModel
        {
            ArrivalTime = createDto.ArrivalTime,
            CreatedAt = createDto.CreatedAt,
            DepartureTime = createDto.DepartureTime,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            shipment.Id = createDto.Id;
        }
        if (createDto.Items != null)
        {
            shipment.Items = await _context
                .Items.Where(item => createDto.Items.Select(t => t.Id).Contains(item.Id))
                .ToListAsync();
        }

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ShipmentDbModel>(shipment.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Shipment
    /// </summary>
    public async Task DeleteShipment(ShipmentWhereUniqueInput uniqueId)
    {
        var shipment = await _context.Shipments.FindAsync(uniqueId.Id);
        if (shipment == null)
        {
            throw new NotFoundException();
        }

        _context.Shipments.Remove(shipment);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Shipments
    /// </summary>
    public async Task<List<Shipment>> Shipments(ShipmentFindManyArgs findManyArgs)
    {
        var shipments = await _context
            .Shipments.Include(x => x.Items)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return shipments.ConvertAll(shipment => shipment.ToDto());
    }

    /// <summary>
    /// Meta data about Shipment records
    /// </summary>
    public async Task<MetadataDto> ShipmentsMeta(ShipmentFindManyArgs findManyArgs)
    {
        var count = await _context.Shipments.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Shipment
    /// </summary>
    public async Task<Shipment> Shipment(ShipmentWhereUniqueInput uniqueId)
    {
        var shipments = await this.Shipments(
            new ShipmentFindManyArgs { Where = new ShipmentWhereInput { Id = uniqueId.Id } }
        );
        var shipment = shipments.FirstOrDefault();
        if (shipment == null)
        {
            throw new NotFoundException();
        }

        return shipment;
    }

    /// <summary>
    /// Update one Shipment
    /// </summary>
    public async Task UpdateShipment(
        ShipmentWhereUniqueInput uniqueId,
        ShipmentUpdateInput updateDto
    )
    {
        var shipment = updateDto.ToModel(uniqueId);

        if (updateDto.Items != null)
        {
            shipment.Items = await _context
                .Items.Where(item => updateDto.Items.Select(t => t).Contains(item.Id))
                .ToListAsync();
        }

        _context.Entry(shipment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Shipments.Any(e => e.Id == shipment.Id))
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
    /// Connect multiple Items records to Shipment
    /// </summary>
    public async Task ConnectItems(
        ShipmentWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Shipments.Include(x => x.Items)
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
    /// Disconnect multiple Items records from Shipment
    /// </summary>
    public async Task DisconnectItems(
        ShipmentWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Shipments.Include(x => x.Items)
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
    /// Find multiple Items records for Shipment
    /// </summary>
    public async Task<List<Item>> FindItems(
        ShipmentWhereUniqueInput uniqueId,
        ItemFindManyArgs shipmentFindManyArgs
    )
    {
        var items = await _context
            .Items.Where(m => m.ShipmentId == uniqueId.Id)
            .ApplyWhere(shipmentFindManyArgs.Where)
            .ApplySkip(shipmentFindManyArgs.Skip)
            .ApplyTake(shipmentFindManyArgs.Take)
            .ApplyOrderBy(shipmentFindManyArgs.SortBy)
            .ToListAsync();

        return items.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Items records for Shipment
    /// </summary>
    public async Task UpdateItems(
        ShipmentWhereUniqueInput uniqueId,
        ItemWhereUniqueInput[] childrenIds
    )
    {
        var shipment = await _context
            .Shipments.Include(t => t.Items)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (shipment == null)
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

        shipment.Items = children;
        await _context.SaveChangesAsync();
    }
}
