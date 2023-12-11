using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CityRide.ClientService.Application.Services.Interfaces;
using CityRide.ClientService.API.Client.Responses;
using CityRide.ClientService.Domain.Dtos;
using CityRide.ClientService.API.Client.Requests;
using Microsoft.AspNetCore.Authorization;

namespace CityRide.ClientService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient(CreateClientRequest req)
        {
            var createdClient = await _clientService.CreateClientAsync(_mapper.Map<ClientDto>(req));

            return Ok(_mapper.Map<ClientResponse>(createdClient));
        }
        
        [HttpGet]
        public async Task<ActionResult<ClientResponse>> GetClientProfile()
        {
            if (_clientService.CurrentClientId == null)
            {
                return Unauthorized();
            }
            
            var client = await _clientService.GetClientProfile(_clientService.CurrentClientId.Value);
            return Ok(_mapper.Map<ClientResponse>(client));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateClient(UpdateClientRequest req)
        {
            if (_clientService.CurrentClientId == null && _clientService.CurrentClientId != req.ID)
            {
                return Unauthorized();
            }

            var clientDto = _mapper.Map<ClientDto>(req);
            await _clientService.UpdateClientAsync(clientDto);
            return NoContent();
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteClient()
        {
            if (_clientService.CurrentClientId == null)
            {
                return Unauthorized();
            }

            await _clientService.DeleteClientAsync(_clientService.CurrentClientId.Value);
            return NoContent(); 
        }
    }
}
