using CityRide.Domain.Enums;

namespace CityRide.RideService.Domain.Dtos
{
    public class ClosestDriversRequestDto
    {
        public LocationDto Location { get; set; } = new();
        public CarClass CarClass { get; set; }
        public int DistanceInMeters { get; set; }
        public int NumberOfUsersToRetrieve { get; set; }
    }
}
