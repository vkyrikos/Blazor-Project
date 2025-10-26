using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorApp.ApiHost;

public static class DependencyRegistration
{
    public static void RegisterApiHostDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    //public static void RegisterValidators(this ServiceCollection services)
    //{
    //    var validatorTypes = Assembly
    //        .GetExecutingAssembly()
    //        .GetTypes()
    //        .Where(t => t is { IsAbstract: false, IsInterface: false })
    //        .SelectMany(t => t.GetInterfaces())
    //        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator))
    //}
}
