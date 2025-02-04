namespace Business.Dtos;

public class ProjectDto
{
    public string Name { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Status { get; set; } = null!;

    public int ProjectManagerId { get; set; }

    public int CustomerId { get; set; }

    public int ServiceId { get; set; }
}
