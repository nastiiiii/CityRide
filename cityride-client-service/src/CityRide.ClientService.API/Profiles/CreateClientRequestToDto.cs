using AutoMapper;
using CityRide.ClientService.API.Client.Requests;
using CityRide.ClientService.Domain.Dtos;

namespace CityRide.ClientService.API.Profiles
{
    public class CreateClientRequestToDto : Profile
    {
        public CreateClientRequestToDto() {
            CreateMap<CreateClientRequest, ClientDto>();
        }
    }
}
