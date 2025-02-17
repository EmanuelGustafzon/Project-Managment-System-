using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context, IMemoryCache cache) : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;
    protected readonly IMemoryCache _memoryCache = cache;
    protected virtual string GetCacheKey(string methodName, object? key = null) => $"RepositoryCache_{methodName}_{typeof(TEntity).Name}_{key ?? "all"}";

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null) return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            _memoryCache.Remove(GetCacheKey(nameof(GetAllAsync)));

            return entity;
        } catch (Exception ex)  
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var cacheKey = GetCacheKey(nameof(GetAllAsync));

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<TEntity>? cachedEntities) && cachedEntities != null)
            return cachedEntities;

        var entities = await _dbSet.ToListAsync();

        _memoryCache.Set(cacheKey, entities, TimeSpan.FromMinutes(5));

        return entities;
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if(predicate == null) return null!;

        var cacheKey = GetCacheKey(nameof(GetAsync), predicate.ToString());

        if (_memoryCache.TryGetValue(cacheKey, out TEntity? cachedEntity) && cachedEntity != null)
            return cachedEntity;

        var entity = await _dbSet.FirstOrDefaultAsync(predicate) ?? null!;

        _memoryCache.Set(cacheKey, entity, TimeSpan.FromMinutes(5));

        return entity;
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

            _memoryCache.Remove(GetCacheKey(nameof(GetAllAsync)));
            _memoryCache.Remove(GetCacheKey(nameof(GetAllAsync), predicate.ToString()));

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

            _memoryCache.Remove(GetCacheKey(nameof(GetAllAsync)));
            _memoryCache.Remove(GetCacheKey(nameof(GetAllAsync), predicate.ToString()));

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error Deleting {nameof(TEntity)} entity :: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EntityExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) return false;

        return await _dbSet.FirstOrDefaultAsync(predicate) != null;
    }

    // Transactions
    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }
    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }
}
