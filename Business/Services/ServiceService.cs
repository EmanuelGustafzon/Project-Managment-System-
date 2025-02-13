using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Enums;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Business.Services;


public class ServiceService(IServiceRespository serviceRespository, ICurrencyRepository currencyRepository) : IServiceService
{
    private readonly IServiceRespository _serviceRespository = serviceRespository;
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;

    public async Task<IResponseResult> CreateServicesAsync(ServiceRegistrationForm serviceForm)
    {

        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<ServiceRegistrationForm>(serviceForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }
        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Name == serviceForm.Name);
            if (serviceExists == true) return Result.AlreadyExists($"Service already exsist with the name: {serviceForm.Name}");

            int currencyId = 0;
            var FoundCurrency = await _currencyRepository.GetAsync(x => x.Currency == serviceForm.Currency);
            if (FoundCurrency == null)
            {
                var currencyForm = CurrencyFactory.CreateRegistrationForm(serviceForm.Currency);
                var currency = await _currencyRepository.CreateAsync(CurrencyFactory.CreateEntity(currencyForm));
                if (currency == null) return Result.Error("Could not create currency");
                currencyId = currency.Id;
            }
            else currencyId = FoundCurrency.Id;

            ServiceEntity serviceEntity = ServiceFactory.CreateEntity(serviceForm, currencyId);
            ServiceEntity createdEntityInDb = await _serviceRespository.CreateAsync(serviceEntity);
            if(createdEntityInDb == null)
            {
                await _serviceRespository.RollbackTransactionAsync();
                return Result.Error("Could not create service");
            }
            ServiceDto serviceDto = ServiceFactory.CreateDto(createdEntityInDb);
            return Result<ServiceDto>.Created(serviceDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Failed to create the service");
        }
    }
    public async Task<IResponseResult> GetAllServicesAsync()
    {
        try
        {
            IEnumerable<ServiceEntity> serviceEntities = await _serviceRespository.GetAllAsync();
            if (!serviceEntities.Any()) Result<IEnumerable<ServiceDto>>.Ok([]);

            IEnumerable<ServiceDto> serviceDtoList = serviceEntities.Select(serviceEntity => ServiceFactory.CreateDto(serviceEntity));

            return Result<IEnumerable<ServiceDto>>.Ok(serviceDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Result.Error("Failed to get the services");
        }
    }

    public async Task<IResponseResult> GetServiceAsync(int id)
    {
        try
        {
            ServiceEntity serviceEntity = await _serviceRespository.GetAsync(x => x.Id == id);
            if (serviceEntity == null) return Result.NotFound("Service not found");

            return Result<ServiceDto>.Ok(ServiceFactory.CreateDto(serviceEntity));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not get service from database");
        }
    }

    public async Task<IResponseResult> UpdateServicesAsync(int id, ServiceRegistrationForm updatedForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<ServiceRegistrationForm>(updatedForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Id == id);
            if (serviceExists == false) return Result.NotFound($"Service not found with the id: {id}");

            int currencyId = 0;
            var FoundCurrency = await _currencyRepository.GetAsync(x => x.Currency == updatedForm.Currency);
            if (FoundCurrency == null)
            {
                var currencyForm = CurrencyFactory.CreateRegistrationForm(updatedForm.Currency);
                var currency = await _currencyRepository.CreateAsync(CurrencyFactory.CreateEntity(currencyForm));
                if (currency == null) return Result.Error("Could not create currency");
                currencyId = currency.Id;
            }
            else currencyId = FoundCurrency.Id;

            var updatedServiceEntity = await _serviceRespository.UpdateAsync(x => x.Id == id, ServiceFactory.CreateEntity(id, currencyId, updatedForm));
            if (updatedServiceEntity == null)
            {
                await _serviceRespository.RollbackTransactionAsync();
                return Result.Error("Could not update service");
            }
            ServiceDto serviceDto = ServiceFactory.CreateDto(updatedServiceEntity);
            return Result<ServiceDto>.Ok(serviceDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not update service in database");
        }
    }
    public async Task<IResponseResult> DeleteServicesAsync(int id)
    {
        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Id == id);
            if (serviceExists == false) return Result.NotFound($"Service not found with the id: {id}");

            bool result = await _serviceRespository.DeleteAsync(x => x.Id == id);
            if (result == false) return Result.Error("Could not delete service");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not delete service");
        }
    }
    public IResponseResult GetAllUnits()
    {
        try
        {
            List<string> unitsAsString = [];

            Units[] units = Enum.GetValues<Units>();
            foreach (var item in units)
            {
                unitsAsString.Add(item.ToString());
            }
            return Result<List<string>>.Ok(unitsAsString);

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Error retrieving units");
        }
    }
}
