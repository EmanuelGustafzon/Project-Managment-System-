using Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ProjectManager { get; set; }
    public int Customer { get; set; }

    [MaxLength(100)]
    public string Service { get; set; } = null!;
    public decimal TotalPrice { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = null!;
}
