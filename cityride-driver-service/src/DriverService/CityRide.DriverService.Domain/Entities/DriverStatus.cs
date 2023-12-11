using CityRide.DriverService.Domain.Enums;

namespace CityRide.DriverService.Domain.Entities;

public class DriverStatus
{
    public int Id { get; set; }
    public int Location { get; set; }
    public int Status { get; set; }
}