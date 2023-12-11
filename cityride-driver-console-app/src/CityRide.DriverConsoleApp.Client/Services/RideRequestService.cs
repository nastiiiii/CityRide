using System.Timers;

using CityRide.DriverConsoleApp.Client.Requests;
using CityRide.DriverConsoleApp.Client.Responses;
using CityRide.DriverConsoleApp.Client.Services.Interfaces;

namespace CityRide.DriverConsoleApp.Client.Services;

public class RideRequestService : IRideRequestService
{
    private const int TimeToRespond = 10000;
    
    private readonly IDriverHubConnectionService _driverHubConnectionService;
    private bool _isTimeElapsed;
    private bool _isDecisionMade;
    private int _currentRideId;

    public RideRequestService(IDriverHubConnectionService driverHubConnectionService)
    {
        _driverHubConnectionService = driverHubConnectionService;
    }

    public async Task StartRideRequestListener()
    {
        await _driverHubConnectionService.StartConnection();
    }

    public async Task StopRideRequestListener()
    {
        await _driverHubConnectionService.StopConnection();
    }

    public void ProcessRideRequest()
    {
        async void Handler(RideRequestResponse r)
        {
            _isTimeElapsed = false;
            _isDecisionMade = false;
            _currentRideId = r.Id;
            
            // Set up a timer
            var timer = new System.Timers.Timer(TimeToRespond);
            timer.Elapsed += OnTimeElapsed;
            timer.AutoReset = false;
            timer.Start();

            // Display ride request
            Console.WriteLine($"New ride request: {r.From.Latitude}, {r.From.Longitude} -> {r.To.Latitude}, {r.To.Longitude}");

            // Get driver's decision
            bool? isRideAccepted = null;
            do
            {
                Console.Write("\nPress 1 to accept ride or 2 to decline it: ");
                var key = Console.ReadKey();
                if(key.Key == ConsoleKey.D1)
                {
                    isRideAccepted = true;
                }
                else
                {
                    isRideAccepted = false;
                }
            } while (isRideAccepted == null && !_isTimeElapsed);

            _isDecisionMade = true;

            timer.Close();
            if (_isTimeElapsed)
            {
                return;
            }

            // If ride was declined
            if (!isRideAccepted!.Value)
            {
                await _driverHubConnectionService.SendDeclineRideRequest(r.Id);
                Console.WriteLine("\nRide declined");
                return;
            }

            // If ride was accepted
            await _driverHubConnectionService.SendAcceptRideRequest(r.Id);
            Console.WriteLine("\nRide accepted");
            
            // Start ride
            ConsoleKey? consoleKey;
            do
            {
                Console.Write("\nPress 1 to start the ride: ");
                consoleKey = Console.ReadKey().Key;
            } while (consoleKey != ConsoleKey.D1);
            await _driverHubConnectionService.SendStartRideRequest(r.Id);
            Console.WriteLine("\nThe ride started");

            // End ride
            do
            {
                Console.Write("\nPress 1 to end the ride: ");
                consoleKey = Console.ReadKey().Key;
            } while (consoleKey != ConsoleKey.D1);
            await _driverHubConnectionService.SendStopRideRequest(r.Id);
            Console.WriteLine("\nThe ride ended");
        }
        
        _driverHubConnectionService.AddOnReceiveRideRequestHandler(Handler);
    }

    private async void OnTimeElapsed(object? source, ElapsedEventArgs e)
    {
        if (!_isDecisionMade)
        {
            Console.Write("\nYour time to decide has ran out. Input 1 to continue: ");
            
            // Decline the ride
            await _driverHubConnectionService.SendDeclineRideRequest(_currentRideId);
        }

        _isTimeElapsed = true;
    }
}