using BlazorApp.Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.Infrastructure;
using BlazorApp.Infrastructure.ApiClients.Customer;
using BlazorApp.Infrastructure.Converters;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using System.Net.Http.Headers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddOptions<CustomerConfiguration>()
    .Bind(builder.Configuration.GetSection("CustomerConfiguration"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(_ =>
{
    var json = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };
    json.Converters.Add(new UnwrapDataJsonConverterFactory());
    return json;
});

builder.Services.AddHttpClient<ICustomerApi, CustomerApi>((sp, client) =>
{
    var cfg = sp.GetRequiredService<IOptions<CustomerConfiguration>>().Value;
    client.BaseAddress = new Uri(cfg.BaseUrl);
})
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(r => (int)r.StatusCode == 429 || (int)r.StatusCode >= 500)
    .WaitAndRetryAsync(5, i => TimeSpan.FromMilliseconds(100 * i)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.Run();
