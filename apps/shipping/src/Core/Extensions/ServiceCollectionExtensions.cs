using Shipping.APIs;

namespace Shipping;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationsService, DestinationsService>();
        services.AddScoped<IItemsService, ItemsService>();
        services.AddScoped<IShipmentsService, ShipmentsService>();
    }
}
