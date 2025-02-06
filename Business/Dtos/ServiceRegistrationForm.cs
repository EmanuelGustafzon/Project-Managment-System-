using Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ServiceRegistrationForm
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(800)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }

    [Required]
    [TypeConverter(typeof(Units))]
    public string Unit { get; set; } = null!;
}
