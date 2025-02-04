using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
    private readonly DataContext _context = context;

    public async override Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users
            .Include(x => x.ContactInformation)
            .ToListAsync();
    }

    public async override Task<UserEntity> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        return await _context.Users
        .Where(predicate)
        .Include(x => x.ContactInformation)
        .FirstOrDefaultAsync() ?? null!;
    }
}
