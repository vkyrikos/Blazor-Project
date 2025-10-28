using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Domain.Models;
using BlazorApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbContextFactory<AssignmentDbContext> _factory;
    private const int PageSize = 10;

    public CustomerRepository(IDbContextFactory<AssignmentDbContext> factory)
    {
        _factory = factory;
    }

    public async Task InsertCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var db = await _factory.CreateDbContextAsync(cancellationToken);

        db.Add(customer);

        await db.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateCustomerAsync(string customerId, Customer customer, CancellationToken cancellationToken = default)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        customer.UpdatedAt = DateTime.UtcNow;

        db.Update(customer);

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Customer?> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentException("Customer id is required.", nameof(customerId));
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Customers
                       .AsNoTracking()
                       .SingleOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public async Task<List<Customer>> GetCustomersAsync(int pageId, CancellationToken cancellationToken = default)
    {
        if (pageId < 1)
        { 
            throw new ArgumentOutOfRangeException(nameof(pageId), "pageId must be >= 1."); 
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Customers
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

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);
        
        var affected = await db.Customers
                                     .Where(c => c.Id == customerId)
                                     .ExecuteDeleteAsync(cancellationToken);

        if (affected == 0)
        {
            throw new KeyNotFoundException($"Customer '{customerId}' was not found.");
        }
    }
}
