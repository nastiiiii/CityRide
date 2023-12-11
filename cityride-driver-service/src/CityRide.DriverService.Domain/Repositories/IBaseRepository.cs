namespace CityRide.DriverService.Domain.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(object id);

    Task<TEntity> CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(int id);
}