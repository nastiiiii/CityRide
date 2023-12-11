using AutoMapper;
using CityRide.DriverService.API.Driver.Requests;
using CityRide.DriverService.Domain.Dtos;

namespace CityRide.DriverService.API.Profiles;

public class CreateDriverRequestToDto : Profile
{
    public CreateDriverRequestToDto()
    {
        CreateMap<CreateDriverRequest, DriverDto>().ReverseMap();
    }
}