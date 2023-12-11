using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;
using CityRide.DriverService.Domain.Entities;
using CityRide.DriverService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace CityRide.DriverService.Infrastructure;

public class DriverLocationRepository : BaseRepository<DriverLocation>, IDriverLocationRepository
{
    public DriverLocationRepository(DriverServiceContext appContext) : base(appContext)
    {
    }
    public async Task<DriverLocation?> GetDriverLocationByDriverIdAsync(int driverId)
    {
        return await _context.DriverLocations
            .Where(dl => dl.DriverId == driverId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<DriverLocation>> GetDriversByLocationAndCarClassAsync(
        LocationDto locationFrom,
        CarClass carClass,
        double distanceInMeters,
        int maxDriversToRetrieve)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
        var targetLocation = geometryFactory.CreatePoint(new Coordinate(locationFrom.Longitude, locationFrom.Latitude));

        var closestDrivers = await _context.DriverLocations
            .Where(l => l.Status == DriverStatus.Available)
            .ToListAsync();

        foreach(var driver in closestDrivers)
        {
            if(!driver.Location.IsWithinDistance(targetLocation, distanceInMeters))
            {
                closestDrivers.Remove(driver);
            }
        }

        closestDrivers = closestDrivers
            .OrderBy(l => CalculateDistance(l.Location, targetLocation))
            .Take(maxDriversToRetrieve)
            .ToList();

        return closestDrivers;
    }

    public double CalculateDistance(Geometry location, Point targetLocation)
    {
        return location.Distance(targetLocation);
    }
}