using System.ComponentModel.DataAnnotations;

namespace Domain.Users;
public sealed class Role
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Key]
    [MaxLength(32)]
    public required string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}

public static class RolesNames
{
    public const string Admin = "Admin";
    public const string Specialist = "Specialist";
    public const string Client = "Client";

}
