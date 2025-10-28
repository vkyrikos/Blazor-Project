using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Infrastructure.Database.Context;
using BlazorApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Infrastructure;

public static class DependencyRegistration
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<AssignmentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.EnableRetryOnFailure();
                    sql.CommandTimeout(30);
                }));

        services.AddSingleton<ICustomerRepository, CustomerRepository>();
    }
}
