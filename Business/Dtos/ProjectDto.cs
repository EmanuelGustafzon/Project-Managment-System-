namespace Business.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = null!;
    public UserDto ProjectManager { get; set; } = null!;
    public ServiceDto Service { get; set; } = null!;
    public CustomerDto Customer { get; set; } = null!;
}
