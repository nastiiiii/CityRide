using CityRide.Domain.Enums;

namespace CityRide.BillingService.API.Responses
{
    public class RidePriceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CarClass CarClass { get; set; }
        public double Coefficient { get; set; } = 1;
        public double CostPerKm { get; set; }
        public double ExtraFees { get; set; }
    }
}
