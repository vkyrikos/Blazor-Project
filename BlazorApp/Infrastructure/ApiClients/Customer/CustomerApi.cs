using BlazorApp.Application.Interfaces;
using BlazorApp.Contracts.Api.Input;
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

            using var response = await httpClient.PostAsJsonAsync(relative, input, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions, cancellationToken);
        }

        public async Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationtoken = default)
        {
            var relative = options.Value.GetCustomerUrl + id.ToString();

            using var response = await httpClient.GetAsync(relative);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CustomerDto>(jsonOptions, cancellationtoken);
        }

        public async Task<List<CustomerDto>> GetCustomersAsync(int page, CancellationToken cancellationToken = default)
        {
            var relative = options.Value.GetCustomersUrl + page.ToString();

            using var response = await httpClient.GetAsync(relative);
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

            using var response = await httpClient.PostAsJsonAsync(relative, deleteRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
