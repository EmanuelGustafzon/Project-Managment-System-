using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserRegistrationForm
{
    [Required]
    public string Firstname { get; set; } = null!;
    [Required]
    public string Lastname { get; set; } = null!;
    public ContactRegistrationForm? ContactInformation { get; set; }
}
