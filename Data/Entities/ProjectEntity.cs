using Data.Enums;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;
    [Column(TypeName = "Date")]
    public DateTime StartTime { get; set; }
    [Column(TypeName = "Date")]
    public DateTime EndTime { get; set; }

    [Column(TypeName = "Money")]
    public decimal TotalPrice { get; set; }
    public int ProjectManagerId { get; set; }
    public UserEntity ProjectManager { get; set; } = null!;
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public int ServiceId { get; set; } 
    public ServiceEntity Service { get; set; } = null!;
    public StatusStates Status { get; set; }
}
