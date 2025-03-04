using System.ComponentModel.DataAnnotations;
using Domain.Schedules;

namespace Domain.Slots;

public sealed class Slot
{
    [Key]
    public Guid Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public Status Status { get; set; }
    public Guid ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }
}
