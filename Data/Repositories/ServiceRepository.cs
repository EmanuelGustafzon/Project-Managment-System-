using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ServiceRepository(DataContext context, IMemoryCache cache) : BaseRepository<ServiceEntity>(context, cache), IServiceRespository
{
    private readonly DataContext _context = context;

    public async override Task<IEnumerable<ServiceEntity>> GetAllAsync()
    {
        var cacheKey = GetCacheKey(nameof(GetAllAsync));

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<ServiceEntity>? cachedEntities) && cachedEntities != null)
            return cachedEntities;

        var services = await _context.Services
        .Include(x => x.Currency)
        .ToListAsync();

        _memoryCache.Set(cacheKey, services, TimeSpan.FromMinutes(5));

        return services;
    }

    public async override Task<ServiceEntity> GetAsync(Expression<Func<ServiceEntity, bool>> predicate)
    {
        var cacheKey = GetCacheKey(nameof(GetAsync), predicate.ToString());

        if (_memoryCache.TryGetValue(cacheKey, out ServiceEntity? cachedEntity) && cachedEntity != null)
            return cachedEntity;

        var service = await _context.Services
        .Where(predicate)
        .Include(x => x.Currency)
        .FirstOrDefaultAsync() ?? null!;

        _memoryCache.Set(cacheKey, service, TimeSpan.FromMinutes(5));

        return service;
    }
}
