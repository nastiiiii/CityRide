using AutoMapper;
using CityRide.DriverService.API.Driver.Responses;
using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.API.Profiles;

public class DriverDtoToDriverProfileResponse : Profile
{
    public DriverDtoToDriverProfileResponse()
    {
        CreateMap<DriverDto, DriverProfileResponse>();
    }
}