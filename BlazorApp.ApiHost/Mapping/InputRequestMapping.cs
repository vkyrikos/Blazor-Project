using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Models;

namespace BlazorApp.ApiHost.Mapping;

public static class InputRequestMapping
{
    public static Customer? ToDomainCustomer(this UpsertCustomerRequest? upsertRequest)
    {
        if (upsertRequest is null)
        {
            return null;
        }

        return new Customer()
        {
            Id = upsertRequest?.Id ?? default,
            Address = upsertRequest!.Address,
            City = upsertRequest.City,
            Region = upsertRequest.Region,
            CompanyName = upsertRequest.CompanyName,
            ContactName = upsertRequest.ContactName,
            Country = upsertRequest.Country,
            Phone = upsertRequest.Phone,
            PostalCode = upsertRequest.PostalCode
        };
    }
}
