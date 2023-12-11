using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

using ClientConsoleApp.Services.Interfaces;

namespace ClientConsoleApp.Services;

public class ClientRideInputService : IClientRideInputService
{
    private readonly IConsoleReadService _consoleReadService;

    public ClientRideInputService(IConsoleReadService consoleReadService)
    {
        _consoleReadService = consoleReadService;
    }
    
    public LocationDto GetLocationInput(string coordinateName = "")
    {
        Console.WriteLine($"Please enter {coordinateName} location.");
        
        // Latitude
        double? latitude;
        do
        {
            Console.Write("Latitude: ");
            latitude = _consoleReadService.ReadDouble();
        } while (latitude == null);
        Console.WriteLine($"Latitude: {latitude}");
        
        // Longitude
        double? longitude;
        do
        {
            Console.Write("Longitude: ");
            longitude = _consoleReadService.ReadDouble();
        } while (longitude == null);
        Console.WriteLine($"Longitude: {longitude}");

        return new LocationDto { Latitude = latitude.Value, Longitude = longitude.Value };
    }

    public CarClass GetCarClassInput()
    {
        CarClass? carClass = _consoleReadService.ReadCarClass();
        while (carClass == null)
        {
            carClass = _consoleReadService.ReadCarClass();
        }
        Console.WriteLine($"Selected car class: {carClass}");

        return carClass.Value;
    }

    public string GetRideDecisionInput(decimal ridePrice, string acceptRideOption, string declineRideOption)
    {
        Console.Write($"Ride price: {ridePrice}. Enter \"{acceptRideOption}\" to accept the ride or \"{declineRideOption}\" to decline it: ");
        string? input = Console.ReadLine();
        while (input != acceptRideOption && input != declineRideOption)
        {
            Console.WriteLine($"\"{input}\" is not an available option, please try again.");
            Console.Write($"Ride price: {ridePrice}. Enter \"{acceptRideOption}\" to accept the ride or \"{declineRideOption}\" to decline it: ");
            input = Console.ReadLine();
        }

        return input;
    }
}