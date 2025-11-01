using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.ApiHost.Mapping.RequestMapping;

public static class GetCustomerRequestExtensionMapping
{
    public static GetCustomerRequestModel ToDomain(this GetCustomerRequest request) => new()
    {
        CustomerId = request.CustomerId
    };
}
