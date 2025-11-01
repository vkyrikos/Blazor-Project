namespace BlazorApp.Infrastructure;

public sealed class CustomerConfiguration
{
    public string BaseUrl { get; init; }
    public string UpsertCustomerUrl { get; init; }
    public string GetCustomerUrl { get; init; }
    public string GetCustomersUrl { get; init; }
    public string DeleteCustomerUrl { get; init; }
}
