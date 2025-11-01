using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Interfaces.Services.Customer;

public interface ICustomerService
{
    Task<IServiceResponse<Domain.Models.Customer?>> UpsertCustomerAsync(UpsertCustomerRequestModel request, CancellationToken cancellationToken = default);
    Task<IServiceResponse<Domain.Models.Customer>> GetCustomerAsync(GetCustomerRequestModel request, CancellationToken cancellationToken = default);
    Task<IServiceResponse<List<Domain.Models.Customer>>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken = default);
    Task<IServiceResponse<int>> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken = default);
}
