using AutoMapper;
using CityRide.ClientService.Application.Services.Interfaces;
using CityRide.ClientService.Domain.Dtos;
using CityRide.ClientService.Domain.Repositories;
using CityRide.ClientService.Application.Exceptions;
using System.Security.Cryptography;
using CityRide.ClientService.Domain.Entities;
using System.Text;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CityRide.ClientService.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        
        private int? _currentClientId;

        public ClientService(
            IClientRepository clientRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper) 
        {
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ClientDto> CreateClientAsync(ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            client.Password = ComputeObjectHash(client.Password);
            var createdClient = await _clientRepository.CreateAsync(client);
            return _mapper.Map<ClientDto>(createdClient);
        }
        public async Task<ClientDto> GetClientProfile(int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new ClientNotFoundException(clientId);
            }
            return _mapper.Map<ClientDto>(client);
        }

        public async Task UpdateClientAsync(ClientDto clientDto)
        {
            var client = await _clientRepository.GetByIdAsync(clientDto.ID);
            if (client == null)
            {
                throw new ClientNotFoundException(clientDto.ID);
            }

            client.Email = clientDto.Email;
            client.Password = ComputeObjectHash<string>(clientDto.Password);
            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.PhoneNumber = clientDto.PhoneNumber;
            _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int clientId) {
            await _clientRepository.DeleteAsync(clientId);  
        }

        public async Task<ClientDto?> GetClientByEmailAndPassword(string email, string password)
        {
            var passwordHashed = ComputeObjectHash(password);

            var client = await _clientRepository.GetClientViaEmailAndPasswordHash(email, passwordHashed);

            return _mapper.Map<ClientDto>(client);
        }

        public int? CurrentClientId
        {
            get
            {
                if (_currentClientId == null)
                {
                    var userIdClaim = GetUserIdFromClaims();

                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        _currentClientId = userId;
                    }
                }

                return _currentClientId;
            }
        }

        private string? GetUserIdFromClaims()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
        
        private string ComputeObjectHash<T>(T obj) {
            var json = JsonConvert.SerializeObject(obj);
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
