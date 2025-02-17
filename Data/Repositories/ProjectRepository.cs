using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class ProjectRepository(DataContext context, IMemoryCache cache) : BaseRepository<ProjectEntity>(context, cache), IProjectRepository
{
    private readonly DataContext _context = context;
    public async override Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        var cacheKey = GetCacheKey(nameof(GetAllAsync));

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<ProjectEntity>? cachedEntities) && cachedEntities != null)
            return cachedEntities;

        var projects = await _context.Projects
            .Include(x => x.ProjectManager)
            .Include(x => x.Customer)
            .Include(x => x.Service)
            .ThenInclude(x => x.Currency)
            .ToListAsync();

        _memoryCache.Set(cacheKey, projects, TimeSpan.FromMinutes(5));

        return projects;
    }
    public async override Task<ProjectEntity> GetAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        var cacheKey = GetCacheKey(nameof(GetAsync), predicate.ToString());

        if (_memoryCache.TryGetValue(cacheKey, out ProjectEntity? cachedEntity) && cachedEntity != null)
            return cachedEntity;

        var project = await _context.Projects
            .Where(predicate)
            .Include(x => x.ProjectManager)
            .Include(x => x.Customer)
            .Include(x => x.Service)
            .ThenInclude(x => x.Currency)
            .FirstOrDefaultAsync() ?? null!;

        _memoryCache.Set(cacheKey, project, TimeSpan.FromMinutes(5));

        return project;
    }
}
