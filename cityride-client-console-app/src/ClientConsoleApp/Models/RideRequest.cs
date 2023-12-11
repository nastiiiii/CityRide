using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

namespace ClientConsoleApp.Models;

public class RideRequest
{
    public LocationDto Source { get; set; } = null!;
    public LocationDto Destination { get; set; } = null!;
    public CarClass CarClass { get; set; }
    public decimal Price { get; set; }
}