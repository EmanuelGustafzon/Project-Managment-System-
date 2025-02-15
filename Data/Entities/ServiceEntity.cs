using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("Services")]
public class ServiceEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar(800)")]
    public string? Description { get; set; }

    [Column(TypeName = "Money")]
    public decimal Price { get; set; }

    public int CurrencyId { get; set; }

    public CurrencyEntity Currency { get; set; } = null!;

    [EnumDataType(typeof(Units))]
    public Units Unit { get; set; } 
    public ICollection<ProjectEntity> Projects { get; } = new List<ProjectEntity>();
}
