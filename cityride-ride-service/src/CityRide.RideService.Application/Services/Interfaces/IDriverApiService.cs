using CityRide.Domain.Enums;
using CityRide.RideService.Domain.Dtos;

namespace CityRide.RideService.Application.Services.Interfaces
{
    public interface IDriverApiService
    {
        Task<List<ClosestDriverDto>> GetClosestDriversAsync(ClosestDriversRequestDto requestClosestDriversDto);
        Task UpdateDriverStatusAsync(int driverId, DriverStatus driverStatus);
    }
}
