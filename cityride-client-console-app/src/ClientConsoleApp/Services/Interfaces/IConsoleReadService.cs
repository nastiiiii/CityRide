using CityRide.Domain.Enums;

namespace ClientConsoleApp.Services.Interfaces;

public interface IConsoleReadService
{
    double? ReadDouble();
    CarClass? ReadCarClass();
}