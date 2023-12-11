using CityRide.ClientService.Domain.Dtos;
using CityRide.ClientService.Application.Exceptions;
using CityRide.ClientService.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CityRide.ClientService.Application.Services
{
    public class BillingApiService : IBillingApiService
    {
        private readonly IConfiguration _configuration;

        public BillingApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CalculatedRidePriceDto> CalculateRidePriceAsync(RidePriceRequestDto ridePriceRequestDto)
        {
            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonContent.Create(ridePriceRequestDto);
                var result = await httpClient.PostAsync(_configuration["BillingService:CalculateUrl"], jsonContent);
                
                if (result.IsSuccessStatusCode)
                {
                    var ridePriceDto = JsonConvert.DeserializeObject<CalculatedRidePriceDto>(await result.Content.ReadAsStringAsync());

                    if(ridePriceDto == null)
                    {
                        throw new BillingApiServiceException("Response is null");
                    }

                    return ridePriceDto;
                }

                throw new BillingApiServiceException(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
