using CityRide.ClientService.Domain.Entities;

namespace CityRide.ClientService.Domain.Repositories
{
    public interface IClientRepository: IBaseRepository<Client>
    {
        Task<Client> GetClientViaEmailAndPasswordHash(string email, string passwordHash);
    }
}
