using System.Net;

namespace CityRide.DriverService.Application.Exceptions.DriverLocationExceptions;

public class DriverLocationNotFoundException : DriverLocationServiceException
{
    public DriverLocationNotFoundException()
        : base("Driver Locaiton not found", (int)HttpStatusCode.NotFound)
    {
    }

    public DriverLocationNotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }

    public DriverLocationNotFoundException(int userId)
        : base($"Driver Location for driver {userId} not found", (int)HttpStatusCode.NotFound)
    {
    }

    public DriverLocationNotFoundException(string message, Exception innerException)
        : base(message, innerException, (int)HttpStatusCode.NotFound)
    {
    }
}