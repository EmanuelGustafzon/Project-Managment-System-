using Data.Enums;

namespace Business.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public string Unit { get; set; } = null!;
}
