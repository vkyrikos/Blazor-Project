using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Contracts.Api.Input;

public record GetCustomerRequest
{
    [FromRoute]
    [Required]
    public string Id { get; init; }
}
