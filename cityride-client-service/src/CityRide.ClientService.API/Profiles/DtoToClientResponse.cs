using AutoMapper;
using CityRide.ClientService.API.Client.Responses;
using CityRide.ClientService.Domain.Dtos;

namespace CityRide.ClientService.API.Profiles
{
    public class DtoToClientResponse : Profile
    {
        public DtoToClientResponse() {
            CreateMap<ClientDto, ClientResponse>();
        }
    }
}
