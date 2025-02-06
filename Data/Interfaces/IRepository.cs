using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    public DataContext GetContext();
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

    public Task<TEntity> CreateAsync(TEntity entity);

    public Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity);

    public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    public Task<bool> EntityExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
