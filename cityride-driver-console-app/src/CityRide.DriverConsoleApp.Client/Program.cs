using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR.Client;
using CityRide.DriverConsoleApp.Client.Requests;
using CityRide.DriverConsoleApp.Client.Services;
using CityRide.DriverConsoleApp.Client.Services.Interfaces;

namespace CityRide.DriverConsoleApp.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        if (args.Length < 2)
        {
            Console.WriteLine("Email and password are required.");
            return;
        }

        var loginRequest = new LoginRequest { Email = args[0], Password = args[1] };

        var tokenService = new TokenService(configuration);
        string jwtToken = await tokenService.GetTokenAsync(loginRequest);

        if(string.IsNullOrEmpty(jwtToken))
        {
            Console.WriteLine("Authorization error.");
            return;
        }

        var hubUrl = configuration["HubUrl"];

        var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options 
            => options.AccessTokenProvider = () => Task.FromResult(jwtToken))
            .WithAutomaticReconnect()
        .Build();

        IDriverHubConnectionService driverHubConnectionService = new DriverHubConnectionService(connection);
        IRideRequestService rideRequestService = new RideRequestService(driverHubConnectionService);

        try
        {
            await rideRequestService.StartRideRequestListener();

            do
            {
                Console.WriteLine("\nListening for ride requests..");
                rideRequestService.ProcessRideRequest();
                Console.Write("\nPress space to exit or any other key to continue receiving rides: ");
            } while (Console.ReadKey().Key != ConsoleKey.Spacebar);

            await rideRequestService.StopRideRequestListener();
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection failed.");
            Console.WriteLine(e.Message);
            // TODO: log exception
        }
    }
}