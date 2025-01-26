using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class StatusEntity
{
    public int Id { get; set; }

    [EnumDataType(typeof(StatusStates))]
    public StatusStates Status { get; set; }
    public ICollection<ProjectEntity> Projects { get; } = new List<ProjectEntity>();
}
