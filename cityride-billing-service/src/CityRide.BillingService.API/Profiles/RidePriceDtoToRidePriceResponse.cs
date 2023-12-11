using AutoMapper;
using CityRide.BillingService.API.Responses;
using CityRide.BillingService.Domain.Dtos;

namespace CityRide.BillingService.Application.Profiles;

public class RidePriceDtoToRidePriceResponse : Profile
{
    public RidePriceDtoToRidePriceResponse()
    {
        CreateMap<RidePriceDto, RidePriceResponse>();
    }
}