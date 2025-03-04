using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.GetById;

public sealed class GetScheduleSlotsByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetScheduleSlotsByIdQuery, List<SlotResponse>>
{
    public async Task<Result<List<SlotResponse>>> Handle(GetScheduleSlotsByIdQuery query,
        CancellationToken cancellationToken)
    {
        List<SlotResponse> result = await context.Slots.Where(slot => slot.ScheduleId == query.ScheduleId && slot.Status == Status.Available)
            .Select(slot => new SlotResponse
            {
                Id = slot.Id,
                StartTime = slot.StartTime,
            })
            .OrderBy(slot => slot.StartTime)
            .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<SlotResponse>>(new Error("NotSlotsFound",
                "There are not slots available on this day.", ErrorType.NotFound));
        }

        return Result.Success(result);
    }
}
