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
        if (createDto.PackageField != null)
        {
            shipment.PackageField = await _context
                .PackageModels.Where(packageModel => createDto.PackageField.Id == packageModel.Id)
                .FirstOrDefaultAsync();
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
            .Shipments.Include(x => x.PackageField)
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

        if (updateDto.PackageField != null)
        {
            shipment.PackageField = await _context
                .PackageModels.Where(packageModel => updateDto.PackageField == packageModel.Id)
                .FirstOrDefaultAsync();
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
    /// Get a Package record for Shipment
    /// </summary>
    public async Task<PackageModel> GetPackageField(ShipmentWhereUniqueInput uniqueId)
    {
        var shipment = await _context
            .Shipments.Where(shipment => shipment.Id == uniqueId.Id)
            .Include(shipment => shipment.PackageField)
            .FirstOrDefaultAsync();
        if (shipment == null)
        {
            throw new NotFoundException();
        }
        return shipment.PackageField.ToDto();
    }
}
