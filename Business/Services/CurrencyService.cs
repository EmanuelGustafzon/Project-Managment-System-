using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Business.Models;
using Business.Factories;
using System.Diagnostics;

namespace Business.Services;
public class CurrencyService(ICurrencyRepository currencyRepository) : ICurrencyService
{

    private readonly ICurrencyRepository _currencyRepository = currencyRepository;

    public async Task<IResponseResult> GetAllCurrenciesAsync()
    {
        try
        {
            IEnumerable<CurrencyEntity> currencyEntities = await _currencyRepository.GetAllAsync();
            IEnumerable<CurrencyDto> currencyDtoList = currencyEntities.Select(x => CurrencyFactory.CreateCurrencyDto(x.Currency, x.Id));

            return Result<IEnumerable<CurrencyDto>>.Ok(currencyDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Result.InternalError(ex.Message);
        }
    }
    public async Task<IResponseResult> GetCurrencyAsync(int id)
    {   
        try
        {
            var currencyEntity = await _currencyRepository.GetAsync(x => x.Id == id);
            if (currencyEntity == null) return Result.NotFound("The currency was not found");
            CurrencyDto currencyDto = CurrencyFactory.CreateCurrencyDto(currencyEntity.Currency, currencyEntity.Id);

            return Result<CurrencyDto>.Ok(currencyDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError(ex.Message);
        }
    }
    public async Task<IResponseResult> CreateCurrencyAsync(CurrencyDto form)
    {
        if (form.Currency == null) return Result.BadRequest("No currency was provided");

        try
        {
            bool alreadyExist = await _currencyRepository.GetAsync(x => x.Currency == form.Currency) != null;
            if (alreadyExist) return Result.AlreadyExists("Currency already exists");


            CurrencyEntity currencyEntity = CurrencyFactory.CreateCurrencyEntity(form);
            await _currencyRepository.CreateAsync(currencyEntity);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError(ex.Message);
        }
    }
    public async Task<IResponseResult> UpdateCurrencyAsync(CurrencyDto updatedForm)
    {
        if (updatedForm.Currency == null) return Result.BadRequest("No currency provided");

        try
        {
            var entityDoesNotExists = await _currencyRepository.GetAsync(x => x.Id == updatedForm.Id) == null;
            if (entityDoesNotExists) return Result.NotFound("The currency was not found");

            var currencyEntity = CurrencyFactory.CreateCurrencyEntity(updatedForm);
            var updatedEntity = await _currencyRepository.UpdateAsync(x => x.Id == currencyEntity.Id, currencyEntity);
            if (updatedEntity == null) return Result.InternalError("Failed to update the currency");

            return Result<CurrencyDto>.Ok(CurrencyFactory.CreateCurrencyDto(updatedEntity.Currency, updatedEntity.Id));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError(ex.Message);
        }
    }

    public async Task<IResponseResult> DeleteCurrencyAsync(int id)
    {
        try
        {
            var currencyEntity = await _currencyRepository.GetAsync(x => x.Id == id);
            if (currencyEntity == null) return Result.NotFound("The currency was not found");

            bool result = await _currencyRepository.DeleteAsync(x => x.Id == id);
            if (result == false) return Result.InternalError("Failed to delete the currency");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError(ex.Message);
        }
    }
}
