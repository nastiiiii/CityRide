using AutoMapper;
using CityRide.RideService.Domain.Dtos;
using CityRide.RideService.Domain.Entities;

namespace CityRide.RideService.Application.Profiles;

public class LocationToDto : Profile
{
    public LocationToDto()
    {
        CreateMap<Location, LocationDto>().ReverseMap();
    }
}