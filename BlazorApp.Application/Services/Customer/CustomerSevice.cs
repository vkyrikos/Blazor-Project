using BlazorApp.Application.Common;
using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Application.Validation;
using BlazorApp.Domain;
using BlazorApp.Domain.Requests;
using Microsoft.Extensions.Logging;
using DomainCustomer = BlazorApp.Domain.Models.Customer;

namespace BlazorApp.Application.Services.Customer;

internal class CustomerSevice(ILogger<CustomerSevice> logger, ICustomerRepository customerRepo) : ICustomerService
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

    public async Task<IServiceResponse<int>> UpsertCustomerAsync(DomainCustomer customer, CancellationToken cancellationToken = default)
    {
        if (customer is null)
            return ServiceResponse<int>.Failure(new Error(ErrorCode.Validation, CustomerPayloadRequired));

        var isInsert = customer.Id <= 0;

        try
        {
            var result = isInsert
                ? await customerRepo
                    .InsertCustomerAsync(customer, cancellationToken)
                : await customerRepo
                    .UpdateCustomerAsync(customer.Id, customer, cancellationToken);

            if (isInsert)
            {
                return ServiceResponse<int>.Success(result);
            }

            return result switch
            {
                -1 => ServiceResponse<int>.Failure(new Error(ErrorCode.NotFound, NotFoundUpdateCustomer)),
                0 => ServiceResponse<int>.Failure(new Error(ErrorCode.Business, "Nothing to update")),
                _ => ServiceResponse<int>.Success(result)
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            var logMessage = isInsert ? CustomerNotInserted : string.Format(CustomerNotUpdatedFormatted, customer.Id);
            logger.LogError(ex, logMessage);

            var serviceErrorMessage = isInsert ? InsertCustomerUnexpectedError : UpdateCustomerUnexpectedError;
            return ServiceResponse<int>.Failure(new Error(ErrorCode.Generic, serviceErrorMessage));
        }
    }

    public async Task<IServiceResponse<DomainCustomer>> GetCustomerAsync(GetCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.CustomerId <= 0)
            return ServiceResponse<DomainCustomer>.Failure(new Error(ErrorCode.Validation, CustomerIdMustBePositive));

        try
        {
            var customer = await customerRepo
                .GetCustomerAsync(request.CustomerId, cancellationToken);

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
                new Error(ErrorCode.Generic, GetCustomersUnexpectedError));
        }
    }

    public async Task<IServiceResponse<int>> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request is null || request.CustromerId <= 0)
            return ServiceResponse<int>.Failure(new Error(ErrorCode.Validation, CustomerIdMustBePositive));

        try
        {
            var affected = await customerRepo.DeleteCustomerAsync(request, cancellationToken);

            return ServiceResponse<int>.Success(affected);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return ServiceResponse<int>.Failure(
                new Error(ErrorCode.Generic, string.Format(DeleteUnexpectedError, request.CustromerId)));
        }
    }
}
