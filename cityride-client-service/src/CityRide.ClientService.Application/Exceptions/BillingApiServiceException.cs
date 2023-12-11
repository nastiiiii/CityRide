using System.Net;

namespace CityRide.ClientService.Application.Exceptions
{
    public class BillingApiServiceException : ClientServiceException
    {
        public BillingApiServiceException()
            : base("BillingApiService error occured", (int)HttpStatusCode.InternalServerError)
        { 
        }

        public BillingApiServiceException(string message)
            :base(message, (int)HttpStatusCode.NotFound) { }

        public BillingApiServiceException(string message, Exception innerException)
            : base (message, innerException, (int)HttpStatusCode.InternalServerError) { }
    }
}
