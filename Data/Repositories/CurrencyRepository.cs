using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class CurrencyRepository(DataContext context) : BaseRepository<CurrencyEntity>(context), ICurrencyRepository
{
    private readonly DataContext _context = context;
}

