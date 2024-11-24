using Shipping.APIs.Dtos;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs.Extensions;

public static class PackageModelsExtensions
{
    public static PackageModel ToDto(this PackageModelDbModel model)
    {
        return new PackageModel
        {
            CreatedAt = model.CreatedAt,
            Destination = model.DestinationId,
            Id = model.Id,
            Shipments = model.Shipments?.Select(x => x.Id).ToList(),
            TrackingNumber = model.TrackingNumber,
            UpdatedAt = model.UpdatedAt,
            Weight = model.Weight,
        };
    }

    public static PackageModelDbModel ToModel(
        this PackageModelUpdateInput updateDto,
        PackageModelWhereUniqueInput uniqueId
    )
    {
        var packageModel = new PackageModelDbModel
        {
            Id = uniqueId.Id,
            TrackingNumber = updateDto.TrackingNumber,
            Weight = updateDto.Weight
        };

        if (updateDto.CreatedAt != null)
        {
            packageModel.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Destination != null)
        {
            packageModel.DestinationId = updateDto.Destination;
        }
        if (updateDto.UpdatedAt != null)
        {
            packageModel.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return packageModel;
    }
}
