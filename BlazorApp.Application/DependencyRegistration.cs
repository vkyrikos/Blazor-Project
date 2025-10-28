using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Services.Customer;
using BlazorApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Application;

public static class DependencyRegistration
{
    public static void RegisterApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<ICustomerService, CustomerSevice>();
    }
}
