using AutoMapper;
using CityRide.DriverService.API.Driver.Requests;
using CityRide.DriverService.API.Driver.Responses;
using CityRide.DriverService.API.Filters;
using CityRide.DriverService.Application.Services.Interfaces;
using CityRide.DriverService.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CityRide.DriverService.API.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[ApiExceptionFilter]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly IMapper _mapper;

    public DriverController(IDriverService driverService, IMapper mapper)
    {
        _driverService = driverService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<DriverProfileResponse>> CreateDriver(CreateDriverRequest request)
    {
        var createdDriver = await _driverService.CreateDriverAsync(_mapper.Map<DriverDto>(request));

        return Ok(_mapper.Map<DriverProfileResponse>(createdDriver));
    }

    [HttpGet]
    public async Task<ActionResult<DriverProfileResponse>> GetDriverProfile(int driverId)
    {
        if (driverId != _driverService.CurrentDriverId)
        {
            return Unauthorized();
        }

        var driver = await _driverService.GetDriverByIdAsync(driverId);

        return Ok(_mapper.Map<DriverProfileResponse>(driver));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateDriver(UpdateDriverRequest request)
    {
        if (request.Id != _driverService.CurrentDriverId)
        {
            return Unauthorized();
        }

        var driverDto = _mapper.Map<DriverDto>(request);

        await _driverService.UpdateDriverAsync(driverDto);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteDriver(int driverId)
    {
        if (driverId != _driverService.CurrentDriverId)
        {
            return Unauthorized();
        }

        await _driverService.DeleteDriverAsync(driverId);

        return NoContent();
    }
}