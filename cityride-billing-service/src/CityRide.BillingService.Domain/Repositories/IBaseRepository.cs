namespace CityRide.BillingService.Domain.Repositories;

public interface IBaseRepository <TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(object id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}