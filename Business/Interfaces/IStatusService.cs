using Business.Dtos;

namespace Business.Interfaces;

public interface IStatusService
{
    public Task<IResponseResult> GetAllStatusesAsync();
    public Task<IResponseResult> GetStatusAsync(int id);
    public Task<IResponseResult> CreateStatusAsync(StatusRegistrationForm statusform);
    public Task<IResponseResult> UpdateStatusAsync(int id, StatusRegistrationForm updatedStatusForm);
    public Task<IResponseResult> DeleteStatusAsync(int id);
    public IResponseResult GetAllStatuses();
}
