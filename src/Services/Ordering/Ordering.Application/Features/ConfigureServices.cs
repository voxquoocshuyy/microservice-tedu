using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviours;

namespace Ordering.Application.Features;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
    services.AddAutoMapper(Assembly.GetExecutingAssembly())
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
        .AddMediatR(Assembly.GetExecutingAssembly())
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandleExceptionBehaviour<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
    ;
}