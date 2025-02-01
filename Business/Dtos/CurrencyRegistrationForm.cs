using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CurrencyRegistrationForm
{
    [Required]
    public string Currency { get; set; } = null!;
}

