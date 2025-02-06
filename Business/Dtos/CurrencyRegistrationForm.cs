using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CurrencyRegistrationForm
{
    [Required]
    [MaxLength(20)]
    public string Currency { get; set; } = null!;
}

