using Business.Dtos;
using Data.Entities;
using Data.Enums;

namespace Business.Factories;

internal class StatusFactory
{
    public static StatusRegistrationForm CreateRegistrationForm(string status)
    {
        return new StatusRegistrationForm { Status = status };
    }
    public static StatusDto CreateDto(StatusEntity entity)
    {
        return new StatusDto { Id = entity.Id, Status = entity.Status.ToString() };
    }
    public static StatusEntity CreateEntity(StatusRegistrationForm form)
    {
        return new StatusEntity { Status = Enum.TryParse<StatusStates>(form.Status, true, out var status) ? status : default };
    }
    public static StatusEntity CreateEntity(int id, StatusRegistrationForm form)
    {
        return new StatusEntity {Id = id, Status = Enum.TryParse<StatusStates>(form.Status, true, out var status) ? status : default };
    }
}
