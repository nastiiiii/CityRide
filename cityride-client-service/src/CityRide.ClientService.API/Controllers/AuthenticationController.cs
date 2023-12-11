using CityRide.ClientService.API.Authentication.Requests;
using CityRide.ClientService.API.Authentication.Responses;
using CityRide.ClientService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CityRide.ClientService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IClientService clientService, IAuthenticationService authenticationService)
    {
        _clientService = clientService;
        _authenticationService = authenticationService;
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest logInRequest)
    {
        var clientDto = await _clientService.GetClientByEmailAndPassword(logInRequest.Email, logInRequest.Password);

        if (clientDto == null)
        {
            return Unauthorized();
        }

        var expirationPeriod = TimeSpan.FromMinutes(15);
        var tokenString = _authenticationService.GetTokenString(clientDto, expirationPeriod);
        var logInResponse = new LogInResponse { JWTToken = tokenString };

        return Ok(logInResponse);
    }
}