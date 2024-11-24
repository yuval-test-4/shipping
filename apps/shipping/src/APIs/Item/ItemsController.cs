using Microsoft.AspNetCore.Mvc;

namespace Shipping.APIs;

[ApiController()]
public class ItemsController : ItemsControllerBase
{
    public ItemsController(IItemsService service)
        : base(service) { }
}
