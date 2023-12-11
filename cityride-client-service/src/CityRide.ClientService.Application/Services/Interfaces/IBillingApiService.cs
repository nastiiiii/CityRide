using CityRide.ClientService.Domain.Dtos;

namespace CityRide.ClientService.Application.Services.Interfaces
{
    public interface IBillingApiService
    {
        Task<CalculatedRidePriceDto> CalculateRidePriceAsync(RidePriceRequestDto ridePriceRequestDto);
    }
}
