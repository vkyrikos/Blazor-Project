using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.ApiHost;

public static class DependencyRegistration
{
    public static void RegisterApiHostDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers();
    }
}
