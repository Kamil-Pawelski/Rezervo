using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.GetById;

public sealed class GetScheduleSlotsByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetScheduleSlotsByIdQuery, List<SlotResponse>>
{
    public Task<Result<List<SlotResponse>>> Handle(GetScheduleSlotsByIdQuery query,
        CancellationToken cancellationToken)
    {
        Task<List<SlotResponse>> result = context.Schedules
            .Include(schedule => schedule.Slots)
            .Where(schedule => schedule.Id == query.ScheduleId)
            .SelectMany(schedule => schedule.Slots)
            .Select(slot => new SlotResponse
            {
                Id = slot.Id,
                StartTime = slot.StartTime,
            })
            .ToListAsync(cancellationToken);
    }
}
