using CityRide.Domain.Enums;
using NetTopologySuite.Geometries;

namespace CityRide.DriverService.Domain.Entities;

public class DriverLocation
{
    public int Id { get; set; }
    public int DriverId { get; set; }
    public Geometry Location { get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Unavailable;
}