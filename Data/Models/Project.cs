using Data.Interfaces;

namespace Data.Models;

internal class Project : IProject
{
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ProjectManager { get; set; }
    public int Customer { get; set; }
    public string Service { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
}
