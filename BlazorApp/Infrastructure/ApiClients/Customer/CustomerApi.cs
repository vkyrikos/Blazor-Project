using BlazorApp.Application.Interfaces;
using BlazorApp.Contracts.Api.Output;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BlazorApp.Infrastructure.ApiClients.Customer
{
    public class CustomerApi(HttpClient httpClient, IOptions<CustomerConfiguration> options, JsonSerializerOptions jsonOptions) : ICustomerApi
    {
        public async Task<CustomerDto> UpsertCustomerAsync(Models.Customer input, CancellationToken cancellationToken = default)
        {
            var relative = options.Value.UpsertCustomerUrl;

            using var resp = await httpClient.PostAsJsonAsync(relative, input, cancellationToken);
            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions);
        }

        public async Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationtoken = default)
        {
            var relative = options.Value.GetCustomerUrl + id.ToString();

            using var resp = await httpClient.GetAsync(relative);
            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions);
        }

        public async Task<List<CustomerDto>> GetCustomersAsync(int page, CancellationToken cancellationToken = default)
        {
            var relative = options.Value.GetCustomersUrl + page.ToString();

            using var resp = await httpClient.GetAsync(relative);
            resp.EnsureSuccessStatusCode();

            var test = await resp.Content.ReadFromJsonAsync<List<CustomerDto>>(jsonOptions);

            return test;
        }

        public async Task<CustomerDto> DeleteCustomerAsync(int id, CancellationToken cancellationToken = default)
        {
            var relative = options.Value.DeleteCustomerUrl + id.ToString();

            using var resp = await httpClient.PostAsJsonAsync(relative, cancellationToken);
            resp.EnsureSuccessStatusCode();

            var test = await resp.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions);

            return test;
        }
    }
}
