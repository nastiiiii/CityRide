namespace CityRide.RideService.Application.Services.Interfaces
{
    public interface IRedisClientService
    {
        Task SetClosestDriversListAsync(string rideId, IEnumerable<string> closestDrivers);
        Task<bool> DeleteKeyIfDriverInListAsync(string rideId, string driverId);
        Task<bool> DeleteElementFromListAsync(string rideId, string driverId);
    }
}
