using CityRide.DriverService.Domain.Entities;

namespace CityRide.DriverService.Domain.Repositories;

public interface IDriverRepository : IBaseRepository<Driver>
{
    Task<Driver> GetDriverByEmailAndPasswordHashAsync(string email, string passwordHash);
}