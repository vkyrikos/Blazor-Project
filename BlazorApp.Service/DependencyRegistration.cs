using BlazorApp.ApiHost;
using BlazorApp.Infrastructure;
using BlazorApp.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Service;

internal static class DependencyRegistration
{
    internal static IServiceCollection RegisterServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();

        services.RegisterInfrastructureDependencies(configuration);
        services.RegisterApplicationDependencies();
        services.RegisterApiHostDependencies();

        return services;
    }
}
