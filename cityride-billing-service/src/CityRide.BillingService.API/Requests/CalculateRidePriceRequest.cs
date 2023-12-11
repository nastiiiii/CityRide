using CityRide.Domain.Dtos;
using CityRide.Domain.Enums;

namespace CityRide.BillingService.API.Requests;

public class CalculateRidePriceRequest
{
    public CarClass CarClass { get; set; }
    
    // Not null by validator restraints
    public LocationDto Source { get; set; } = null!;
    public LocationDto Destination { get; set; } = null!;
}