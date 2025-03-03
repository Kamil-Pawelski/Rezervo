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
            .Include(schedule => schedule.Slots)
            .Where(schedule => schedule.SpecialistId == query.SpecialistId && schedule.Date >= today)
            .Where(schedule => schedule.Slots.Any(slot => slot.Status == Status.Available))
            .Select(schedule => new ScheduleDateResponse
            {
                ScheduleId = schedule.Id,
                Date = schedule.Date
            })
            .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<ScheduleDateResponse>>(new Error("ScheduleNotFound",
                "There are no slots for the specialist.", ErrorType.NotFound));
        }

        return Result.Success(result);
    }
}
