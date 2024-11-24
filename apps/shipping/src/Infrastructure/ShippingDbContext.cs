using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shipping.Infrastructure.Models;

namespace Shipping.Infrastructure;

public class ShippingDbContext : IdentityDbContext<IdentityUser>
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
        : base(options) { }

    public DbSet<ShipmentDbModel> Shipments { get; set; }

    public DbSet<DestinationDbModel> Destinations { get; set; }

    public DbSet<ItemDbModel> Items { get; set; }
}
