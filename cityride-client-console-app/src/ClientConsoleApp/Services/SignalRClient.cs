using Microsoft.AspNetCore.SignalR.Client;

using ClientConsoleApp.Models;

namespace ClientConsoleApp;

public class SignalRClient
{
    private readonly HubConnection _connection;

    public SignalRClient(HubConnection connection)
    {
        _connection = connection;
    }

    public async Task StartReceivingRideStatusMessages(Action<string> callback)
    {
        _connection.On<string>("ReceiveRideStatus", message =>
        {
            callback(message);
            Console.WriteLine(message);
        });

        _connection.StartAsync().GetAwaiter().GetResult();
        Console.WriteLine("Connected to SignalR hub.");
    }

    public async Task<decimal> SendRidePriceRequest(RidePriceRequest request)
    {
        await _connection.StartAsync();

        var ridePrice = await _connection.InvokeAsync<decimal>("CalculateRidePrice", arg1: request);

        await _connection.StopAsync();
        
        return ridePrice;
    }

    public async Task SendRideRequest(RideRequest request)
    {
        await _connection.StartAsync();

        await _connection.InvokeAsync("RequestRide", arg1: request);
        
        await _connection.StopAsync();
    }
}