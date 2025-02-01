using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ServiceRepository(DataContext context) : BaseRepository<ServiceEntity>(context), IServiceRespository
{
    private readonly DataContext _context = context;

    public async override Task<IEnumerable<ServiceEntity>> GetAllAsync()
    {
        return await _context.Services
        .Include(x => x.Currency)
        .ToListAsync();
    }

    public async override Task<ServiceEntity> GetAsync(Expression<Func<ServiceEntity, bool>> predicate)
    {
        return await _context.Services
        .Where(predicate)
        .Include(x => x.Currency)
        .FirstOrDefaultAsync() ?? null!;
    }
}
