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

        var app = appBuilder.Build();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}