using BlazorApp.Contracts.Api.Output;
using BlazorApp.Domain.Models;

namespace BlazorApp.ApiHost.Mapping.DtoMapping;

public static class CustomerDtoExtensionMapping
{
    public static CustomerDto? ToDto(this Customer? domainCustomer)
    {
        if (domainCustomer is null)
        {
            return null;
        }

        return new()
        {
            Id = domainCustomer.Id,
            CompanyName = domainCustomer.CompanyName,
            ContactName = domainCustomer.ContactName,
            Address = domainCustomer.Address,
            City = domainCustomer.City,
            Region = domainCustomer.Region,
            PostalCode = domainCustomer.PostalCode,
            Country = domainCustomer.Country,
            Phone = domainCustomer.Phone,
            CreatedAt = domainCustomer.CreatedAt,
            UpdatedAt = domainCustomer.UpdatedAt,
            IsDeleted = domainCustomer.IsDeleted
        };
    }

    public static List<CustomerDto>? ToDto(this List<Customer>? domainCustomers)
    {
        if (domainCustomers is null)
        {
            return null;
        }

        return [.. domainCustomers.Select(c => c.ToDto())];
    }
}
