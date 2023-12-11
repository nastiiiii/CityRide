using AutoMapper;
using ClientConsoleApp.Models;
using ClientConsoleApp.Services;
using ClientConsoleApp.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClientConsoleApp;

internal class Program
{
    private const string HelpOption = "h";
    private const string AcceptRideOption = "a";
    private const string DeclineRideOption = "d";

    private static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        if (args.Length < 2)
        {
            Console.WriteLine("Email and password are required.");
            return;
        }

        var client = new LogInCredentials { Email = args[0], Password = args[1] };
        
        var tokenService = new TokenService(config);
        var token = await tokenService.GetTokenAsync(client);

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Failed to retrieve the JWT token from the API.");
            return;
        }
        
        var hubUrl = config["HubUrl"];
        
        var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options => { options.AccessTokenProvider = () => Task.FromResult(token); })
            .WithAutomaticReconnect()
            .Build();
        
        var signalRClient = new SignalRClient(connection);
        
        // Mapper
        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<RidePriceRequest, RideRequest>());
        var mapper = new Mapper(mapperConfig);
        
        // Services
        IConsoleReadService consoleReadService = new ConsoleReadService(HelpOption);
        IClientRideInputService clientRideInputService = new ClientRideInputService(consoleReadService);
        IRideRequestService rideRequestService = new RideRequestService(
            signalRClient, 
            clientRideInputService, 
            mapper, 
            AcceptRideOption, 
            DeclineRideOption);
        
        // Get ride request from user
        var rideRequest = await rideRequestService.GetRideRequest();
        
        // Currently RequestRide is not implemented in CityRide.ClientService
        await signalRClient.SendRideRequest(rideRequest);
        
        try
        {
            await signalRClient.StartReceivingRideStatusMessages(message =>
            {
                Console.WriteLine($"Received ride status message: {message}");
            });

            Console.WriteLine("Waiting for ride status messages. Press any key to exit.");
            Console.ReadKey();
        }
        finally
        {
            await connection.StopAsync();
        }
    }
}