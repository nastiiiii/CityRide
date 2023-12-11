using CityRide.Domain.Enums;
using CityRide.Domain.Dtos;

namespace CityRide.DriverService.API.Authentication.Requests;

public class GetClosestDriversRequest
{
    public LocationDto Location { get; set; }
    public CarClass CarClass { get; set; }
    public double DistanceInMeters { get; set; }
    public int NumberOfDriversToRetrieve { get; set; } = 5;
}