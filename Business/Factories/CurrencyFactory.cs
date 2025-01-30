using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class CurrencyFactory
{
    public static CurrencyDto CreateCurrencyDto()
    {
        return new CurrencyDto();
    }
    public static CurrencyDto CreateCurrencyDto(string currency)
    {
        return new CurrencyDto { Currency = currency };
    }
    public static CurrencyDto CreateCurrencyDto(string currency, int id)
    {
        return new CurrencyDto { Currency = currency, Id = id };
    }
    public static CurrencyEntity CreateCurrencyEntity(CurrencyDto form)
    {
        return new CurrencyEntity { Currency = form.Currency, Id = form.Id };
    }
}
