namespace BlazorApp.Domain.Requests;

public sealed class GetCustomersRequestModel
{
    public int PageNumber { get; init; }

    public bool IncludeDeleted { get; init; }
}
