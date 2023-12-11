using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CityRide.ClientService.Domain.Entities;

namespace CityRide.ClientService.Infrastructure
{
    public class ClientServiceContext:DbContext
    {
        private readonly string _connectionString;
        public ClientServiceContext(IConfiguration configuration) { 
            _connectionString = configuration.GetConnectionString("ClientServiceContextDb") ?? string.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
           
        }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().HasKey(l => l.ID);
        }

    }
}
