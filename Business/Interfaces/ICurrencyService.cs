using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface ICurrencyService
{
    public Task<IResponseResult> GetAllCurrenciesAsync();
    public Task<IResponseResult> GetCurrencyAsync(int id);

    public Task<IResponseResult> CreateCurrencyAsync(CurrencyDto form);

    public Task<IResponseResult> UpdateCurrencyAsync(int id, CurrencyDto updatedForm);

    public Task<IResponseResult> DeleteCurrencyAsync(int id);
}
