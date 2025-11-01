using BlazorApp.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddHostedService<Service>()
            .RegisterServiceDependencies(builder.Configuration);

        builder.Services.AddMemoryCache(options => new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(5),
            Priority = CacheItemPriority.Normal
        });

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}