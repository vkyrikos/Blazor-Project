using BlazorApp.Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.Infrastructure.ApiClients.Customer;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

var apiBase = builder.Configuration["ExternalApi:BaseUrl"]!;

builder.Services.AddHttpClient<ICustomerApi, CustomerApi>(client =>
{
    client.BaseAddress = new Uri(apiBase);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CustomersWasm/1.0");
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
