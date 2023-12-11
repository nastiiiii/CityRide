using CityRide.RideService.Domain.Entities;

namespace CityRide.RideService.Domain.Repositories;

public interface IRideRepository : IBaseRepository<Ride>
{
    Task<IEnumerable<Ride>> GetRidesByClientIdAsync(int clientId);
    Task<IEnumerable<Ride>> GetRidesByDriverIdAsync(int driverId);
}