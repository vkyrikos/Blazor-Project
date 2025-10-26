using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Services.Customer;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Application;

public static class DependencyRegistration
{
    public static void RegisterApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerService, CustomerSevice>();
    }
}
