using CityRide.Domain.Enums;
using CityRide.BillingService.Domain.Entities;

namespace CityRide.BillingService.Domain.Repositories;

public interface IRidePriceRepository : IBaseRepository<RidePrice>
{
    Task<RidePrice?> GetRidePriceByCarClass(CarClass carClass);
}