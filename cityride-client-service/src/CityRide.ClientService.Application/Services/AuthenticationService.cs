using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using CityRide.ClientService.Domain.Dtos;

namespace CityRide.ClientService.Application.Services.Interfaces;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetTokenString(ClientDto clientDto, TimeSpan expirationPeriod)
    {
        var encodedSecret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty);
        var secretKey = new SymmetricSecurityKey(encodedSecret);

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, clientDto.ID.ToString()),
            new(ClaimTypes.Email, clientDto.Email)
        };
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.Add(expirationPeriod),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}