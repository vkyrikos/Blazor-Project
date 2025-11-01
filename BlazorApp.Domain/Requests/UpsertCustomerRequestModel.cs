using BlazorApp.Domain.Models;

namespace BlazorApp.Domain.Requests;

public class UpsertCustomerRequestModel
{
    public Customer Customer { get; init; }
}
