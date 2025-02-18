using System.ComponentModel.DataAnnotations;
using Domain.Bookings;
using Domain.Specialists;

namespace Domain.Schedules;
public sealed class Schedule
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid SpecialistId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    [Required]
    public Status Status { get; set; }

    public Specialist Specialist { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
