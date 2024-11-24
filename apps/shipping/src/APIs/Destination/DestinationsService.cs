using Shipping.Infrastructure;

namespace Shipping.APIs;

public class DestinationsService : DestinationsServiceBase
{
    public DestinationsService(ShippingDbContext context)
        : base(context) { }
}
