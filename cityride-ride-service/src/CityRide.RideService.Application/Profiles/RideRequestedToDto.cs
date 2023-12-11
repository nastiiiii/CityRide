using AutoMapper;
using CityRide.Events;
using CityRide.RideService.Domain.Dtos;

namespace CityRide.RideService.Application.Profiles;

public class RideRequestedToDto : Profile
{
    public RideRequestedToDto()
    {
        CreateMap<RideRequested, RideDto>()
            .ForMember(m => m.From,
            m => m.MapFrom(r => new LocationDto { Latitude = r.Source.Latitude, Longitude = r.Source.Longitude }))
            .ForMember(m => m.To,
            m => m.MapFrom(r => new LocationDto { Latitude = r.Destination.Latitude, Longitude = r.Destination.Longitude }))
            .ReverseMap();
    }
}
