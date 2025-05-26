using System.ComponentModel.DataAnnotations;
using Domain.Bookings;
using Domain.Specialists;
using Domain.Slots;

namespace Domain.Schedules;
public sealed class Schedule
{
    public Guid Id { get; set; }
    public Guid SpecialistId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
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
