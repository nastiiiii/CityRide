namespace CityRide.ClientService.Application.Services.Interfaces
{
    public interface IClientAppClient
    {
        Task ReceiveRideStatus(string message);
    }
}
