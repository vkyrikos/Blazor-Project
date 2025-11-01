using System.Text.Json.Serialization;

namespace BlazorApp.Models;

public class Customer
{
    public int Id { get; set; }              // not bound in the form, fine
    public string? CompanyName { get; set; }
    public string? ContactName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }  // keep as string for InputText
    public string? Country { get; set; }
    public string? Phone { get; set; }       // keep as string for InputText


    [JsonConstructor]
    public Customer()
    {

    }
}
