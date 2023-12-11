using CityRide.DriverService.API.Driver.Requests;
using FluentValidation;

namespace CityRide.DriverService.API.Validators;

public class CreateDriverRequestValidator : AbstractValidator<CreateDriverRequest>
{
    public CreateDriverRequestValidator()
    {
        RuleFor(driver => driver.Email).NotEmpty().EmailAddress();
        RuleFor(driver => driver.Password).NotEmpty().MinimumLength(8);
        RuleFor(driver => driver.FirstName).NotEmpty();
        RuleFor(driver => driver.LastName).NotEmpty();
        RuleFor(driver => driver.PhoneNumber).NotEmpty().Matches(@"^\d{10}$")
            .WithMessage("Phone number must be 10 digits.");
        RuleFor(driver => driver.CarClass).IsInEnum();
    }
}