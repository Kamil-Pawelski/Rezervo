using System.ComponentModel.DataAnnotations;
using Domain.Schedules;
using Domain.Users;
using Domain.Specializations;

namespace Domain.Specialists;
public sealed class Specialist
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid SpecializationId { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
    [Required]
    public required string City { get; set; }

    public User? User { get; set; }
    public Specialization? Specialization { get; set; }
    public ICollection<Schedule> Schedules { get; set; } = [];
}
