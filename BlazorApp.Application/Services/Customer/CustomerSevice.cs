using BlazorApp.Application.Common;
using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Validation;
using BlazorApp.Domain;
using BlazorApp.Domain.Requests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using DomainCustomer = BlazorApp.Domain.Models.Customer;

namespace BlazorApp.Application.Services.Customer;

internal class CustomerSevice(ILogger<CustomerSevice> logger,ICustomerRepository customerRepo) : ICustomerService
{
    public async Task<IServiceResponse<int>> UpsertCustomerAsync(DomainCustomer customer, CancellationToken cancellationToken = default)
    {
        if (customer is null)
        {
            return ServiceResponse<int>.Failure(
                new Error(ErrorCode.Validation, "Customer data are required."));
        }

        try
        {
            if (customer.Id <= 0)
            {
                var inserted = await customerRepo.InsertCustomerAsync(customer, cancellationToken);
                return ServiceResponse<int>.Success(inserted);
            }

            var updated = await customerRepo.UpdateCustomerAsync(customer.Id, customer, cancellationToken);

            return ServiceResponse<int>.Success(updated);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return ServiceResponse<int>.Failure(
                new Error(ErrorCode.Generic, "Unexpected error while upserting customer."));
        }
    }

    public async Task<IServiceResponse<DomainCustomer>> GetCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
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
            logger.LogError(ex, ex.Message);
            return ServiceResponse<DomainCustomer>.Failure(
                new Error(ErrorCode.Generic, "Unexpected error while fetching customer."));
        }
    }

    public async Task<IServiceResponse<List<DomainCustomer>>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.PageNumber <= 0)
        {
            return ServiceResponse<List<DomainCustomer>>.Failure(
                new Error(ErrorCode.Validation, "Parameter pageNumber should be greater than 0"));
        }

        try
        {
            var result = await customerRepo.GetCustomersAsync(request, cancellationToken);

            return ServiceResponse<List<DomainCustomer>>.Success(result);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return ServiceResponse<List<DomainCustomer>>.Failure(
                new Error(ErrorCode.Generic, $"Unexpected error while fetching customer.\nMessage: {ex.Message}"));
        }
    }

    public async Task<IServiceResponse<int>> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            return ServiceResponse<int>.Failure(
                new Error(ErrorCode.Validation, "Parameter customerId should be greater than 0"));
        }

        try
        {
            var affected = await customerRepo.DeleteCustomerAsync(customerId, cancellationToken);

            if (affected == 0)
            {
                return ServiceResponse<int>.Failure(
                new Error(ErrorCode.Deletion, $"The deletion of customer with Id has not "));
            }

            return ServiceResponse<int>.Success(affected);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }

    }
}
