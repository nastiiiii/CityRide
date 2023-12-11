using FluentValidation;
using CityRide.BillingService.API.Requests;

namespace CityRide.BillingService.API.Validators;

public class CreateRidePriceRequestValidator : AbstractValidator<CreateRidePriceRequest>
{
    public CreateRidePriceRequestValidator()
    {
        RuleFor(c => c.Name).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
        RuleFor(c => c.Coefficient).Cascade(CascadeMode.Stop).GreaterThan(0);
        RuleFor(r => r.CostPerKm).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
        RuleFor(r => r.ExtraFees).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
    }
}