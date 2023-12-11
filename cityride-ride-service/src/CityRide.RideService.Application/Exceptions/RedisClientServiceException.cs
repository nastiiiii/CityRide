namespace CityRide.RideService.Application.Exceptions
{
    public class RedisClientServiceException : Exception
    {
        public RedisClientServiceException()
        : base()
        {
        }

        public RedisClientServiceException(string message)
            : base(message)
        {
        }
    }
}
