namespace CityRide.BillingService.API.Responses;

public class CalculateRidePriceResponse
{
    public string CarClass { get; set; } = string.Empty;
    public double TotalCost { get; set; }
}