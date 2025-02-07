using Data.Entities;
using Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Business.Dtos;

public class ProjectRegistrationForm
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [TypeConverter(typeof(StatusStates))]
    public string Status { get; set; } = StatusStates.NotStarted.ToString();
    public int ProjectManagerId { get; set; }
    public decimal? TotalPrice { get; set; }
    public int? ServiceId { get; set; }
    public ServiceRegistrationForm? ServiceForm { get; set; }
    public int? CustomerId { get; set; }
    public CustomerRegistrationForm? CustomerForm { get; set; }
}
