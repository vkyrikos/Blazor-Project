using BlazorApp.Application.Interfaces.Common;

namespace BlazorApp.Application.Interfaces.Services.Customer;

public interface ICustomerService
{
    //Task<IServiceResponse<int>> UpsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<IServiceResponse<Domain.Models.Customer>> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    //Task<IServiceResponse<List<Domain.Models.Customer>>> GetCustomersAsync(int pageNumber, CancellationToken cancellationToken = default);
    //Task<IServiceResponse<int>> DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default);
}
