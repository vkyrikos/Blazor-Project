namespace BlazorApp.Contracts.Api.Input;

public sealed class GetCustomersRequest
{
    public int PageNumber { get; init; }

    public bool GetActive { get; init; }
}
