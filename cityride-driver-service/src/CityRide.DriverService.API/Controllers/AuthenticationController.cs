using CityRide.DriverService.API.Authentication.Requests;
using CityRide.DriverService.API.Authentication.Responses;
using CityRide.DriverService.API.Filters;
using CityRide.DriverService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CityRide.DriverService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExceptionFilter]
public class AuthenticationController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IDriverService driverService, IAuthenticationService authenticationService)
    {
        _driverService = driverService;
        _authenticationService = authenticationService;
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest logInRequest)
    {
        var driverDto = await _driverService.GetDriverByEmailAndPassword(logInRequest.Email, logInRequest.Password);

        if (driverDto == null) return Unauthorized();

        var expirationPeriod = TimeSpan.FromMinutes(15);
        var tokenString = _authenticationService.GetTokenString(driverDto, expirationPeriod);
        var logInResponse = new LogInResponse { JwtToken = tokenString };

        return Ok(logInResponse);
    }
}