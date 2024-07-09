using Infrastructure.Configurations;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>();
        services.AddSingleton(emailSettings);
        return services;
    }
}