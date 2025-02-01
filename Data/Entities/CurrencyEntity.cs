namespace Data.Entities;

public class CurrencyEntity
{
    public int Id { get; set; }
    public string Currency { get; set; } = null!;

    public ICollection<ServiceEntity> Projects { get; } = new List<ServiceEntity>();
}
