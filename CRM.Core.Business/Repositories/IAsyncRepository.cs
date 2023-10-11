namespace CRM.Core.Business.Repositories;

public interface IAsyncRepository<TEntity> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> FindManyWhere(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindOneWhere(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
