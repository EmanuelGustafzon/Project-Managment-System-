namespace Business.Dtos;

public class ContactDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
}
