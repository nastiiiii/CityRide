using CityRide.Kafka.Interfaces;
using CityRide.RideService.Application.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CityRide.Events;
using CityRide.Domain.Enums;
using CityRide.RideService.Application.Exceptions;

namespace CityRide.RideService.Application.Services
{
    [Authorize]
    public class RideHub : Hub<IRideClient>
    {
        private readonly IRedisClientService _redisClientService;
        private readonly IProducer<string, RideStatusUpdated> _producer;
        private readonly IConfiguration _configuration;
        private readonly IRideService _rideService;
        private readonly IDriverApiService _driverApiService;

        public RideHub(
            IRedisClientService redisClientService,
            IProducerFactory<string, RideStatusUpdated> producerFactory,
            IConfiguration configuration,
            IRideService rideService,
            IDriverApiService driverApiService)
        {
            _producer = producerFactory.CreateProducer();
            _redisClientService = redisClientService;
            _configuration = configuration;
            _rideService = rideService;
            _driverApiService = driverApiService;
        }

        public async Task<string> AcceptRideRequest(int rideId)
        {
            int driverId = int.Parse(Context.UserIdentifier);
            try
            {
                await _redisClientService.DeleteKeyIfDriverInListAsync(rideId.ToString(), Context.UserIdentifier);
            }
            catch(RedisClientServiceException exception)
            {
                return exception.Message;
            }
            
            var ride = await _rideService.GetRide(rideId);
            var clientMessage = new Message<string, RideStatusUpdated>()
            {
                Key = ride.ClientId.ToString(),
                Value = new RideStatusUpdated
                {
                    Status = RideStatus.Accepted
                }
            };
            await _producer.ProduceAsync(_configuration["Topics:RideStatus"], clientMessage);

            await _rideService.AcceptRideAndSetDriver(rideId, driverId);

            await _driverApiService.UpdateDriverStatusAsync(driverId, CityRide.Domain.Enums.DriverStatus.Unavailable);

            return "Ride accepted successfully";
        }

        public async Task<string> DeclineRideRequest(int rideId)
        {
            bool result;
            try
            {
                result = await _redisClientService.DeleteElementFromListAsync(rideId.ToString(), Context.UserIdentifier);
            }
            catch(RedisClientServiceException exception)
            {
                return exception.Message;
            }
            

            if (!result)
            {
                var ride = await _rideService.GetRide(rideId);

                var clientMessage = new Message<string, RideStatusUpdated>
                {
                    Key = ride.ClientId.ToString(),
                    Value = new RideStatusUpdated
                    {
                        Status = RideStatus.Declined
                    }
                };
                await _producer.ProduceAsync(_configuration["Topics:RideStatus"], clientMessage);

                await _rideService.DeclineRide(rideId);
            }
            return "Ride declined successfully";
        }

        public async Task StartRide(int rideId)
        {
            var rideDto = await _rideService.GetRide(rideId);
            if(rideDto.DriverId == int.Parse(Context.UserIdentifier))
            {
                var ride = await _rideService.GetRide(rideId);
                var clientMessage = new Message<string, RideStatusUpdated>
                {
                    Key = ride.ClientId.ToString(),
                    Value = new RideStatusUpdated
                    {
                        Status = RideStatus.Started
                    }
                };
                await _producer.ProduceAsync(_configuration["Topics:RideStatus"], clientMessage);

                await _rideService.StartRide(rideDto.Id);
            }
        }

        public async Task EndRide(int rideId)
        {
            int driverId = int.Parse(Context.UserIdentifier);
            var rideDto = await _rideService.GetRide(rideId);
            if(rideDto.DriverId == driverId)
            {
                var ride = await _rideService.GetRide(rideId);
                var clientMessage = new Message<string, RideStatusUpdated>
                {
                    Key = ride.ClientId.ToString(),
                    Value = new RideStatusUpdated
                    {
                        Status = RideStatus.Ended
                    }
                };
                await _producer.ProduceAsync(_configuration["Topics:RideStatus"], clientMessage);

                await _rideService.EndRide(rideDto.Id);

                await _driverApiService.UpdateDriverStatusAsync(driverId, CityRide.Domain.Enums.DriverStatus.Available);
            }
        }
    }
}
