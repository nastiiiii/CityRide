using CityRide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using CityRide.BillingService.Domain.Entities;
using CityRide.BillingService.Domain.Repositories;

namespace CityRide.BillingService.Infrastructure.Repositories;

public class RidePriceRepository : BaseRepository<RidePrice>, IRidePriceRepository
{
    public RidePriceRepository(BillingServiceContext context) :
        base (context) { }

    public async Task<RidePrice?> GetRidePriceByCarClass(CarClass carClass)
    {
        return await context.RidePrices.FirstOrDefaultAsync(r => r.CarClass == carClass);
    }
}