using AutoMapper;
using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;
using CityRide.DriverService.Application.Exceptions.DriverLocationExceptions;
using CityRide.DriverService.Application.Services.Interfaces;
using CityRide.DriverService.Domain.Dtos;
using CityRide.DriverService.Domain.Repositories;
using NetTopologySuite.Geometries;

namespace CityRide.DriverService.Application.Services;

public class DriverLocationService : IDriverLocationService
{
    private readonly IDriverLocationRepository _driverLocationRepository;
    private readonly IMapper _mapper;

    public DriverLocationService(IDriverLocationRepository driverLocationRepository, IMapper mapper)
    {
        _driverLocationRepository = driverLocationRepository;
        _mapper = mapper;
    }

    public async Task<DriverLocationDto> GetDriverLocationById(int driverId)
    {
        var driverLocation = await _driverLocationRepository.GetDriverLocationByDriverIdAsync(driverId);
        if (driverLocation == null)
        {
            throw new DriverLocationNotFoundException(driverId);
        }

        return _mapper.Map<DriverLocationDto>(driverLocation);
    }

    public async Task<List<DriverLocationDto>> GetClosestDriversLocationsAsync(LocationDto locationFrom,
        CarClass carClass, double distanceInMeters,
        int numberOfDriversToRetrieve)
    {
        var closestDrivers = await _driverLocationRepository.GetDriversByLocationAndCarClassAsync(
            locationFrom,
            carClass,
            distanceInMeters,
            numberOfDriversToRetrieve);

        return _mapper.Map<List<DriverLocationDto>>(closestDrivers);
    }

    public async Task<DriverStatus> GetDriverStatus(int driverId)
    {
        var driverLocation = await _driverLocationRepository.GetDriverLocationByDriverIdAsync(driverId);

        if (driverLocation == null)
        {
            throw new DriverLocationNotFoundException(driverId);
        }

        return driverLocation.Status;
    }
    
    public async Task SetDriverStatus(int driverId, DriverStatus status)
    {
        var driverLocation = await _driverLocationRepository.GetDriverLocationByDriverIdAsync(driverId);

        if (driverLocation == null)
        {
            throw new DriverLocationNotFoundException(driverId);
        }

        driverLocation.Status = status;

        await _driverLocationRepository.UpdateAsync(driverLocation);
    }
    
    public async Task UpdateDriverStatus(int driverId, DriverStatus status)
    {
        await SetDriverStatus(driverId, status);
    }
    
    public async Task UpdateDriverLocation(int driverId, LocationDto location)
    {
        var driverLocation = await _driverLocationRepository.GetDriverLocationByDriverIdAsync(driverId);

        if (driverLocation == null)
        {
            throw new DriverLocationNotFoundException(driverId);
        }

        driverLocation.Location = new Point(location.Latitude, location.Longitude);

        await _driverLocationRepository.UpdateAsync(driverLocation);
    }
}