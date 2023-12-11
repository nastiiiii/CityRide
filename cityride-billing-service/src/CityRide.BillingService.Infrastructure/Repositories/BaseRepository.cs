using Microsoft.EntityFrameworkCore;

using CityRide.BillingService.Domain.Repositories;

namespace CityRide.BillingService.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    // Protected for any child directories
    protected readonly BillingServiceContext context;
    protected readonly DbSet<TEntity> entitySet;

    public BaseRepository(BillingServiceContext context)
    {
        this.context = context;
        this.entitySet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await context.FindAsync<TEntity>(id);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var createdEntry = await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return createdEntry.Entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        entitySet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entityToDelete = await context.FindAsync<TEntity>(id);

        if (entityToDelete != null)
        {
            entitySet.Remove(entityToDelete);
        }

        await context.SaveChangesAsync();
    }
}