using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

namespace CityRide.DriverService.API.DriverLocation.Responses
{
    public class DriverLocationResponse
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public LocationDto Location { get; set; }

        public DriverStatus Status { get; set; } = DriverStatus.Unavailable;
    }
}
