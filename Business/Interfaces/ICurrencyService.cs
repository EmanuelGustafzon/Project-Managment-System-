using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface ICurrencyService
{
    public Task<IResponseResult> GetAllCurrenciesAsync();
    public Task<IResponseResult> GetCurrencyAsync(int id);
    public Task<IResponseResult> CreateCurrencyAsync(CurrencyRegistrationForm currencyform);
    public Task<IResponseResult> UpdateCurrencyAsync(int id, CurrencyRegistrationForm updatedCurrencyForm);
    public Task<IResponseResult> DeleteCurrencyAsync(int id);
}
