using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;

namespace BlazorApp.Application.Services.Customer;

internal class CustomerSevice(ICustomerRepository customerRepo) : ICustomerService
{
    public async Task UpsertCustomerAsync(Domain.Models.Customer customer, CancellationToken cancellationToken = default)
    {
        if (customer is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(customer.Id))
        {
            await customerRepo.InsertCustomerAsync(customer, cancellationToken);

            return;
        }

        await customerRepo.UpdateCustomerAsync(customer.Id, customer, cancellationToken);
    }

    public async Task<Domain.Models.Customer> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentNullException(nameof(customerId));
        }

        return await customerRepo.GetCustomerAsync(customerId, cancellationToken);
    }

    public Task<List<Domain.Models.Customer>> GetCustomersAsync(int pageNumber, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
        {
            return customerRepo.GetCustomersAsync(1, cancellationToken);
        }

        return customerRepo.GetCustomersAsync(pageNumber, cancellationToken);
    }

    public async Task DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId) || Convert.ToInt32(customerId) <= 0)
        {
            return;
        }

        await customerRepo.DeleteCustomerAsync(customerId, cancellationToken);
    }
}
