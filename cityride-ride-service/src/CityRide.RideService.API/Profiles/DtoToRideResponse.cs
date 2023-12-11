using AutoMapper;
using CityRide.RideService.API.Ride.Responses;
using CityRide.RideService.Domain.Dtos;

namespace CityRide.RideService.API.Ride.Requests;

public class DtoToRideResponse : Profile
{
    public DtoToRideResponse()
    {
        CreateMap<RideDto, RideResponse>();
    }
}