namespace Data.Interfaces;

public interface IProject
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ProjectManager { get; set; }
    public int Customer { get; set; }
    public string Service { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
}