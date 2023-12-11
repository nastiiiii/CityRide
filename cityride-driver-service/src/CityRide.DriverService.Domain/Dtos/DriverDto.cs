using CityRide.Domain.Enums;

namespace CityRide.DriverService.Domain.Dtos;

public class DriverDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public CarClass CarClass { get; set; }
}