namespace CityRide.DriverService.Application.Exceptions.DriverExceptions;

public class DriverServiceException : Exception
{
    public int StatusCode { get; set; }

    public DriverServiceException(string message, int code)
        : base(message)
    {
        StatusCode = code;
    }

    public DriverServiceException(string message, Exception innerException, int code)
        : base(message, innerException)
    {
        StatusCode = code;
    }

    public DriverServiceException(DriverServiceException ex)
    {
    }
}