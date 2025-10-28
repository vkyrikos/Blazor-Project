using BlazorApp.ApiHost.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace BlazorApp.ApiHost;

public static class DependencyRegistration
{
    public static void RegisterApiHostDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers()
            .AddApplicationPart(typeof(CustomerController).Assembly)
            .AddJsonOptions(o =>
                o.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));
    }
}
