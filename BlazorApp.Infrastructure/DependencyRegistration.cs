using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Infrastructure;

public static class DependencyRegistration
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
    }
}
