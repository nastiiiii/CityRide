using CityRide.Events;
using CityRide.Kafka.Interfaces;
using CityRide.ClientService.Application.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace CityRide.ClientService.Application.Services
{
    public class ClientRideStatusService : BackgroundService
	{
		private readonly IConsumer<string, RideStatusUpdated> _kafkaConsumer;
		private readonly IHubContext<ClientHub, IClientAppClient> _hubContext;
		private readonly string _rideStatusTopic;
		

		public ClientRideStatusService(
			IConsumerFactory<string, RideStatusUpdated> kafkaConsumer, 
			IHubContext<ClientHub, IClientAppClient> hubContext, 
			IConfiguration configuration)
        {
			_kafkaConsumer = kafkaConsumer.CreateConsumer();
			_hubContext = hubContext;
            _rideStatusTopic = configuration["KafkaTopics:RideStatusTopic"];

		}

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(_rideStatusTopic))
            {
                throw new InvalidOperationException("Kafka topic is not configured.");
            }

            _kafkaConsumer.Subscribe(_rideStatusTopic);

            stoppingToken.Register(_kafkaConsumer.Close);

            await Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _kafkaConsumer.Consume(stoppingToken);
                        if (consumeResult != null)
                        {
                            var rideStatus = consumeResult.Message.Value;

                            _hubContext.Clients.User(consumeResult.Message.Key).ReceiveRideStatus($"{rideStatus.Status}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while consuming the Kafka topic.");
                    }
                }
            });
        }
    }
}
