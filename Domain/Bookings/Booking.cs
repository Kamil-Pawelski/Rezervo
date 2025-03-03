using System.ComponentModel.DataAnnotations;
using Domain.Users;
using Domain.Schedules;

namespace Domain.Bookings;
public sealed class Booking
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid ScheduleId { get; set; }
    [Required] 
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Schedule Schedule { get; set; }
}
