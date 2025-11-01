using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.ApiHost.Mapping;

public static class UpsertRequestExtensionMapping
{
    public static UpsertCustomerRequestModel ToDomain(this UpsertCustomerRequest upsertRequest) => new()
    {
        Customer = new()
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
        }
    };
}
