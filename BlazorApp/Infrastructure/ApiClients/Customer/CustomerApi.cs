using BlazorApp.Application.Interfaces;
using BlazorApp.Contracts.Api.Input;
using BlazorApp.Contracts.Api.Output;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorApp.Infrastructure.ApiClients.Customer;

public class CustomerApi(HttpClient client, IOptions<CustomerConfiguration> options, JsonSerializerOptions jsonOptions) : ICustomerApi
{
    private static readonly Uri TokenEndpoint = new("https://demo.duendesoftware.com/connect/token");
    public async Task<CustomerDto> UpsertCustomerAsync(Models.Customer input, CancellationToken cancellationToken = default)
    {
        var relative = options.Value.UpsertCustomerUrl;
        
        var token = await GetAccessTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync(relative, input, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions, cancellationToken);
    }

    public async Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationtoken = default)
    {
        var relative = options.Value.GetCustomerUrl + id.ToString();

        var token = await GetAccessTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync(relative);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions, cancellationtoken);
    }

    public async Task<List<CustomerDto>> GetCustomersAsync(int page, CancellationToken cancellationToken = default)
    {
        var relative = options.Value.GetCustomersUrl + page.ToString();

        var token = await GetAccessTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync(relative);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CustomerDto>>(jsonOptions, cancellationToken);
    }

    public async Task DeleteCustomerAsync(int id, CancellationToken cancellationToken = default)
    {
        var relative = options.Value.DeleteCustomerUrl;
        var deleteRequest = new DeleteCustomerRequest()
        {
            CustomerId = id,
        };

        var token = await GetAccessTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync(relative, deleteRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetAccessTokenAsync()
    {
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = "m2m",
            ["client_secret"] = "secret",
            ["grant_type"] = "client_credentials",
            ["scope"] = "api"
        });

        var resp = await client.PostAsync(TokenEndpoint, form);
        resp.EnsureSuccessStatusCode();

        using var s = await resp.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(s);
        return json.RootElement.GetProperty("access_token").GetString()!;
    }
}
