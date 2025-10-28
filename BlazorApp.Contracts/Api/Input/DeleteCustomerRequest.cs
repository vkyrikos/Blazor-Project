using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Contracts.Api.Input;

public sealed class DeleteCustomerRequest
{
    [FromRoute]
    public int Id { get; init; }
}
