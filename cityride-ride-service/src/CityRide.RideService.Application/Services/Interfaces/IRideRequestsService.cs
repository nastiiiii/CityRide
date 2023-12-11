namespace CityRide.RideService.Application.Services.Interfaces
{
    public interface IRideRequestsService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
