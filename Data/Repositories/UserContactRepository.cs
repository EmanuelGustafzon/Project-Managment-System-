using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class UserContactRepository(DataContext context) : BaseRepository<UserContactEntity>(context), IUserContactRepository
{
    public readonly DataContext _context = context;
}
