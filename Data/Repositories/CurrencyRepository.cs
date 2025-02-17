using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class CurrencyRepository(DataContext context, IMemoryCache cache) : BaseRepository<CurrencyEntity>(context, cache), ICurrencyRepository
{
    private readonly DataContext _context = context;
}

