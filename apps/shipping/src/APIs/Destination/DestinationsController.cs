using Microsoft.AspNetCore.Mvc;

namespace Shipping.APIs;

[ApiController()]
public class DestinationsController : DestinationsControllerBase
{
    public DestinationsController(IDestinationsService service)
        : base(service) { }
}
