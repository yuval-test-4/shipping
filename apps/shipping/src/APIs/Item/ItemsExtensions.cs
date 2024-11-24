using Shipping.APIs.Dtos;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs.Extensions;

public static class ItemsExtensions
{
    public static Item ToDto(this ItemDbModel model)
    {
        return new Item
        {
            CreatedAt = model.CreatedAt,
            Destination = model.DestinationId,
            Id = model.Id,
            Name = model.Name,
            Quantity = model.Quantity,
            Shipment = model.ShipmentId,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ItemDbModel ToModel(this ItemUpdateInput updateDto, ItemWhereUniqueInput uniqueId)
    {
        var item = new ItemDbModel
        {
            Id = uniqueId.Id,
            Name = updateDto.Name,
            Quantity = updateDto.Quantity
        };

        if (updateDto.CreatedAt != null)
        {
            item.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Destination != null)
        {
            item.DestinationId = updateDto.Destination;
        }
        if (updateDto.Shipment != null)
        {
            item.ShipmentId = updateDto.Shipment;
        }
        if (updateDto.UpdatedAt != null)
        {
            item.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return item;
    }
}
