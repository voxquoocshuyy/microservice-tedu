using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigurationServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryHttpRepository, InventoryHttpRepository>();
        return services;
    }
    public static IServiceCollection ConfigurationHttpRepository(this IServiceCollection services)
    {
        services.AddTransient<ICheckoutSagaService, CheckoutSagaService>();
        services.AddScoped<IOrderHttpRepository, OrderHttpRepository>();
        services.AddScoped<IBasketHttpRepository, BasketHttpRepository>();
        return services;
    }

    public static void ConfigurationHttpClients(this IServiceCollection services)
    {
        ConfigureOrderHttpClient(services);
        ConfigureBasketHttpClient(services);
        ConfigureInventoryHttpClient(services);
    }
    private static void ConfigureOrderHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>("OrdersAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("https://localhost:5005/api/v1");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("OrdersAPI"));
    }

    private static void ConfigureBasketHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>("BasketsAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("https://localhost:5005/api/v1");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("BasketsAPI"));
    }

    private static void ConfigureInventoryHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IInventoryHttpRepository, InventoryHttpRepository>("InventoryAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("https://localhost:5005/api/v1");
        });
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("InventoryAPI"));
    }
}