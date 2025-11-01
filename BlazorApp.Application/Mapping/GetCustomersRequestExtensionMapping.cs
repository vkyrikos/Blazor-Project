using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Mapping;

public static class GetCustomersRequestExtensionMapping
{
    public static GetCustomersRequestModel ToDomain(this GetCustomersRequest request) => new()
    {
        IncludeDeleted = request.IncludeDeleted,
        PageNumber = request.PageNumber,
    };
}
