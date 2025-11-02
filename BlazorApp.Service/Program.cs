using BlazorApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
        
        builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "https://demo.duendesoftware.com";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                //NameClaimType = "name",
                //RoleClaimType = "role"
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", p =>
            {
                p.RequireAuthenticatedUser();
                p.RequireAssertion(ctx =>
                    ctx.User.HasClaim("scope", "api") || ctx.User.HasClaim("scp", "api"));
            });
        });
        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers().RequireAuthorization("ApiScope"); ;

        app.Run();
    }
}