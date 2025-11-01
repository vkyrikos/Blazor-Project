using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Interfaces.Repositories.Customer;

public interface ICustomerRepository
{
    Task<int> InsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<int> UpdateCustomerAsync(int customerId, Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<Domain.Models.Customer> GetCustomerAsync(int customerId, CancellationToken cancellationToken);
    Task<List<Domain.Models.Customer>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken);
    Task<int> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken);
}
