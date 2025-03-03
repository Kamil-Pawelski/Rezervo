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
    public TimeOnly StartTime { get; set; }
    [Required]
    public TimeOnly EndTime { get; set; }
    [Required]
    public DateOnly Date { get; set; }

    public Specialist? Specialist { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Slot> Slots { get; set; } = [];

    public void GenerateSlots(int slotDuration)
    {
        TimeOnly currentTime = StartTime;

        while (currentTime < EndTime)
        {
            Slots.Add(new Slot
            {
                Id = Guid.NewGuid(),
                ScheduleId = Id,
                StartTime = currentTime,
                Status = Status.Available
            });
            currentTime = currentTime.AddMinutes(slotDuration);
        }
    }

}
