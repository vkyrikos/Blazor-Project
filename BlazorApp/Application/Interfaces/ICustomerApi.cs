using BlazorApp.Contracts.Api.Output;
using BlazorApp.Models;

namespace BlazorApp.Application.Interfaces;

public interface ICustomerApi
{
    Task<CustomerDto> UpsertCustomerAsync(Customer input, CancellationToken cancellationToken = default);
    Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationToken = default);
    Task<List<CustomerDto>> GetCustomersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<CustomerDto> DeleteCustomerAsync(int id, CancellationToken cancellationToken = default);
}
