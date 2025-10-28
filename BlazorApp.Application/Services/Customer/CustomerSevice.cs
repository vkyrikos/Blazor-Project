using BlazorApp.Application.Common;
using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Validation;
using BlazorApp.Domain;
using DomainCustomer = BlazorApp.Domain.Models.Customer;

namespace BlazorApp.Application.Services.Customer;

internal class CustomerSevice(ICustomerRepository customerRepo) : ICustomerService
{
    public async Task<IServiceResponse<DomainCustomer>> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return ServiceResponse<DomainCustomer>.Failure(
                new Error(ErrorCode.Validation, "customerId is required."));
        }

        try
        {
            var customer = await customerRepo
                .GetCustomerAsync(customerId, cancellationToken)
                .ConfigureAwait(false);

            if (customer is null)
            {
                return ServiceResponse<DomainCustomer>.Failure(
                    new Error(ErrorCode.NotFound, $"Customer '{customerId}' was not found."));
            }

            return ServiceResponse<DomainCustomer>.Success(customer);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "Failed to get customer {CustomerId}", customerId);
            return ServiceResponse<DomainCustomer>.Failure(
                new Error(ErrorCode.Generic, "Unexpected error while fetching customer."));
        }
    }
    
    public async Task UpsertCustomerAsync(DomainCustomer customer, CancellationToken cancellationToken = default)
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



    public Task<List<DomainCustomer>> GetCustomersAsync(int pageNumber, CancellationToken cancellationToken = default)
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
