namespace CityRide.ClientService.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity> CreateAsync(TEntity entity);
        void UpdateAsync (TEntity entity);
        Task DeleteAsync(object id);
    }
}
