using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CityRide.RideService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace CityRide.RideService.Infrastructure;

public class RideServiceContext : DbContext
{
    public RideServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    public DbSet<Ride> Rides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ride>(ride =>
        {
            ride.HasKey(l => l.RideId);
            ride.OwnsOne(r => r.From);
            ride.OwnsOne(r => r.To);
        });
    }
}