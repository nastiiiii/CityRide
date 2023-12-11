using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityRide.DriverService.Application.Services.Interfaces;
using CityRide.DriverService.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CityRide.DriverService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetTokenString(DriverDto driverDto, TimeSpan expirationPeriod)
    {
        var encodedSecret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty);
        var secretKey = new SymmetricSecurityKey(encodedSecret);

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, driverDto.Id.ToString()),
            new(ClaimTypes.Email, driverDto.Email)
        };
        var tokenOptions = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            claims,
            expires: DateTime.Now.Add(expirationPeriod),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}