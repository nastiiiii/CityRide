using AutoMapper;
using CityRide.DriverService.Domain.Dtos;
using CityRide.DriverService.Domain.Entities;

namespace CityRide.DriverService.Application.Profiles;

public class DriverLocationToDto : Profile
{
    public DriverLocationToDto()
    {
        CreateMap<DriverLocation, DriverLocationDto>().ReverseMap();
    }
}