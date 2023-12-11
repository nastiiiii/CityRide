using Microsoft.EntityFrameworkCore;
using CityRide.ClientService.Domain.Repositories;

namespace CityRide.ClientService.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal readonly ClientServiceContext _context;
        internal readonly DbSet<TEntity> _entitySet;
        public BaseRepository(ClientServiceContext appContext) {
            _context = appContext;
            _entitySet = appContext.Set<TEntity>();

        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _entitySet.ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(object id) { 
            return await _context.FindAsync<TEntity>(id);
        }

        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            var createdEntity = await _context.AddAsync<TEntity>(entity);
            _context.SaveChanges();
            return createdEntity.Entity;
        }

        public void UpdateAsync(TEntity entity)
        {
            _entitySet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task DeleteAsync(object id)
        {
            var entityToRemove = await _context.FindAsync<TEntity>(id);
            if (entityToRemove != null)
            {
                _entitySet.Remove(entityToRemove);
            }
            _context.SaveChanges(); 
        }

    }
}
