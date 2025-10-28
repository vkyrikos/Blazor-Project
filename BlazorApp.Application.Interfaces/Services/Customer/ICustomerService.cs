using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Interfaces.Services.Customer;

public interface ICustomerService
{
    Task<IServiceResponse<int>> UpsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<IServiceResponse<Domain.Models.Customer>> GetCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IServiceResponse<List<Domain.Models.Customer>>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken = default);
    Task<IServiceResponse<int>> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default);
}
