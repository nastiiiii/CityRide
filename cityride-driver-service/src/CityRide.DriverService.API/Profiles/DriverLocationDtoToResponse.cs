using AutoMapper;
using CityRide.Domain.Dtos;
using CityRide.DriverService.API.DriverLocation.Responses;
using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.API.Profiles
{
    public class DriverLocationDtoToResponse : Profile
    {
        public DriverLocationDtoToResponse()
        {
            CreateMap<DriverLocationDto, DriverLocationResponse>()
                .ForMember(x => x.Location, opt => opt.MapFrom(x => new LocationDto { Latitude = x.Location.Coordinate.X, Longitude = x.Location.Coordinate.Y }));
        }
    }
}
