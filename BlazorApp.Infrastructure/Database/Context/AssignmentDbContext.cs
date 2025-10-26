using BlazorApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure.Database.Context;

public class AssignmentDbContext : DbContext
{
    public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options): base(options)
    {
        
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
