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

    public async Task<int> InsertCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await using var db =  await _factory.CreateDbContextAsync(cancellationToken);

        db.Add(customer);

        var affected = await db.SaveChangesAsync(cancellationToken);

        return affected;
    }

    public async Task<int> UpdateCustomerAsync(int customerId, Customer customer, CancellationToken cancellationToken = default)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        customer.UpdatedAt = DateTime.UtcNow;

        db.Update(customer);

        var affected = await db.SaveChangesAsync(cancellationToken);

        return affected;
    }

    public async Task<Customer?> GetCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id should be greater or equal to 1.", nameof(customerId));
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Customers
                       .AsNoTracking()
                       .SingleOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public async Task<List<Customer>> GetCustomersAsync(int pageId, CancellationToken cancellationToken = default)
    {
        if (pageId <= 0)
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

    public async Task<int> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id is required.", nameof(customerId));
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        var affected = await db.Customers
                                     .Where(c => c.Id == customerId)
                                     .ExecuteDeleteAsync(cancellationToken);

        return affected;
    }
}
