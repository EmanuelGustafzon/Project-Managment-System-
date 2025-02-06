using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class CurrencyFactory
{
    public static CurrencyDto CreateDto(CurrencyEntity entity)
    {
        return new CurrencyDto { Id = entity.Id, Currency = entity.Currency};
    }
    public static CurrencyEntity CreateEntity(CurrencyRegistrationForm form)
    {
        return new CurrencyEntity { Currency = form.Currency };
    }
    public static CurrencyEntity CreateEntity(int id, CurrencyRegistrationForm form)
    {
        return new CurrencyEntity { Id = id, Currency = form.Currency};
    }
}
