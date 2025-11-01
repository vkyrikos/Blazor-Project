using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.ApiHost.Mapping.RequestMapping;

public static class DeleteCustomerRequestExtensionMapping
{
    public static DeleteCustomerRequestModel ToDomain(this DeleteCustomerRequest request) => new()
    {
        CustromerId = request.CustomerId
    };
}
