using AutoMapper;
using CityRide.BillingService.API.Requests;
using CityRide.BillingService.Domain.Dtos;

namespace CityRide.BillingService.Application.Profiles;

public class UpdateRidePriceRequestToDto : Profile
{
    public UpdateRidePriceRequestToDto()
    {
        CreateMap<UpdateRidePriceRequest, RidePriceDto>();
    }
}