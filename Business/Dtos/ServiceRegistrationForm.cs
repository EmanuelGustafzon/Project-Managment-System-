using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ServiceRegistrationForm
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }

    [Required]
    public string Unit { get; set; } = null!;
}
