using System.Net;

namespace CityRide.BillingService.Application.Exceptions;

public class RidePriceNotFoundException : BillingServiceException
{
    public RidePriceNotFoundException()
        : base("Ride price not found", (int)HttpStatusCode.NotFound)
    {
    }

    public RidePriceNotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }

    public RidePriceNotFoundException(int ridePriceId)
        : base($"Ride price with id {ridePriceId} not found", (int)HttpStatusCode.NotFound)
    {
    }

    public RidePriceNotFoundException(string message, Exception innerException)
        : base(message, innerException, (int)HttpStatusCode.NotFound)
    {
    }
}