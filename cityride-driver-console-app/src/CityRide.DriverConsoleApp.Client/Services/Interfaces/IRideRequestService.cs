namespace CityRide.DriverConsoleApp.Client.Services.Interfaces;

public interface IRideRequestService
{
    Task StartRideRequestListener();
    Task StopRideRequestListener();
    void ProcessRideRequest();
}