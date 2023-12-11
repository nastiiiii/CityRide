using CityRide.ClientService.Domain.Entities;
using CityRide.ClientService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CityRide.ClientService.Infrastructure
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ClientServiceContext clientServiceContext) : base(clientServiceContext) { }
        public async Task<Client> GetClientViaEmailAndPasswordHash(string email, string passwordHash)
        {
            return await _context.Clients.Where(c => c.Email == email && c.Password == passwordHash).FirstOrDefaultAsync();
        }
    }
}
