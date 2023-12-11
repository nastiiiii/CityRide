using CityRide.BillingService.Domain.Dtos;
using CityRide.Domain.Enums;

namespace CityRide.BillingService.Application.Services.Interfaces;

public interface IRidePriceService
{
    Task<IEnumerable<RidePriceDto?>> GetAllRidePrices();
    Task<RidePriceDto?> GetRidePriceByCarClass(CarClass carClass);
    Task<RidePriceDto?> GetRidePriceById(int ridePriceId);
    Task<RidePriceDto> CreateRidePrice(RidePriceDto ridePriceDto);
    Task UpdateRidePrice(RidePriceDto ridePriceDto);
    Task DeleteRidePrice(int ridePriceId);
}