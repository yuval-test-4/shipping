using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shipping.Infrastructure;

public class ShippingDbContext : IdentityDbContext<IdentityUser>
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
        : base(options) { }
}
