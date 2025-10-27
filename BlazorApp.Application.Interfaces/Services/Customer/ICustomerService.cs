namespace BlazorApp.Application.Interfaces.Services.Customer;

public interface ICustomerService
{
    Task UpsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default);
    Task<Domain.Models.Customer> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<List<Domain.Models.Customer>> GetCustomersAsync(int pageNumber, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default);
}
