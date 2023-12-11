using CityRide.RideService.Domain.Dtos;

namespace CityRide.RideService.Application.Services.Interfaces
{
    public interface IRideClient
    {
        Task ReceiveRideRequest(ReceiveRideRequestDto receiveRideRequestDto);
    }
}
