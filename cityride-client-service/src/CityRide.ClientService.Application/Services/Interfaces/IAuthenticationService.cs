using CityRide.ClientService.Domain.Dtos;

namespace CityRide.ClientService.Application.Services.Interfaces;

public interface IAuthenticationService
{
    string GetTokenString(ClientDto clientDto, TimeSpan expirationPeriod);
}