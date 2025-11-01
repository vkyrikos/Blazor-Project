using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Contracts.Api.Input;

public sealed class GetCustomersRequest
{
    [FromRoute]
    public int PageNumber { get; init; }
    [FromQuery]
    public bool IncludeDeleted { get; init; }
}
