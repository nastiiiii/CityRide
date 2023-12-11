using AutoMapper;
using CityRide.ClientService.Domain.Dtos;
using CityRide.Domain.Dtos;
using CityRide.Events;

namespace CityRide.ClientService.Application.Profiles
{
    public class RideRequestDtoToRideRequested : Profile
    {
        public RideRequestDtoToRideRequested() {
            CreateMap<RideRequestDto, RideRequested>()
                .ForMember(m => m.Source,
                m => m.MapFrom(r => new LocationDto { Latitude = r.Source.Latitude, Longitude = r.Source.Longitude }))
                .ForMember(m => m.Destination,
                m => m.MapFrom(r => new LocationDto { Latitude = r.Destination.Latitude, Longitude = r.Destination.Longitude }));
        }
    }
}
