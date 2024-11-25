using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Saga.Orchestrator.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {

        return services;
    }

}