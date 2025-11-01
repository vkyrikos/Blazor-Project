using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.ApiHost.Mapping;

public static class GetCustomerRequestExtensionMapping
{
    public static GetCustomerRequestModel ToDomain(this GetCustomerRequest request) => new()
    {
        CustomerId = request.CustomerId
    };
}
