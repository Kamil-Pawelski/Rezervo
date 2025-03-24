using System.ComponentModel.DataAnnotations;

namespace Domain.Users;
public sealed class UserRole
{
    [Key]
    [Required]
    public Guid UserId { get; set; }
    [Key]
    [Required]
    public Guid RoleId { get; set; }

    public User? User { get; set; }
    public Role? Role { get; set; }
}
