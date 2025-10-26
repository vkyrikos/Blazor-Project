using BlazorApp.Application.Interfaces.Repositories;
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

    public Task InsertCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
