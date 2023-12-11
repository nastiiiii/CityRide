using AutoMapper;
using CityRide.RideService.Domain.Dtos;
using CityRide.RideService.Domain.Entities;

namespace CityRide.RideService.Application.Profiles;

public class RideToDto : Profile
{
    public RideToDto()
    {
        CreateMap<Ride, RideDto>()
            .ForMember(r => r.Id, opt => opt.MapFrom(src => src.RideId)).ReverseMap();
    }
}