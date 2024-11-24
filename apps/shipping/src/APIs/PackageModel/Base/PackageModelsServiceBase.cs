using Microsoft.EntityFrameworkCore;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;
using Shipping.APIs.Extensions;
using Shipping.Infrastructure;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs;

public abstract class PackageModelsServiceBase : IPackageModelsService
{
    protected readonly ShippingDbContext _context;

    public PackageModelsServiceBase(ShippingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Package
    /// </summary>
    public async Task<PackageModel> CreatePackageModel(PackageModelCreateInput createDto)
    {
        var packageModel = new PackageModelDbModel
        {
            CreatedAt = createDto.CreatedAt,
            TrackingNumber = createDto.TrackingNumber,
            UpdatedAt = createDto.UpdatedAt,
            Weight = createDto.Weight
        };

        if (createDto.Id != null)
        {
            packageModel.Id = createDto.Id;
        }
        if (createDto.Destination != null)
        {
            packageModel.Destination = await _context
                .Destinations.Where(destination => createDto.Destination.Id == destination.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Shipments != null)
        {
            packageModel.Shipments = await _context
                .Shipments.Where(shipment =>
                    createDto.Shipments.Select(t => t.Id).Contains(shipment.Id)
                )
                .ToListAsync();
        }

        _context.PackageModels.Add(packageModel);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<PackageModelDbModel>(packageModel.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Package
    /// </summary>
    public async Task DeletePackageModel(PackageModelWhereUniqueInput uniqueId)
    {
        var packageModel = await _context.PackageModels.FindAsync(uniqueId.Id);
        if (packageModel == null)
        {
            throw new NotFoundException();
        }

        _context.PackageModels.Remove(packageModel);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Packages
    /// </summary>
    public async Task<List<PackageModel>> PackageModels(PackageModelFindManyArgs findManyArgs)
    {
        var packageModels = await _context
            .PackageModels.Include(x => x.Shipments)
            .Include(x => x.Destination)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return packageModels.ConvertAll(packageModel => packageModel.ToDto());
    }

    /// <summary>
    /// Meta data about Package records
    /// </summary>
    public async Task<MetadataDto> PackageModelsMeta(PackageModelFindManyArgs findManyArgs)
    {
        var count = await _context.PackageModels.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Package
    /// </summary>
    public async Task<PackageModel> PackageModel(PackageModelWhereUniqueInput uniqueId)
    {
        var packageModels = await this.PackageModels(
            new PackageModelFindManyArgs { Where = new PackageModelWhereInput { Id = uniqueId.Id } }
        );
        var packageModel = packageModels.FirstOrDefault();
        if (packageModel == null)
        {
            throw new NotFoundException();
        }

        return packageModel;
    }

    /// <summary>
    /// Update one Package
    /// </summary>
    public async Task UpdatePackageModel(
        PackageModelWhereUniqueInput uniqueId,
        PackageModelUpdateInput updateDto
    )
    {
        var packageModel = updateDto.ToModel(uniqueId);

        if (updateDto.Destination != null)
        {
            packageModel.Destination = await _context
                .Destinations.Where(destination => updateDto.Destination == destination.Id)
                .FirstOrDefaultAsync();
        }

        if (updateDto.Shipments != null)
        {
            packageModel.Shipments = await _context
                .Shipments.Where(shipment =>
                    updateDto.Shipments.Select(t => t).Contains(shipment.Id)
                )
                .ToListAsync();
        }

        _context.Entry(packageModel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.PackageModels.Any(e => e.Id == packageModel.Id))
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
    /// Get a Destination record for Package
    /// </summary>
    public async Task<Destination> GetDestination(PackageModelWhereUniqueInput uniqueId)
    {
        var packageModel = await _context
            .PackageModels.Where(packageModel => packageModel.Id == uniqueId.Id)
            .Include(packageModel => packageModel.Destination)
            .FirstOrDefaultAsync();
        if (packageModel == null)
        {
            throw new NotFoundException();
        }
        return packageModel.Destination.ToDto();
    }

    /// <summary>
    /// Connect multiple Shipments records to Package
    /// </summary>
    public async Task ConnectShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .PackageModels.Include(x => x.Shipments)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Shipments.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Shipments);

        foreach (var child in childrenToConnect)
        {
            parent.Shipments.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Shipments records from Package
    /// </summary>
    public async Task DisconnectShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .PackageModels.Include(x => x.Shipments)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Shipments.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Shipments?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Shipments records for Package
    /// </summary>
    public async Task<List<Shipment>> FindShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentFindManyArgs packageModelFindManyArgs
    )
    {
        var shipments = await _context
            .Shipments.Where(m => m.PackageFieldId == uniqueId.Id)
            .ApplyWhere(packageModelFindManyArgs.Where)
            .ApplySkip(packageModelFindManyArgs.Skip)
            .ApplyTake(packageModelFindManyArgs.Take)
            .ApplyOrderBy(packageModelFindManyArgs.SortBy)
            .ToListAsync();

        return shipments.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Shipments records for Package
    /// </summary>
    public async Task UpdateShipments(
        PackageModelWhereUniqueInput uniqueId,
        ShipmentWhereUniqueInput[] childrenIds
    )
    {
        var packageModel = await _context
            .PackageModels.Include(t => t.Shipments)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (packageModel == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Shipments.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        packageModel.Shipments = children;
        await _context.SaveChangesAsync();
    }
}
