using AutoMapper;

using ClientConsoleApp.Models;
using ClientConsoleApp.Services.Interfaces;

namespace ClientConsoleApp.Services;

// Facade for user request logic
public class RideRequestService : IRideRequestService
{
    private readonly SignalRClient _signalRClient;
    private readonly IClientRideInputService _clientRideInputService;
    private readonly IMapper _mapper;
    private readonly string _acceptRideOption;
    private readonly string _declineRideOption;

    public RideRequestService(
        SignalRClient signalRClient, 
        IClientRideInputService clientRideInputService, 
        IMapper mapper,
        string acceptRideOption, 
        string declineRideOption)
    {
        _signalRClient = signalRClient;
        _clientRideInputService = clientRideInputService;
        _mapper = mapper;
        _acceptRideOption = acceptRideOption;
        _declineRideOption = declineRideOption;
    }

    public async Task<RideRequest> GetRideRequest()
    {
        RidePriceRequest? ridePriceRequest = null;
        bool isRideAccepted = false;
        decimal ridePrice = 0;

        do
        {
            // User input: source and destination locations, car class
            ridePriceRequest = GetRidePriceRequest();

            // Calculate ride price
            ridePrice = await CalculateRidePrice(ridePriceRequest);

            // Get user decision on whether he wants to accept this ride
            isRideAccepted = IsRideAccepted(ridePrice);

            if (!isRideAccepted)
            {
                Console.WriteLine("Ride declined successfully.\n");
            }
        } while (!isRideAccepted);
        
        Console.WriteLine("Ride accepted successfully.\n");

        var rideRequest = _mapper.Map<RideRequest>(ridePriceRequest);
        rideRequest.Price = ridePrice;
        return _mapper.Map<RideRequest>(ridePriceRequest);
    }

    private RidePriceRequest GetRidePriceRequest()
    {
        var sourceLocation = _clientRideInputService.GetLocationInput("source");
        Console.WriteLine();
        var destinationLocation = _clientRideInputService.GetLocationInput("destination");
        Console.WriteLine();
        var carClass = _clientRideInputService.GetCarClassInput();
        Console.WriteLine();

        return new RidePriceRequest
        {
            Source = sourceLocation,
            Destination = destinationLocation,
            CarClass = carClass
        };
    }

    private async Task<decimal> CalculateRidePrice(RidePriceRequest request)
    {
        Console.WriteLine("Calculating ride price..");
        decimal ridePrice = await _signalRClient.SendRidePriceRequest(request);

        return ridePrice;
    }

    private bool IsRideAccepted(decimal ridePrice)
    {
        string decision = _clientRideInputService.GetRideDecisionInput(ridePrice, _acceptRideOption, _declineRideOption);

        // Decision can have only two values that were passed to the method above
        return decision == _acceptRideOption;
    }
}