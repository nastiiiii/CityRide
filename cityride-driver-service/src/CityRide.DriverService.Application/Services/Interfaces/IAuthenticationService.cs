using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.Application.Services.Interfaces;

public interface IAuthenticationService
{
    string GetTokenString(DriverDto driverDto, TimeSpan expirationPeriod);
}