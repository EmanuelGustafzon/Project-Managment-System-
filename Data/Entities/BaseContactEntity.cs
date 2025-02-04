using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public abstract class BaseContactEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(15)")]
    public string PhoneNumber { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string Email { get; set; } = null!;

}
