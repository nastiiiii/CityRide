using CityRide.ClientService.API.Client.Requests;
using FluentValidation;

namespace CityRide.ClientService.API.Validators
    
{
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator() {
            RuleFor(client => client.Email).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
            RuleFor(client => client.Password).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
            RuleFor(client => client.FirstName).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
            RuleFor(client => client.LastName).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
            RuleFor(client => client.PhoneNumber).Cascade(CascadeMode.Stop).NotNull().NotEmpty().Matches(@"^\d{10}$"); //number must be exactly 10 digits
        }
    }
}
