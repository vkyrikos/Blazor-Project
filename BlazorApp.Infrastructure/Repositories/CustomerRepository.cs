using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Domain.Models;
using BlazorApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AssignmentDbContext _context;

    public CustomerRepository(IDbContextFactory<AssignmentDbContext> factory)
    {
        _context = factory.CreateDbContext();
    }

    public Task<Customer> GetCustomerAsync(string customerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task InsertCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Add(customer);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
