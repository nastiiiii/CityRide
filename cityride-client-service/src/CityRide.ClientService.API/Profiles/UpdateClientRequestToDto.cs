using AutoMapper;
using CityRide.ClientService.Domain.Dtos;
using CityRide.ClientService.API.Client.Requests;

namespace CityRide.ClientService.API.Profiles
{
    public class UpdateClientRequestToDto : Profile
    {
        public UpdateClientRequestToDto() {
            CreateMap<UpdateClientRequest, ClientDto>();
        }
    }
}
