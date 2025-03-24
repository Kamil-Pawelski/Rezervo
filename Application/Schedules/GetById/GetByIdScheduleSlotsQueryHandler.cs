using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.GetById;

public sealed class GetByIdScheduleSlotsQueryHandler(ISlotRepository slotRepository) : IQueryHandler<GetByIdScheduleSlotsQuery, List<SlotResponse>>
{
    public async Task<Result<List<SlotResponse>>> Handle(GetByIdScheduleSlotsQuery query, CancellationToken cancellationToken)
    {
        List<Slot> result = await slotRepository.GetScheduleSlotsAsync(query.ScheduleId, cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<SlotResponse>>(SlotErrors.NotFoundSlots);
        }

        return Result.Success(result.MapToSlotResponseList());
    }
}
