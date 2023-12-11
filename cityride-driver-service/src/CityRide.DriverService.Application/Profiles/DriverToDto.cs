using AutoMapper;
using CityRide.DriverService.Domain.Dtos;
using CityRide.DriverService.Domain.Entities;

namespace CityRide.DriverService.Application.Profiles;

public class DriverToDto : Profile
{
    public DriverToDto()
    {
        CreateMap<Driver, DriverDto>().ReverseMap();
    }
}