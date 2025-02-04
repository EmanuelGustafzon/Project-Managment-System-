using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserContactEntity : BaseContactEntity
{
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}
