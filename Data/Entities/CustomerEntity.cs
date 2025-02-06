using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class CustomerEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string? OrgansisationNumber { get; set; } 
    public ICollection<ProjectEntity> Projects { get; } = new List<ProjectEntity>();
}
