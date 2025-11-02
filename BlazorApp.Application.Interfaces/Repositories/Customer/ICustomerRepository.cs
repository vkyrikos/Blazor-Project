using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Interfaces.Repositories.Customer;

public interface ICustomerRepository
{
    Task<Domain.Models.Customer> InsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<Domain.Models.Customer?> UpdateCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<Domain.Models.Customer?> GetCustomerAsync(int customerId, CancellationToken cancellationToken);
    Task<List<Domain.Models.Customer>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken);
    Task<Domain.Models.Customer> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken);
}
