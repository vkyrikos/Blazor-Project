using BlazorApp.Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.Infrastructure;
using BlazorApp.Infrastructure.ApiClients.Customer;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddOptions<CustomerConfiguration>()
    .Bind(builder.Configuration.GetSection("CustomerConfiguration"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<ICustomerApi, CustomerApi>((sp, client) =>
{
    var cfg = sp.GetRequiredService<IOptions<CustomerConfiguration>>().Value;
    client.BaseAddress = new Uri(cfg.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
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
