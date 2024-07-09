using Microsoft.EntityFrameworkCore;
using minimal_api_pagination.Models;

namespace minimal_api_pagination.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasData(
        Enumerable.Range(1, 100).Select(i => new Customer
        {
            Id = i,
            Name = $"Peter Parker { i }",
            Email = $"peter.parker.{ i }@marvel.com"
        }).ToArray());
    }
}
