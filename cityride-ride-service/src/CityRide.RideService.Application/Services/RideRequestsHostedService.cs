using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CityRide.RideService.Application.Services.Interfaces;

namespace CityRide.RideService.Application.Services
{
    public class RideRequestsHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public RideRequestsHostedService(IServiceProvider services)
        {
            _services = services;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                using (var scope = _services.CreateScope())
                {
                    var rideRequestsService = scope.ServiceProvider.GetRequiredService<IRideRequestsService>();
                    rideRequestsService.ExecuteAsync(stoppingToken);
                }
            });
        }
    }
}
