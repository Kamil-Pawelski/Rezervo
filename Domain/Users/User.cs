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
    public string Username { get; set; }
    [Required]
    [MaxLength(64)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(32)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; }


    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
