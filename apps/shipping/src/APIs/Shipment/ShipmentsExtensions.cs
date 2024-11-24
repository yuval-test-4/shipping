using Shipping.APIs.Dtos;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs.Extensions;

public static class ShipmentsExtensions
{
    public static Shipment ToDto(this ShipmentDbModel model)
    {
        return new Shipment
        {
            ArrivalTime = model.ArrivalTime,
            CreatedAt = model.CreatedAt,
            DepartureTime = model.DepartureTime,
            Id = model.Id,
            PackageField = model.PackageFieldId,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ShipmentDbModel ToModel(
        this ShipmentUpdateInput updateDto,
        ShipmentWhereUniqueInput uniqueId
    )
    {
        var shipment = new ShipmentDbModel
        {
            Id = uniqueId.Id,
            ArrivalTime = updateDto.ArrivalTime,
            DepartureTime = updateDto.DepartureTime
        };

        if (updateDto.CreatedAt != null)
        {
            shipment.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.PackageField != null)
        {
            shipment.PackageFieldId = updateDto.PackageField;
        }
        if (updateDto.UpdatedAt != null)
        {
            shipment.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return shipment;
    }
}
