using CityRide.DriverService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CityRide.DriverService.Infrastructure;

public class BaseRepository<TEntity> :
    IBaseRepository<TEntity>
    where TEntity : class
{
    internal readonly DriverServiceContext _context;
    internal readonly DbSet<TEntity> _entitySet;

    public BaseRepository(DriverServiceContext appContext)
    {
        _context = appContext;
        _entitySet = appContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _context.FindAsync<TEntity>(id);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var createdEntry = await _context.AddAsync(entity);
        _context.SaveChanges();
        return createdEntry.Entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _entitySet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entityToRemove = await _context.FindAsync<TEntity>(id);

        if (entityToRemove != null) _entitySet.Remove(entityToRemove);
        _context.SaveChanges();
    }
}