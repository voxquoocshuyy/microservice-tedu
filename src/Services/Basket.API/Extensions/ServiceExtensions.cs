using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Grpc.Protos;
using MassTransit;
using MassTransit.Caching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Shared.Configurations;
using CacheSettings = Shared.Configurations.CacheSettings;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);

        var cacheSettings = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        services.AddSingleton(cacheSettings);

        var grpcSettings = configuration.GetSection(nameof(GrpcSettings)).Get<GrpcSettings>();
        services.AddSingleton(grpcSettings);

        return services;
    }

    public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
    {
        var settings = services.GetOptions<GrpcSettings>("GrpcSettings");
        services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(o =>
        {
            o.Address = new Uri(settings.StockUrl);
        });
        services.AddScoped<StockItemGrpcService>();

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>()
            .AddTransient<ISerializeService, SerializeService>();

        return services;
    }

    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetOptions<CacheSettings>("CacheSettings");
        if (string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new ArgumentNullException("Redis connection string is not configured.");
        }

        services.AddStackExchangeRedisCache(options => { options.Configuration = settings.ConnectionString; });
    }

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
        if (string.IsNullOrEmpty(settings.HostAddress))
        {
            throw new ArgumentNullException("Event bus setting is not configured.");
        }

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) => { cfg.Host(mqConnection); });
            // publish submit order message
            config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }
}