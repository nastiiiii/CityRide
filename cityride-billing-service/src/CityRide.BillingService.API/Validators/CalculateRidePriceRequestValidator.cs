using System.Data;
using CityRide.BillingService.API.Requests;
using CityRide.BillingService.Domain.Dtos;
using FluentValidation;

namespace CityRide.BillingService.API.Validators;

public class CalculateRidePriceRequestValidator: AbstractValidator<CalculateRidePriceRequest>
{
    public CalculateRidePriceRequestValidator()
    {
        RuleFor(c => (int) c.CarClass).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Source).Cascade(CascadeMode.Stop).NotNull();
        RuleFor(c => c.Destination).Cascade(CascadeMode.Stop).NotNull();
    }
}