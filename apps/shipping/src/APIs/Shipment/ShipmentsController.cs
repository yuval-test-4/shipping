using Microsoft.AspNetCore.Mvc;

namespace Shipping.APIs;

[ApiController()]
public class ShipmentsController : ShipmentsControllerBase
{
    public ShipmentsController(IShipmentsService service)
        : base(service) { }
}
