using System.ComponentModel.DataAnnotations;
using Domain.Schedules;
using Domain.Users;
using Domain.Specializations;

namespace Domain.Specialists;
public sealed class Specialist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SpecializationId { get; set; }
    public required string Description { get; set; }
    public required string PhoneNumber { get; set; }
    public required string City { get; set; }

    public User? User { get; set; }
    public Specialization? Specialization { get; set; }
    public ICollection<Schedule> Schedules { get; set; } = [];
}
