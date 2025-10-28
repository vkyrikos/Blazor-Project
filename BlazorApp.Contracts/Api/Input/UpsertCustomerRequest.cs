using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Contracts.Api.Input
{
    public record UpsertCustomerRequest
    {
        public int? Id { get; set; }

        [Required]
        public string CompanyName { get; init; } = string.Empty;

        [Required]
        public string ContactName { get; init; } = string.Empty;

        [Required]
        public string Address { get; init; } = string.Empty;

        [Required]
        public string City { get; init; } = string.Empty;

        [Required]
        public string Region { get; init; } = string.Empty;

        [Required]
        public string PostalCode { get; init; } = string.Empty;

        [Required]
        public string Country { get; init; } = string.Empty;

        [Required]
        public string Phone { get; init; } = string.Empty;
    }
}
