using Application.Schedules;
using Domain.Slots;

namespace Application.Mapper;

public static class MapSlot
{
    public static List<SlotResponse> MapToSlotResponseList(this List<Slot> slots) => [.. slots
            .OrderBy(slot => slot.StartTime)
            .Select(slot => new SlotResponse(slot.Id, slot.StartTime))];
}
