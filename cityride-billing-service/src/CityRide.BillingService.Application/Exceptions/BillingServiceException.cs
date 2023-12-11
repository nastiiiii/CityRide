namespace CityRide.BillingService.Application.Exceptions;

public class BillingServiceException : Exception
{
    public int StatusCode { get; set; }

    public BillingServiceException(string message, int code)
        : base(message)
    {
        StatusCode = code;
    }

    public BillingServiceException(string message, Exception innerException, int code)
        : base(message, innerException)
    {
        StatusCode = code;
    }

    public BillingServiceException(BillingServiceException ex)
    {
    }
}