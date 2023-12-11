using AutoMapper;
using CityRide.DriverService.API.Driver.Requests;
using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.API.Profiles;

public class UpdateDriverRequestToDto : Profile
{
    public UpdateDriverRequestToDto()
    {
        CreateMap<UpdateDriverRequest, DriverDto>().ReverseMap();
    }
}