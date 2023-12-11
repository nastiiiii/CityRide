using ClientConsoleApp.Models;

namespace ClientConsoleApp.Services.Interfaces;

public interface IRideRequestService
{
    Task<RideRequest> GetRideRequest();
}