using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
}
