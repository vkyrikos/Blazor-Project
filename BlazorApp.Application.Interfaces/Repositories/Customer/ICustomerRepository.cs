namespace BlazorApp.Application.Interfaces.Repositories.Customer;

public interface ICustomerRepository
{
    Task InsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken);
    Task UpdateCustomerAsync(string customerId, Domain.Models.Customer customer, CancellationToken cancellationToken);
    Task<Domain.Models.Customer> GetCustomerAsync(string customerId, CancellationToken cancellationToken);
    Task<List<Domain.Models.Customer>> GetCustomersAsync(int pageId, CancellationToken cancellationToken);
    Task DeleteCustomerAsync(string customerId, CancellationToken cancellationToken);
}
