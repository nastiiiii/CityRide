using CityRide.Domain.Enums;

namespace CityRide.RideService.Domain.Dtos;

public class RideDto
{
    public int Id { get; set; }
    public LocationDto From { get; set; } = new();
    public LocationDto To { get; set; } = new();
    public int ClientId { get; set; }
    public int? DriverId { get; set; } = null;

    public RideStatus Status { get; set; } = RideStatus.SearchingForDriver;
    public decimal Price { get; set; }
}