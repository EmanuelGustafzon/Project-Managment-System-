using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Business.Models;
using Business.Factories;
using System.Diagnostics;
using Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Business.Services;
public class CurrencyService(ICurrencyRepository currencyRepository) : ICurrencyService
{

    private readonly ICurrencyRepository _currencyRepository = currencyRepository;

    public async Task<IResponseResult> GetAllCurrenciesAsync()
    {
        try
        {
            IEnumerable<CurrencyEntity> currencyEntities = await _currencyRepository.GetAllAsync();
            if (!currencyEntities.Any()) Result<IEnumerable<CurrencyDto>>.Ok([]);

            IEnumerable<CurrencyDto> currencyDtoList = currencyEntities.Select(x => CurrencyFactory.CreateDto(x.Id, x.Currency));

            return Result<IEnumerable<CurrencyDto>>.Ok(currencyDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Result.InternalError("Failed to get the currencies");
        }
    }
    public async Task<IResponseResult> GetCurrencyAsync(int id)
    {   
        try
        {
            var currencyEntity = await _currencyRepository.GetAsync(x => x.Id == id);
            if (currencyEntity == null) return Result.NotFound("The currency was not found");

            CurrencyDto currencyResponseDto = CurrencyFactory.CreateDto(currencyEntity.Id, currencyEntity.Currency);

            return Result<CurrencyDto>.Ok(currencyResponseDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to get the currency");
        }
    }
    public async Task<IResponseResult> CreateCurrencyAsync(CurrencyRegistrationForm currencyForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate(currencyForm);
        if (errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        try
        {
            bool alreadyExist = await _currencyRepository.GetAsync(x => x.Currency == currencyForm.Currency) != null;
            if (alreadyExist) return Result.AlreadyExists("Currency already exists");


            CurrencyEntity currencyEntity = CurrencyFactory.CreateCurrencyEntity(currencyForm);

            CurrencyEntity resultEntity = await _currencyRepository.CreateAsync(currencyEntity);

            return Result<CurrencyDto>.Created(CurrencyFactory.CreateDto(resultEntity.Id, resultEntity.Currency));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to create currency");
        }
    }
    public async Task<IResponseResult> UpdateCurrencyAsync(int id ,CurrencyRegistrationForm updatedCurrencyForm)
    {
        if (updatedCurrencyForm.Currency == null) return Result.BadRequest("No currency provided");

        try
        {
            bool currencyExists = await _currencyRepository.EntityExistsAsync(x => x.Id == id);
            if (currencyExists == false) return Result.NotFound($"Currency not found with the id: {id}");

            var currencyEntity = CurrencyFactory.CreateCurrencyEntity(CurrencyFactory.CreateDto(id, updatedCurrencyForm.Currency));

            var updatedEntity = await _currencyRepository.UpdateAsync(x => x.Id == id, currencyEntity);
            if (updatedEntity == null) return Result.InternalError("Failed to update the currency");

            return Result<CurrencyDto>.Ok(CurrencyFactory.CreateDto(id, updatedEntity.Currency));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to update the currency");
        }
    }

    public async Task<IResponseResult> DeleteCurrencyAsync(int id)
    {
        try
        {
            bool currencyExists = await _currencyRepository.EntityExistsAsync(x => x.Id == id);
            if (currencyExists == false) return Result.NotFound($"Currency not found with the id: {id}");

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
