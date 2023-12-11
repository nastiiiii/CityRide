using AutoMapper;
using CityRide.RideService.Domain.Dtos;

namespace CityRide.RideService.Application.Profiles
{
    public class DtoToReceiveRideRequestDto : Profile
    {
        public DtoToReceiveRideRequestDto()
        {
            CreateMap<RideDto, ReceiveRideRequestDto>();
        }
    }
}
