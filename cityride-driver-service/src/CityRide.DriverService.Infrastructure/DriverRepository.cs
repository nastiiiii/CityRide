using CityRide.DriverService.Domain.Entities;
using CityRide.DriverService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CityRide.DriverService.Infrastructure;

public class DriverRepository : BaseRepository<Driver>, IDriverRepository
{
    public DriverRepository(DriverServiceContext appContext) : base(appContext)
    {
    }

    public async Task<Driver> GetDriverByEmailAndPasswordHashAsync(string email, string passwordHash)
    {
        return await _context.Drivers.Where(c => c.Email == email && c.Password == passwordHash).FirstOrDefaultAsync();
    }
}