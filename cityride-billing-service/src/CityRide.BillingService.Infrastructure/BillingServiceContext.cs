using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using CityRide.BillingService.Domain.Entities;

namespace CityRide.BillingService.Infrastructure;

public class BillingServiceContext : DbContext
{
    private readonly string _connectionString;

    public BillingServiceContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BillingServiceContextDb") ?? string.Empty;
    }
    
    public DbSet<RidePrice> RidePrices { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RidePrice>().HasKey(r => r.Id);
    }
}