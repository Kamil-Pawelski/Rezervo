using System.ComponentModel.DataAnnotations;
using Domain.Bookings;
using Domain.Specialists;

namespace Domain.Users;

public sealed class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }


    public ICollection<UserRole> UserRoles { get; set; } = [];
    public Specialist? Specialist { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
}
