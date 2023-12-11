using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;
using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.Application.Services.Interfaces;

public interface IDriverLocationService
{
    Task<List<DriverLocationDto>> GetClosestDriversLocationsAsync(LocationDto locationFrom, CarClass carClass,
        double distanceInMeters, int numberOfUsersToRetrieve);

    Task<DriverStatus> GetDriverStatus(int driverId);
    
    Task UpdateDriverStatus(int driverId, DriverStatus status);
    Task SetDriverStatus(int driverId, DriverStatus status);
    Task UpdateDriverLocation(int driverId, LocationDto location);
    Task<DriverLocationDto> GetDriverLocationById(int driverId);
}