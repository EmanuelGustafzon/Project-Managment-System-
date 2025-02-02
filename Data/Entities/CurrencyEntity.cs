using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class CurrencyEntity
{
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string Currency { get; set; } = null!;

    public ICollection<ServiceEntity> Services { get; } = new List<ServiceEntity>();
}
