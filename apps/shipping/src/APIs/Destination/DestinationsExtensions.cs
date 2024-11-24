using Shipping.APIs.Dtos;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs.Extensions;

public static class DestinationsExtensions
{
    public static Destination ToDto(this DestinationDbModel model)
    {
        return new Destination
        {
            Address = model.Address,
            City = model.City,
            Country = model.Country,
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Items = model.Items?.Select(x => x.Id).ToList(),
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static DestinationDbModel ToModel(
        this DestinationUpdateInput updateDto,
        DestinationWhereUniqueInput uniqueId
    )
    {
        var destination = new DestinationDbModel
        {
            Id = uniqueId.Id,
            Address = updateDto.Address,
            City = updateDto.City,
            Country = updateDto.Country
        };

        if (updateDto.CreatedAt != null)
        {
            destination.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            destination.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return destination;
    }
}
