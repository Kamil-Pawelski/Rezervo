using Domain.Common;

namespace Domain.Slots;

public static class SlotErrors
{
    public static Error SlotAlreadyExist(TimeOnly startTime) => Error.Conflict("SlotAlreadyExist", $"A slot already exists for the time {startTime}. Please choose a different time.");

    public static readonly Error NotFoundSlot = Error.NotFound("NotFoundSlot", "Slot with the specified ID does not exist.");
    public static readonly Error NotFoundSlots = Error.NotFound("NotFoundSlots", "There are no slots available on this day.");
    public static readonly Error InvalidTimeRange = Error.Conflict("InvalidTimeRange", "Slot time must be within the schedule time range.");
}
