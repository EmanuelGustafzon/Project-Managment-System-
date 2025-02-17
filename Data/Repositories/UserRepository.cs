using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class UserRepository(DataContext context, IMemoryCache cache) : BaseRepository<UserEntity>(context, cache), IUserRepository
{
    private readonly DataContext _context = context;
}
