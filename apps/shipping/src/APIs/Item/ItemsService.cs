using Shipping.Infrastructure;

namespace Shipping.APIs;

public class ItemsService : ItemsServiceBase
{
    public ItemsService(ShippingDbContext context)
        : base(context) { }
}
