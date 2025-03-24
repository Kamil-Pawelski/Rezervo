using Application.Schedules;
using Domain.Schedules;

namespace Application.Mapper;

public static class MapSchedule
{
    public static List<ScheduleResponse> MapToScheduleResponse(this List<Schedule> schedules) => [.. schedules
          .Where(schedule => schedule.Slots.Any(slot => slot.Status == Status.Available))
          .Select(schedule => new ScheduleResponse(schedule.Id, schedule.Date))];
}
