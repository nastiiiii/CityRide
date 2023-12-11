using AutoMapper;

using CityRide.BillingService.Domain.Dtos;
using CityRide.BillingService.Domain.Entities;

namespace CityRide.BillingService.Application.Profiles;

public class RidePriceToDto : Profile
{
    public RidePriceToDto()
    {
        CreateMap<RidePrice, RidePriceDto>().ReverseMap();
    }
}