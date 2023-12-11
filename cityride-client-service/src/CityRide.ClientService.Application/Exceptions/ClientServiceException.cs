namespace CityRide.ClientService.Application.Exceptions
{
    public class ClientServiceException : Exception
    {
        public int ErrorCode { get; set; }

        public ClientServiceException(string message, int code) : base(message)
        {
            ErrorCode = code;
        }

        public ClientServiceException(string message, Exception innerException, int code) : base(message, innerException)
        {
            ErrorCode = code;
        }
    }
}
