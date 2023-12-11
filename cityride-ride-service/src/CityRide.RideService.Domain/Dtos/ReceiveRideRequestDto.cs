using CityRide.RideService.Domain.Entities;

namespace CityRide.RideService.Domain.Dtos
{
    public class ReceiveRideRequestDto
    {
        public int Id { get; set; }
        public Location From { get; set; } = new();
        public Location To { get; set; } = new();
        public int ClientId { get; set; }
        public decimal Price { get; set; }
    }
}
