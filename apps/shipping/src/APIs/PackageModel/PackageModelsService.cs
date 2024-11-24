using Shipping.Infrastructure;

namespace Shipping.APIs;

public class PackageModelsService : PackageModelsServiceBase
{
    public PackageModelsService(ShippingDbContext context)
        : base(context) { }
}
