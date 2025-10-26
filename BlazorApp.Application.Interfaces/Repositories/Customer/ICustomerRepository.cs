namespace BlazorApp.Application.Interfaces.Repositories.Customer;

public interface ICustomerRepository
{
    Task InsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken);
    Task<Domain.Models.Customer> GetCustomerAsync(string customerId, CancellationToken cancellationToken);
}
