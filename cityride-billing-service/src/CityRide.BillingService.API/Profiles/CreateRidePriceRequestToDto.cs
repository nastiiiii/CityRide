using AutoMapper;
using CityRide.BillingService.API.Requests;
using CityRide.BillingService.Domain.Dtos;

namespace CityRide.BillingService.Application.Profiles;

public class CreateRidePriceRequestToDto : Profile
{
    public CreateRidePriceRequestToDto()
    {
        CreateMap<CreateRidePriceRequest, RidePriceDto>();
    }
}