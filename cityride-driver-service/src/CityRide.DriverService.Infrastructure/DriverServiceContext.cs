using CityRide.DriverService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CityRide.DriverService.Infrastructure;

public class DriverServiceContext : DbContext
{
    private readonly string _connectionString;

    public DriverServiceContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();
        optionsBuilder.UseNpgsql(dataSource, o => o.UseNetTopologySuite());
    }

    public DbSet<Driver> Drivers { get; set; }

    public DbSet<DriverLocation> DriverLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseSerialColumns();
        modelBuilder.Entity<DriverLocation>().Property(c => c.Location).HasColumnType("geography (point)")
            .HasSrid(4326);
        modelBuilder.Entity<Driver>().HasKey(l => l.Id);
        modelBuilder.Entity<DriverLocation>().HasKey(l => l.Id);
    }
}