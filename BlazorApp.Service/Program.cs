using BlazorApp.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    static async Task Main(string[] args)
    {
        var appBuilder = WebApplication.CreateBuilder(args);

        appBuilder.Services
            .AddHostedService<Service>()
            .RegisterServiceDependencies();

        appBuilder.Services.AddControllers();

        // Need to add request filter.

        var app = appBuilder.Build();

        app.Run();
    }
}