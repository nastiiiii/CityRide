using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

namespace ClientConsoleApp.Services.Interfaces;

public interface IClientRideInputService
{
    LocationDto GetLocationInput(string coordinateName);
    CarClass GetCarClassInput();
    string GetRideDecisionInput(decimal ridePrice, string acceptRideOption, string declineRideOption);
}