using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Contracts.Api.Input;

public sealed class DeleteCustomerRequest
{
    public int CustomerId { get; init; }
}
