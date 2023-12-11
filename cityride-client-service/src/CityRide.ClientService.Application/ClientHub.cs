using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Confluent.Kafka;
using CityRide.Kafka.Interfaces;
using CityRide.Events;
using AutoMapper;
using CityRide.ClientService.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using CityRide.ClientService.Application.Services.Interfaces;

namespace CityRide.ClientService.Application
{
    [Authorize]
    public class ClientHub : Hub<IClientAppClient>
    {
        private readonly IBillingApiService _billingApiService;
        private readonly IProducer<string, RideRequested> _producer;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ClientHub(
            IBillingApiService billingApiService,
            IProducerFactory<string, RideRequested> producerFactory,
            IConfiguration configuration,
            IMapper mapper)
        {
            _producer = producerFactory.CreateProducer();
            _billingApiService = billingApiService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task SendMessageToClient(string clientId, string message)
        {
            await Clients.Client(clientId).ReceiveRideStatus(message);
        }

        public async Task<decimal> CalculateRidePrice(RidePriceRequestDto requestDto)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException();
            }

            var ridePrice = await _billingApiService.CalculateRidePriceAsync(requestDto);

            return ridePrice.TotalCost;
        }

        public async Task RequestRide(RideRequestDto request)
        {
            if(request == null)
            {
                throw new ArgumentNullException();
            }

            var kafkaEvent = new Message<string, RideRequested>
            {
                Key = Context.UserIdentifier!,
                Value = _mapper.Map<RideRequested>(request)
            };

            await _producer.ProduceAsync(_configuration["KafkaTopics:ClientRideRequestTopic"], kafkaEvent);
        }

    } 
}
