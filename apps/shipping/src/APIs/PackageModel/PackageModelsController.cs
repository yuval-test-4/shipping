using Microsoft.AspNetCore.Mvc;

namespace Shipping.APIs;

[ApiController()]
public class PackageModelsController : PackageModelsControllerBase
{
    public PackageModelsController(IPackageModelsService service)
        : base(service) { }
}
