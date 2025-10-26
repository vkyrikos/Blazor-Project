using BlazorApp.ApiHost;
using BlazorApp.Infrastructure;
using BlazorApp.Application;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Service;

internal static class DependencyRegistration
{
    internal static IServiceCollection RegisterServiceDependencies(this IServiceCollection services)
    {
        services.AddLogging();

        services.RegisterInfrastructureDependencies();
        services.RegisterApplicationDependencies();
        services.RegisterApiHostDependencies();

        return services;
    }
}
