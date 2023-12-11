using System.Net;

namespace CityRide.DriverService.Application.Exceptions.DriverExceptions;

public class DriverNotFoundException : DriverServiceException
{
    public DriverNotFoundException()
        : base("Driver not found", (int)HttpStatusCode.NotFound)
    {
    }

    public DriverNotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }

    public DriverNotFoundException(int userId)
        : base($"Driver with id {userId} not found", (int)HttpStatusCode.NotFound)
    {
    }

    public DriverNotFoundException(string message, Exception innerException)
        : base(message, innerException, (int)HttpStatusCode.NotFound)
    {
    }
}