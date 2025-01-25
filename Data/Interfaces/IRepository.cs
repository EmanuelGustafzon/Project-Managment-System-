using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

    public Task<TEntity> CreateAsync(TEntity entity);

    public Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity);

    public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    public Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
