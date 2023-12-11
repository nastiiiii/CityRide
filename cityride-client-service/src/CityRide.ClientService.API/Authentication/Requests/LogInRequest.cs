namespace CityRide.ClientService.API.Authentication.Requests;

public class LogInRequest
{
    // Not null by validator
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}