using FluentValidation;
using CityRide.BillingService.API.Requests;

namespace CityRide.BillingService.API.Validators;

public class UpdateRidePriceRequestValidator : AbstractValidator<UpdateRidePriceRequest>
{
    public UpdateRidePriceRequestValidator()
    {
        RuleFor(c => c.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Name).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
        RuleFor(c => c.Coefficient).Cascade(CascadeMode.Stop).GreaterThan(0);
        RuleFor(r => r.CostPerKm).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
        RuleFor(r => r.ExtraFees).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0);
    }
}