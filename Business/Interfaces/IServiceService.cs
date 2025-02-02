using Business.Dtos;

namespace Business.Interfaces;

public  interface IServiceService
{
    public Task<IResponseResult> GetAllServicesAsync();
    public Task<IResponseResult> GetServiceAsync(int id);
    public Task<IResponseResult> CreateServicesAsync(ServiceRegistrationForm serviceform);
    public Task<IResponseResult> UpdateServicesAsync(int id, ServiceRegistrationForm updatedServiceForm);
    public Task<IResponseResult> DeleteServicesAsync(int id);
    public IResponseResult GetAllUnits();
}
