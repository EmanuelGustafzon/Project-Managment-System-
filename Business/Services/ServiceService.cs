using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services;


public class ServiceService(IServiceRespository serviceRespository, ICurrencyRepository currencyRepository) : IServiceService
{
    private readonly IServiceRespository _serviceRespository = serviceRespository;
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;

    public async Task<IResponseResult> CreateServicesAsync(ServiceRegistrationForm serviceForm)
    {
        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Name == serviceForm.Name);
            if (serviceExists == true) return Result.AlreadyExists($"Service already exsist with the name: {serviceForm.Name}");

            bool currencyExists = await _currencyRepository.EntityExistsAsync(x => x.Id == serviceForm.CurrencyId);
            if (serviceExists == false) return Result.AlreadyExists($"Currency Id not found with the Id: {serviceForm.CurrencyId}");

            ServiceEntity serviceEntity = ServiceFactory.CreateServiceEntity(serviceForm);
            ServiceEntity createdEntityInDb = await _serviceRespository.CreateAsync(serviceEntity);

            ServiceDto serviceDto = ServiceFactory.CreateDto(createdEntityInDb);

            return Result<ServiceDto>.Created(serviceDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to create the service");
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
            return Result.InternalError("Failed to get the services");
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
            return Result.InternalError("Could not get service from database");
        }
    }

    public async Task<IResponseResult> UpdateServicesAsync(int id, ServiceRegistrationForm updatedForm)
    {
        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Id == id);
            if (serviceExists == false) return Result.NotFound($"Service not found with the id: {id}");

            var updatedServiceEntity = await _serviceRespository.UpdateAsync(x => x.Id == id, ServiceFactory.CreateServiceEntity(id, updatedForm));
            if(updatedServiceEntity == null) return Result.InternalError("Could not update service in database");

            return Result<ServiceDto>.Ok(ServiceFactory.CreateDto(updatedServiceEntity));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not update service in database");
        }
    }
    public async Task<IResponseResult> DeleteServicesAsync(int id)
    {
        try
        {
            bool serviceExists = await _serviceRespository.EntityExistsAsync(x => x.Id == id);
            if (serviceExists == false) return Result.NotFound($"Service not found with the id: {id}");

            bool result = await _serviceRespository.DeleteAsync(x => x.Id == id);
            if (result == false) return Result.InternalError("Could not delete service");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not delete service");
        }
    }
}
