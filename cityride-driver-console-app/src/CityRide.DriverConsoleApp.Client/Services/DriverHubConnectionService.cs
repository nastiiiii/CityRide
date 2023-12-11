using Microsoft.AspNetCore.SignalR.Client;

using CityRide.DriverConsoleApp.Client.Requests;
using CityRide.DriverConsoleApp.Client.Services.Interfaces;
using CityRide.DriverConsoleApp.Client.Responses;

namespace CityRide.DriverConsoleApp.Client.Services;

public class DriverHubConnectionService : IDriverHubConnectionService
{
    private readonly HubConnection _hubConnection;

    public DriverHubConnectionService(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public async Task StartConnection()
    {
        await _hubConnection.StartAsync();
    }

    public async Task StopConnection()
    {
        await _hubConnection.StopAsync();
    }

    public void AddOnReceiveRideRequestHandler(Action<RideRequestResponse> handler)
    {
        _hubConnection.On("ReceiveRideRequest", handler);
    }

    public async Task SendAcceptRideRequest(int rideId)
    {
        await _hubConnection.SendAsync("AcceptRideRequest", rideId);
    }

    public async Task SendDeclineRideRequest(int rideId)
    {
        await _hubConnection.SendAsync("DeclineRideRequest", rideId);
    }

    public async Task SendStartRideRequest(int rideId)
    {
        await _hubConnection.SendAsync("StartRide", rideId);
    }

    public async Task SendStopRideRequest(int rideId)
    {
        await _hubConnection.SendAsync("EndRide", rideId);
    }
}