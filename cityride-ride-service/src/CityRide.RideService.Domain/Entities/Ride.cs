using CityRide.Domain.Enums;


namespace CityRide.RideService.Domain.Entities;

public class Ride
{
    public int RideId { get; set; }
    public Location From { get; set; } = new();
    public Location To { get; set; } = new();
    public int ClientId { get; set; }
    public int? DriverId { get; set; } = null;
    public RideStatus Status { get; set; } = RideStatus.SearchingForDriver;
    public decimal Price { get; set; }
}