using BlazorApp.Application.Interfaces;
using BlazorApp.Contracts.Api.Output;
using Microsoft.Extensions.Options;

namespace BlazorApp.Infrastructure.ApiClients.Customer
{
    public class CustomerApi(HttpClient httpClient, IOptions<CustomerConfiguration> options) : ICustomerApi
    {
        public async Task<CustomerDto> UpsertCustomerAsync(Models.Customer input, CancellationToken cancellationToken = default)
        {
            var relative = options.Value.UpsertCustomerUrl;

            using var resp = await httpClient.PostAsJsonAsync(relative, input, cancellationToken);
            resp.EnsureSuccessStatusCode();

            var dto = await resp.Content.ReadFromJsonAsync<CustomerDto>(cancellationToken: cancellationToken);
            return dto ?? throw new InvalidOperationException("Empty response from API.");
        }

        public Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationtoken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomerDto>> GetCustomersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDto> DeleteCustomerAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
