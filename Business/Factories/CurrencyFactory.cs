using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class CurrencyFactory
{

    public static CurrencyRegistrationForm CreateRegistrationForm()
    {
        return new CurrencyRegistrationForm();
    }
    public static CurrencyRegistrationForm CreateRegistrationForm(string currency)
    {
        return new CurrencyRegistrationForm { Currency = currency};
    }
    public static CurrencyDto CreateDto(int id, string currency)
    {
        return new CurrencyDto { Id = id, Currency = currency};
    }
    public static CurrencyEntity CreateCurrencyEntity(CurrencyRegistrationForm form)
    {
        return new CurrencyEntity { Currency = form.Currency };
    }
    public static CurrencyEntity CreateCurrencyEntity(CurrencyDto form)
    {
        return new CurrencyEntity { Currency = form.Currency, Id = form.Id };
    }
}
