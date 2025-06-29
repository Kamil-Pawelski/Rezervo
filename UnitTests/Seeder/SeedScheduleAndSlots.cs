using Domain.Schedules;
using Domain.Slots;
using Infrastructure.Database;

namespace Tests;

public static class SeedScheduleAndSlots
{
    public static readonly Guid TestScheduleId = Guid.NewGuid();
    public static readonly Guid TestScheduleToDeleteId = Guid.NewGuid();

    public static readonly Guid TestSlotId = Guid.NewGuid();
    public static readonly TimeOnly TestSlotStartTime = new(9, 0, 0);

    public static readonly Guid TestSlotToDeleteId = Guid.NewGuid();
    public static readonly TimeOnly TestSlotToDeleteStartTime = new(12, 0, 0);

    public static readonly Guid TestSlotForBookingId = Guid.NewGuid();
    public static readonly Guid TestSlotForBooking2Id = Guid.NewGuid();

    public static void Seed(ApplicationDbContext dbContext)
    {
        var schedule = new Schedule()
        {
            Id = TestScheduleId,
            SpecialistId = SeedSpecialist.TestSpecialistId,
            StartTime = new TimeOnly(8, 0, 0),
            EndTime = new TimeOnly(16, 0, 0),
            Date = DateOnly.FromDateTime(DateTime.Now)
        };

        var scheduleToDelete = new Schedule()
        {
            Id = TestScheduleToDeleteId,
            SpecialistId = SeedSpecialist.TestSpecialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };

        var slot = new Slot
        {
            Id = TestSlotId,
            ScheduleId = TestScheduleId,
            StartTime = TestSlotStartTime,
            Status = Status.Available
        };

        var slotForBooking = new Slot
        {
            Id = TestSlotForBookingId,
            ScheduleId = TestScheduleId,
            StartTime = new TimeOnly(11, 20),
            Status = Status.Available
        };

        var slotForBooking2 = new Slot
        {
            Id = TestSlotForBooking2Id,
            ScheduleId = TestScheduleId,
            StartTime = new TimeOnly(11, 40),
            Status = Status.Available
        };

        var slotToDelete = new Slot
        {
            Id = TestSlotToDeleteId,
            ScheduleId = TestScheduleId,
            StartTime = TestSlotToDeleteStartTime,
            Status = Status.Available
        };

        dbContext.Schedules.AddRange(schedule, scheduleToDelete);
        dbContext.Slots.AddRange(slot, slotForBooking, slotForBooking2, slotToDelete);
        dbContext.SaveChanges();
    }
}
