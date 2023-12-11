using CityRide.DriverService.Application.Exceptions.DriverExceptions;

namespace CityRide.DriverService.Application.Exceptions.DriverLocationExceptions;

public class DriverLocationServiceException : Exception
{
    public int StatusCode { get; set; }

    public DriverLocationServiceException(string message, int code)
        : base(message)
    {
        StatusCode = code;
    }

    public DriverLocationServiceException(string message, Exception innerException, int code)
        : base(message, innerException)
    {
        StatusCode = code;
    }

    public DriverLocationServiceException(DriverServiceException ex)
    {
    }
}