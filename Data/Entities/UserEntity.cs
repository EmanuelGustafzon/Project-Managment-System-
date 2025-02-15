using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Data.Entities;

[Table("Users")]
public class UserEntity
{
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string Firstname { get; set; } = null!;
    [Column(TypeName = "nvarchar(50)")]
    public string Lastname { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string Email { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; } = new List<ProjectEntity>();

}
