using Inventory.Product.API.Persistence;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrationDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var settings = services.GetRequiredService<DatabaseSettings>();
        if(settings == null || string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new ArgumentNullException(nameof(DatabaseSettings));
        }
        try
        {
            var mongoClient = services.GetRequiredService<IMongoClient>();
            var inventoryDbSeed = new InventoryDbSeed();
            inventoryDbSeed.SeedDataAsync(mongoClient, settings).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
        return host;
    }
}