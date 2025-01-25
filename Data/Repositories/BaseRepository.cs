using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null) return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        } catch (Exception ex)  
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if(predicate == null) return null!;

        return await _dbSet.FirstOrDefaultAsync(predicate) ?? null!;
    }

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        if (predicate == null) return null!;

        try
        {
            var currentEntity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (currentEntity == null) return null!;

            _context.Entry(currentEntity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return currentEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error Updating {nameof(TEntity)} entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) return false;

        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error Deleting {nameof(TEntity)} entity :: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) return false;

        return await _dbSet.FirstOrDefaultAsync(predicate) != null;
    }
}
