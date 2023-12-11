using System.Net;

namespace CityRide.RideService.Application.Exceptions;

public class RideNotFoundException : RideServiceException
{
    public RideNotFoundException()
        : base("Ride not found", (int)HttpStatusCode.NotFound)
    {
    }

    public RideNotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }

    public RideNotFoundException(int rideId)
        : base($"Ride with id {rideId} not found", (int)HttpStatusCode.NotFound)
    {
    }

    public RideNotFoundException(string message, Exception innerException)
        : base(message, innerException, (int)HttpStatusCode.NotFound)
    {
    }
}