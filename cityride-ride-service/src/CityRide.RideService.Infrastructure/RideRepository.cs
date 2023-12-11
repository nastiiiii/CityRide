using Microsoft.EntityFrameworkCore;
using CityRide.RideService.Domain.Entities;
using CityRide.RideService.Domain.Repositories;

namespace CityRide.RideService.Infrastructure;

public class RideRepository : BaseRepository<Ride>, IRideRepository
{
    public RideRepository(RideServiceContext appContext) : base(appContext)
    {
    }

    public async Task<IEnumerable<Ride>> GetRidesByClientIdAsync(int clientId)
    {
        return await _context.Rides.Where(x => x.ClientId == clientId).ToListAsync();
    }

    public async Task<IEnumerable<Ride>> GetRidesByDriverIdAsync(int driverId)
    {
        return await _context.Rides.Where(x => x.DriverId == driverId).ToListAsync();
    }
}