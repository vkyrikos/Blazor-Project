using BlazorApp.Application.Cache;
using BlazorApp.Application.Common;
using BlazorApp.Application.Interfaces.Cache;
using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Validation;
using BlazorApp.Domain;
using BlazorApp.Domain.Requests;
using Microsoft.Extensions.Logging;
using DomainCustomer = BlazorApp.Domain.Models.Customer;

namespace BlazorApp.Application.Services.Customer;

internal class CustomerSevice(ILogger<CustomerSevice> logger, ICustomerRepository customerRepo, ICache cache) : ICustomerService
{
    private const string CustomerPayloadRequired = "Customer payload is required.";
    private const string CustomerIdMustBePositive = "The 'customerId' must be greater than 0.";
    private const string PageNumberMustBePositive = "The 'pageNumber' must be greater than 0.";
    private const string NotFoundUpdateCustomer = "Trying to update a non existent customer.";

    private const string UpdateCustomerUnexpectedError = "An unexpected error occurred while updating the customer.";
    private const string InsertCustomerUnexpectedError = "An unexpected error occurred while inserting the customer.";
    private const string GetCustomerUnexpectedError = "An unexpected error occurred while retrieving the customer.";
    private const string GetCustomersUnexpectedError = "An unexpected error occurred while retrieving customers.";
    private const string DeleteUnexpectedError = "An unexpected error occurred while deleting the customer with Id: {0}.";

    private const string CustomerNotUpdatedFormatted = "Failed to update customer with Id: {0}.";
    private const string CustomerNotInserted = "Failed to insert customer.";
    private const string CustomerNotFoundFormatted = "Customer with Id {0} was not found.";

    public async Task<IServiceResponse<DomainCustomer?>> UpsertCustomerAsync(UpsertCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return ServiceResponse<DomainCustomer?>.Failure(new Error(ErrorCode.Validation, CustomerPayloadRequired));

        try
        {

            return request.Customer.Id switch
            {
                0 => await InsertCustomerAsync(request, cancellationToken),
                >= 1 => await UpdateCustomerAsync(request, cancellationToken)
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            var isInsert = request.Customer.Id > 0;

            var logMessage = isInsert ? CustomerNotInserted : string.Format(CustomerNotUpdatedFormatted, request.Customer.Id);
            logger.LogError(ex, logMessage);

            var serviceErrorMessage = isInsert ? InsertCustomerUnexpectedError : UpdateCustomerUnexpectedError;
            return ServiceResponse<DomainCustomer?>.Failure(new Error(ErrorCode.Generic, serviceErrorMessage));
        }
    }

    public async Task<IServiceResponse<DomainCustomer>> GetCustomerAsync(GetCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.CustomerId <= 0)
            return ServiceResponse<DomainCustomer>.Failure(new Error(ErrorCode.Validation, CustomerIdMustBePositive));

        try
        {
            var customer = await cache.GetOrCreateAsync(
                CacheRoutingKeys.GetCustomerRoutingKey(request.CustomerId),
                async ct => await customerRepo.GetCustomerAsync(request.CustomerId, cancellationToken), 
                cancellationToken); 

            if (customer is null)
            {
                return ServiceResponse<DomainCustomer>.Failure(new Error(ErrorCode.NotFound, string.Format(CustomerNotFoundFormatted, request.CustomerId)));
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
                new Error(ErrorCode.Generic, GetCustomerUnexpectedError));
        }
    }

    public async Task<IServiceResponse<List<DomainCustomer>>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.PageNumber <= 0)
            return ServiceResponse<List<DomainCustomer>>.Failure(new Error(ErrorCode.Validation, PageNumberMustBePositive));

        try
        {
            var cachedPage = await cache.GetOrCreateAsync(
                CacheRoutingKeys.GetCustomersPageRoutingKey(request.PageNumber),
                async ct => await customerRepo.GetCustomersAsync(request, cancellationToken),
                cancellationToken);

            return ServiceResponse<List<DomainCustomer>>.Success(cachedPage);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return ServiceResponse<List<DomainCustomer>>.Failure(
                new Error(ErrorCode.Generic, GetCustomersUnexpectedError));
        }
    }

    public async Task<IServiceResponse<DomainCustomer>> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request is null || request.CustomerId <= 0)
            return ServiceResponse<DomainCustomer>.Failure(new Error(ErrorCode.Validation, CustomerIdMustBePositive));

        try
        {
            var affected = await customerRepo.DeleteCustomerAsync(request, cancellationToken);

            if (affected is not null)
            {
                cache.Remove(CacheRoutingKeys.GetCustomerRoutingKey(request.CustomerId));


            }

            return ServiceResponse<DomainCustomer>.Success(affected);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return ServiceResponse<DomainCustomer>.Failure(
                new Error(ErrorCode.Generic, string.Format(DeleteUnexpectedError, request.CustomerId)));
        }
    }

    private async Task<IServiceResponse<DomainCustomer?>> UpdateCustomerAsync(UpsertCustomerRequestModel request, CancellationToken cancellationToken)
    {
        var result = await customerRepo.UpdateCustomerAsync(request.Customer, cancellationToken);

        return result switch
        {
            null => ServiceResponse<DomainCustomer?>.Failure(new Error(ErrorCode.NotFound, NotFoundUpdateCustomer)),
            _ => ServiceResponse<DomainCustomer?>.Success(result)
        };
    }

    private async Task<IServiceResponse<DomainCustomer?>> InsertCustomerAsync(UpsertCustomerRequestModel request, CancellationToken cancellationToken)
    {
        var insertResult = await customerRepo
            .InsertCustomerAsync(request.Customer, cancellationToken);

        return ServiceResponse<DomainCustomer?>.Success(insertResult);
    }
}
