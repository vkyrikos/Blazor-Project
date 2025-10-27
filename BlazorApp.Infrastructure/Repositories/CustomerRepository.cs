using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Domain.Models;
using BlazorApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AssignmentDbContext _context;
    private const int PageSize = 10;

    public CustomerRepository(IDbContextFactory<AssignmentDbContext> factory)
    {
        _context = factory.CreateDbContext();
    }

    public async Task InsertCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Add(customer);

        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateCustomerAsync(string customerId, Customer customer, CancellationToken cancellationToken = default)
    {
        customer.UpdatedAt = DateTime.UtcNow;

        _context.Update(customer);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<Customer?> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentException("Customer id is required.", nameof(customerId));
        }

        return _context.Customers
                       .AsNoTracking()
                       .SingleOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public Task<List<Customer>> GetCustomersAsync(int pageId, CancellationToken cancellationToken = default)
    {
        if (pageId < 1)
        { 
            throw new ArgumentOutOfRangeException(nameof(pageId), "pageId must be >= 1."); 
        }

        return _context.Customers
                       .AsNoTracking()
                       .OrderBy(c => c.Id)
                       .Skip((pageId - 1) * PageSize)
                       .Take(PageSize)
                       .ToListAsync(cancellationToken);
    }

    public async Task DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentException("Customer id is required.", nameof(customerId));
        }

        var affected = await _context.Customers
                                     .Where(c => c.Id == customerId)
                                     .ExecuteDeleteAsync(cancellationToken);

        if (affected == 0)
        {
            throw new KeyNotFoundException($"Customer '{customerId}' was not found.");
        }
    }
}
