using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.Get;

public sealed class GetScheduleQueryHandler(IApplicationDbContext context) : IQueryHandler<GetScheduleQuery, List<ScheduleDateResponse>>
{
    public async Task<Result<List<ScheduleDateResponse>>> Handle(GetScheduleQuery query, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        List<ScheduleDateResponse> result = await context.Schedules
            .Where(schedule => schedule.SpecialistId == query.SpecialistId && schedule.Date >= today)
            .Select(schedule => new
            {
                Schedule = schedule,
                HasAvailableSlots = schedule.Slots.Any(slot => slot.Status == Status.Available)
            })
            .Where(a => a.HasAvailableSlots)
            .Select(a => new ScheduleDateResponse
            {
                ScheduleId = a.Schedule.Id,
                Date = a.Schedule.Date
            })
            .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<ScheduleDateResponse>>(ScheduleErrors.NoAvailableSlots);
        }

        return Result.Success(result);
    }
}
