using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;
using CityRide.DriverService.Domain.Entities;

namespace CityRide.DriverService.Domain.Repositories;

public interface IDriverLocationRepository : IBaseRepository<DriverLocation>
{
    Task<DriverLocation?> GetDriverLocationByDriverIdAsync(int driverId);
    Task<List<DriverLocation>> GetDriversByLocationAndCarClassAsync(
        LocationDto locationFrom,
        CarClass carClass,
        double distanceInMeters,
        int maxDriversToRetrieve);
}