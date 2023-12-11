namespace CityRide.RideService.Application.Exceptions;

public class RideServiceException : Exception
{
    public int StatusCode { get; set; }

    public RideServiceException(string message, int code)
        : base(message)
    {
        StatusCode = code;
    }

    public RideServiceException(string message, Exception innerException, int code)
        : base(message, innerException)
    {
        StatusCode = code;
    }

    public RideServiceException(RideServiceException ex)
    {
    }
}