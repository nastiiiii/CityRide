using CityRide.Domain.Enums;
using CityRide.RideService.Application.Services.Interfaces;
using CityRide.RideService.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CityRide.RideService.Application.Services
{
    public class DriverApiService : IDriverApiService
    {
        private readonly IConfiguration _configuration;

        public DriverApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ClosestDriverDto>> GetClosestDriversAsync(ClosestDriversRequestDto requestClosestDriversDto)
        {
            using(var httpClient  = new HttpClient())
            {
                var result = await httpClient.PostAsync(_configuration["DriverApi:RequestClosestDriversUrl"], JsonContent.Create(requestClosestDriversDto));

                if(result.IsSuccessStatusCode)
                {
                    var deserializedObject = JsonConvert.DeserializeObject<List<ClosestDriverDto>>(await result.Content.ReadAsStringAsync());
                    return deserializedObject ?? new List<ClosestDriverDto>();
                }
            }
            return new List<ClosestDriverDto>();
        }

        public async Task UpdateDriverStatusAsync(int driverId, DriverStatus driverStatus)
        {
            var parameters = new Dictionary<string, string>
            {
                { "driverId", $"{driverId}" },
                { "status", $"{driverStatus}" }
            };

            using(var httpClient = new HttpClient())
            {
                await httpClient.PutAsync(_configuration["DriverApi:RequestUpdateDriverStatusUrl"], new FormUrlEncodedContent(parameters));
            };
        }
    }
}
