using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

namespace CityRide.ClientService.Domain.Dtos
{
    public class RideRequestDto
    {
        public LocationDto Source { get; set; } = null!;
        public LocationDto Destination { get; set; } = null!;
        public CarClass CarClass { get; set; }
        public decimal Price { get; set; }
    }
}
