using CityRide.DriverConsoleApp.Client.Requests;
using CityRide.DriverConsoleApp.Client.Responses;

namespace CityRide.DriverConsoleApp.Client.Services.Interfaces;

public interface IDriverHubConnectionService
{
    Task StartConnection();
    Task StopConnection();
    void AddOnReceiveRideRequestHandler(Action<RideRequestResponse> handler);
    
    // Should these requests have bodies?
    Task SendAcceptRideRequest(int rideId);
    Task SendDeclineRideRequest(int rideId);
    Task SendStartRideRequest(int rideId);
    Task SendStopRideRequest(int rideId);
}