using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerRegistrationForm
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string? OrganisationNumber { get; set; }
}
