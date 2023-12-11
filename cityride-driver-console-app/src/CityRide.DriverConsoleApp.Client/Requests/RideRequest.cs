using CityRide.Domain.Dtos;

namespace CityRide.DriverConsoleApp.Client.Requests;

public class RideRequest
{
    public LocationDto Source { get; set; }
    public LocationDto Destination { get; set; }
    // public decimal Profit { get; set; } 
}