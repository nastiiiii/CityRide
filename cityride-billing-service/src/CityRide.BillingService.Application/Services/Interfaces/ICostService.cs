using CityRide.BillingService.Domain.Dtos;
using CityRide.Domain.Dtos;

namespace CityRide.BillingService.Application.Services.Interfaces;

public interface ICostService
{
    double CalculateRideCost(RidePriceDto? ridePriceDto, LocationDto source, LocationDto destination);
}