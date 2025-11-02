using BlazorApp.Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.Infrastructure;
using BlazorApp.Infrastructure.ApiClients.Customer;
using BlazorApp.Infrastructure.Converters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System.IdentityModel.Tokens.Jwt;
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

var baseUrl = builder.Configuration.GetSection("CustomerConfiguration").GetSection("BaseUrl").Value!;
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Server-side auth: Cookies + OIDC against Duende demo
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://demo.duendesoftware.com";
    options.ClientId = "interactive.confidential";
    options.ClientSecret = "secret";           // demo client
    options.ResponseType = "code";

    options.SaveTokens = true;                 // keep tokens in auth session
    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;

    // Ask for an access token for your API and a refresh token
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api");
    options.Scope.Add("offline_access");       // enables silent refresh

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ICustomerApi, CustomerApi>((sp, client) =>
{
    client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.Run();
