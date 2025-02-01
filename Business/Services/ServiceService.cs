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
            bool currencyDoesNotExist = await _currencyRepository.GetAsync(x => x.Id == serviceForm.CurrencyId) == null;
            if (currencyDoesNotExist) return Result.BadRequest("Currency Id not found");

            bool serviceAlreadyExist = await _serviceRespository.GetAsync(x => x.Name == serviceForm.Name) != null;
            if (serviceAlreadyExist) return Result.AlreadyExists("Currency already exists");

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

    public Task<IResponseResult> UpdateServicesAsync(int id, ServiceRegistrationForm updatedForm)
    {
        throw new NotImplementedException();
    }
    public Task<IResponseResult> DeleteServicesAsync(int id)
    {
        throw new NotImplementedException();
    }
}
