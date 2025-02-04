using Data.Entities;
using Data.Enums;
using System;
namespace Business.Dtos;

public class ProjectRegistrationForm
{
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = StatusStates.NotStarted.ToString();
    public int ProjectManagerId { get; set; }
    public int ServiceId { get; set; }
    public int? CustomerId { get; set; }
    public CustomerRegistrationForm? CustomerForm { get; set; }
}
