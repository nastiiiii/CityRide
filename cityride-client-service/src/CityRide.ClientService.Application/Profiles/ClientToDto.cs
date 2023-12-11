using AutoMapper;
using CityRide.ClientService.Domain.Dtos;
using CityRide.ClientService.Domain.Entities;

namespace CityRide.ClientService.Application.Profiles
{
    public class ClientToDto : Profile
    {
        public ClientToDto() {
            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
