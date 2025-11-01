using BlazorApp.Contracts.Api.Input;
using BlazorApp.Domain.Requests;

namespace BlazorApp.Application.Mapping;

public static class GetCustomersRequestExtensionMapping
{
    public static GetCustomersRequestModel ToDomainGetCustomersRequest(this GetCustomersRequest request) => new()
    {
        IncludeDeleted = request.IncludeDeleted,
        PageNumber = request.PageNumber,
    };
}
