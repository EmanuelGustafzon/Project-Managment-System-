using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Data.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string Firstname { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string Lastname { get; set; } = null!;
    public UserContactEntity? ContactInformation { get; set; }
    public ICollection<ProjectEntity> Projects { get; } = new List<ProjectEntity>();
}
