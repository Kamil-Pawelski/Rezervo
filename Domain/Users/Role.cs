using System.ComponentModel.DataAnnotations;

namespace Domain.Users;
public sealed class Role
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Key]
    [MaxLength(32)]
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
