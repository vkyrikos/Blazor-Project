namespace BlazorApp.Domain.Models;

public class Customer
{
    public int Id { get; init; }
    public string CompanyName { get; init; }
    public string ContactName { get; init; }
    public string Address { get; init; }
    public string City { get; init; }
    public string Region { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }
    public string Phone { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get;init; }
    public bool IsDeleted { get; init; }
}
