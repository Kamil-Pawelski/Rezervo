using Domain.Common;

namespace Domain.Schedules;

public static class ScheduleErrors
{
    public static readonly Error NotFoundSchedule = Error.NotFound("NotFoundSchedule", "Schedule with the specified ID does not exist.");
    public static readonly Error NoAvailableSlots = Error.NotFound("NoAvailableSlots", "The specialist has no available slots or does not have a schedule.");
}
