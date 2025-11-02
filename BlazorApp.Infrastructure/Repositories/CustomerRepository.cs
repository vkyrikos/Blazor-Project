using BlazorApp.Application.Interfaces.Repositories.Customer;
using BlazorApp.Domain.Models;
using BlazorApp.Domain.Requests;
using BlazorApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure.Repositories;

public class CustomerRepository(IDbContextFactory<AssignmentDbContext> factory) : ICustomerRepository
{
    private readonly IDbContextFactory<AssignmentDbContext> _factory = factory;
    private const int PageSize = 10;

    public async Task<Customer> InsertCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        db.Customers.Add(customer);
        await db.SaveChangesAsync(cancellationToken);

        return customer;
    }

    public async Task<Customer?> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        var updatedAtValue = DateTime.UtcNow;

        var affected = await db.Customers.
            Where(c => c.Id == customer.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(c => c.CompanyName, _ => customer.CompanyName)
            .SetProperty(c => c.ContactName, _ => customer.ContactName)
            .SetProperty(c => c.Address, _ => customer.Address)
            .SetProperty(c => c.City, _ => customer.City)
            .SetProperty(c => c.Region, _ => customer.Region)
            .SetProperty(c => c.PostalCode, _ => customer.PostalCode)
            .SetProperty(c => c.Country, _ => customer.Country)
            .SetProperty(c => c.Phone, _ => customer.Phone)
            .SetProperty(c => c.UpdatedAt, _ => updatedAtValue),
            cancellationToken);

        if (affected == 0)
        {
            return null;
        }

        var updated = await db.Customers
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == customer.Id, cancellationToken);

        return updated;
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

    public async Task<List<Customer>> GetCustomersAsync(GetCustomersRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.PageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.PageNumber), "pageNumber must be >= 1.");
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);
        var query = db.Customers.AsQueryable();

        if (!request.IncludeDeleted)
        {
            query = query.Where(c => !c.IsDeleted);
        }

        return await query
                       .AsNoTracking()
                       .OrderBy(c => c.Id)
                       .Skip((request.PageNumber - 1) * PageSize)
                       .Take(PageSize)
                       .ToListAsync(cancellationToken);
    }

    public async Task<Customer> DeleteCustomerAsync(DeleteCustomerRequestModel request, CancellationToken cancellationToken = default)
    {
        if (request.CustomerId <= 0)
        { 
            throw new ArgumentOutOfRangeException(nameof(request.CustomerId)); 
        }

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        var now = DateTime.UtcNow;

        var rows = await db.Customers
            .Where(c => c.Id == request.CustomerId && !c.IsDeleted)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.IsDeleted, true)
                .SetProperty(c => c.UpdatedAt, now), cancellationToken);

        if (rows == 0)
        {
            return null;
        }

        var deleted = await db.Customers
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

        return deleted;
    }
}
