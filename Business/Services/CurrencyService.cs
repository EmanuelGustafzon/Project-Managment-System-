using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Business.Models;
using Business.Factories;
using System.Diagnostics;
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

            IEnumerable<CurrencyDto> currencyDtoList = currencyEntities.Select(currencyEntity => CurrencyFactory.CreateDto(currencyEntity));

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

            CurrencyDto currencyResponseDto = CurrencyFactory.CreateDto(currencyEntity);

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
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<CurrencyRegistrationForm>(currencyForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }
        try
        {
            bool alreadyExist = await _currencyRepository.EntityExistsAsync(x => x.Currency == currencyForm.Currency);
            if (alreadyExist) return Result.AlreadyExists("Currency already exists");


            CurrencyEntity currencyEntity = CurrencyFactory.CreateEntity(currencyForm);

            CurrencyEntity resultEntity = await _currencyRepository.CreateAsync(currencyEntity);

            return Result<CurrencyDto>.Created(CurrencyFactory.CreateDto(resultEntity));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to create currency");
        }
    }
    public async Task<IResponseResult> UpdateCurrencyAsync(int id ,CurrencyRegistrationForm updatedCurrencyForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<CurrencyRegistrationForm>(updatedCurrencyForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        try
        {
            bool currencyExists = await _currencyRepository.EntityExistsAsync(x => x.Id == id);
            if (currencyExists == false) return Result.NotFound($"Currency not found with the id: {id}");

            var updatedEntity = await _currencyRepository.UpdateAsync(x => x.Id == id, CurrencyFactory.CreateEntity(id, updatedCurrencyForm));
            if (updatedEntity == null) return Result.InternalError("Failed to update the currency");

            CurrencyDto currencyDto = CurrencyFactory.CreateDto(updatedEntity);
            return Result<CurrencyDto>.Ok(currencyDto);
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
            return Result.InternalError("failed to delete currency");
        }
    }
}
