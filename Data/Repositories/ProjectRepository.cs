using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    private readonly DataContext _context = context;

    public async override Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _context.Projects
            .Include(x => x.ProjectManager)
            .Include(x => x.Customer)
            .Include(x => x.Service)
            .ThenInclude(x => x.Currency)
            .ToListAsync();
    }
    public async override Task<ProjectEntity> GetAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return await _context.Projects
            .Where(predicate)
            .Include(x => x.ProjectManager)
            .Include(x => x.Customer)
            .Include(x => x.Service)
            .ThenInclude(x => x.Currency)
            .FirstOrDefaultAsync() ?? null!;
    }
}
