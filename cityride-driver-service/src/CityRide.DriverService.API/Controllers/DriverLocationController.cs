using AutoMapper;
using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;
using CityRide.DriverService.API.Authentication.Requests;
using CityRide.DriverService.API.DriverLocation.Responses;
using CityRide.DriverService.API.Filters;
using CityRide.DriverService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityRide.DriverService.API.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[ApiExceptionFilter]
public class DriverLocationController : ControllerBase
{
    private readonly IDriverLocationService _driverLocationService;
    private readonly IDriverService _driverService;
    private readonly IMapper _mapper;

    public DriverLocationController(
        IDriverLocationService driverLocationService,
        IDriverService driverService,
        IMapper mapper)
    {
        _driverLocationService = driverLocationService;
        _driverService = driverService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<DriverLocationResponse>> GetClosestDrivers(
        GetClosestDriversRequest getClosestDriversRequest)
    {
        var closestDrivers =
            await _driverLocationService.GetClosestDriversLocationsAsync(
                getClosestDriversRequest.Location,
                getClosestDriversRequest.CarClass,
                getClosestDriversRequest.DistanceInMeters,
                getClosestDriversRequest.NumberOfDriversToRetrieve);

        return Ok(_mapper.Map<List<DriverLocationResponse>>(closestDrivers));
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task UpdateDriverStatus(int driverId, DriverStatus status)
    {
        await _driverLocationService.UpdateDriverStatus(driverId, status);
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetDriverStatus()
    {
        var driverId = _driverService.CurrentDriverId;
        var driverStatus = await _driverLocationService.GetDriverStatus(driverId);

        return Ok(driverStatus.ToString());
    }

    [HttpPut]
    public async Task SetDriverStatus(DriverStatus status)
    {
        var driverId = _driverService.CurrentDriverId;
        await _driverLocationService.UpdateDriverStatus(driverId, status);
    }

    [HttpPut]
    public async Task UpdateDriverLocation(LocationDto location)
    {
        var driverId = _driverService.CurrentDriverId;
        await _driverLocationService.UpdateDriverLocation(driverId, location);
    }
}