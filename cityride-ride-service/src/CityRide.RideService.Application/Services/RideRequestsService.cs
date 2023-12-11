using AutoMapper;
using Confluent.Kafka;
using CityRide.Domain.Enums;
using CityRide.Kafka.Interfaces;
using CityRide.RideService.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using CityRide.RideService.Application.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using CityRide.Events;

namespace CityRide.RideService.Application.Services
{
    public class RideRequestsService : IRideRequestsService
    {
        private readonly IRideService _rideService;
        private readonly IConfiguration _configuration;
        private readonly IRedisClientService _redisClientService;
        private readonly IDriverApiService _driverApiService;
        private readonly IHubContext<RideHub, IRideClient> _rideHubContext;

        private readonly IConsumer<string, RideRequested> _consumer;
        private readonly IProducer<string, RideStatusUpdated> _producer;
        private readonly IMapper _mapper;
        private readonly string _clientRideRequestsTopic;

        public RideRequestsService(
            IMapper mapper,
            IRideService rideService,
            IConfiguration configuration,
            IRedisClientService redisClientService,
            IDriverApiService driverApiService,
            IHubContext<RideHub, IRideClient> rideHubContext,
            IConsumerFactory<string, RideRequested> consumerFactory,
            IProducerFactory<string, RideStatusUpdated> producerFactory)
        {
            _consumer = consumerFactory.CreateConsumer();
            _producer = producerFactory.CreateProducer();
            _rideHubContext = rideHubContext;
            _driverApiService = driverApiService;
            _mapper = mapper;
            _rideService = rideService;
            _configuration = configuration;
            _redisClientService = redisClientService;
            _clientRideRequestsTopic = _configuration["Topics:ClientRideRequests"]!;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_clientRideRequestsTopic);

            stoppingToken.Register(_consumer.Close);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                try
                {
                    var rideDto = await CreateRideEntityAsync(consumeResult.Message);
                    var closestDrivers = await RequestClosestDriversAsync(rideDto.To, consumeResult.Message.Value.CarClass);

                    if(closestDrivers.Any())
                    {
                        await SetClosestDriversInCacheAsync(rideDto.Id, closestDrivers);
                        await NotifyDriversAsync(closestDrivers, _mapper.Map<ReceiveRideRequestDto>(rideDto));
                    }
                    else
                    {
                        var clientMessage = new Message<string, RideStatusUpdated>
                        {
                            Key = consumeResult.Message.Key,
                            Value = new RideStatusUpdated
                            {
                                Status = RideStatus.Declined
                            }
                        };
                        await _producer.ProduceAsync(_configuration["Topics:RideStatus"], clientMessage);
                        await _rideService.DeclineRide(rideDto.Id);
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Error occured while processing event: {ex.Message}");
                }
            }
        }

        private async Task<RideDto> CreateRideEntityAsync(Message<string, RideRequested> message)
        {
            var rideDto = _mapper.Map<RideRequested, RideDto>(message.Value);

            rideDto.ClientId = int.Parse(message.Key);
            rideDto.Price = message.Value.Price;
            rideDto = await _rideService.CreateRideAsync(rideDto);
            return rideDto;
        }

        private async Task<List<ClosestDriverDto>> RequestClosestDriversAsync(LocationDto location, CarClass carClass)
        {
            var requestClosestDriversDto = new ClosestDriversRequestDto
            {
                CarClass = carClass,
                Location = location,
                DistanceInMeters = int.Parse(_configuration["SearchDriversOptions:DistanceInMeters"]!),
                NumberOfUsersToRetrieve = int.Parse(_configuration["SearchDriversOptions:NumberOfDriversToReturn"]!)
            };

            var result = await _driverApiService.GetClosestDriversAsync(requestClosestDriversDto);

            return result;
        }

        private async Task SetClosestDriversInCacheAsync(int rideId, List<ClosestDriverDto> closestDriverDtos)
        {
            var closestDrivers = closestDriverDtos.Select(x => x.DriverId.ToString());

            await _redisClientService.SetClosestDriversListAsync(rideId.ToString(), closestDrivers);
        }

        private async Task NotifyDriversAsync(List<ClosestDriverDto> closestDrivers, ReceiveRideRequestDto receiveRideRequestDto)
        {
            foreach(var driver in closestDrivers)
            {
                await _rideHubContext.Clients.User(driver.DriverId.ToString()).ReceiveRideRequest(receiveRideRequestDto);
            }
        }
    }
}
