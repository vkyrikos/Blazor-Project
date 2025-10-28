namespace BlazorApp.Domain.Models;

public class Customer
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    // Προσθήκη audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get;set; }
    public bool IsDelete { get; set; }
}
