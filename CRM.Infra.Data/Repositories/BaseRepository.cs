using CRM.Core.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRM.Infra.Data.Repositories;

public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected DbSet<TEntity> Set => _dbContext.Set<TEntity>();

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        Set.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync() => await Set.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        TEntity? data = await Set.FindAsync(id);
        return data;
    }


    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TEntity>> FindManyWhere(Expression<Func<TEntity, bool>> predicate) =>
        await Set.Where(predicate).ToListAsync();

    public async Task<TEntity?> FindOneWhere(Expression<Func<TEntity, bool>> predicate) => 
        await Set.Where(predicate).FirstOrDefaultAsync();
}
