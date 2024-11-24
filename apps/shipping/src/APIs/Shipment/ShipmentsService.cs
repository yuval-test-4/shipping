using Shipping.Infrastructure;

namespace Shipping.APIs;

public class ShipmentsService : ShipmentsServiceBase
{
    public ShipmentsService(ShippingDbContext context)
        : base(context) { }
}
