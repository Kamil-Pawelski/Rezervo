using System.ComponentModel.DataAnnotations;
using Domain.Bookings;
using Domain.Specialists;

namespace Domain.Users;

public sealed class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(32)]
    public required string Username { get; set; }
    [Required]
    [MaxLength(64)]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MaxLength(32)]
    public required string FirstName { get; set; }
    [Required]
    [MaxLength(32)]
    public required string LastName { get; set; }
    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }


    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Specialist> Specialists { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}
