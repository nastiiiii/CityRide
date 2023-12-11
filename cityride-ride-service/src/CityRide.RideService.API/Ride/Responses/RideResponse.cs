using CityRide.Domain.Enums;
using CityRide.RideService.Domain.Entities;

namespace CityRide.RideService.API.Ride.Responses;

public class RideResponse
{
    public int Id { get; set; }
    public Location From { get; set; } = new();
    public Location To { get; set; } = new();
    public int ClientId { get; set; } = default;
    public int DriverId { get; set; } = default;
    public RideStatus Status { get; set; } = default;
    public decimal Price { get; set; } = default;
}