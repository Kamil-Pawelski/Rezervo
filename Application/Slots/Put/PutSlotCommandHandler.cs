using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Slots.Put;

public sealed class PutSlotCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<PutSlotCommand, string>
{
    public async Task<Result<string>> Handle(PutSlotCommand command, CancellationToken cancellationToken)
    {
        Slot? slot = await context.Slots
            .Include(slot => slot.Schedule)
            .ThenInclude(schedule => schedule!.Specialist)
            .FirstOrDefaultAsync(slot => slot.Id == command.Id, cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(SlotErrors.NotFoundSlot);
        }

        if (slot.Schedule?.Specialist?.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        if(command.StartTime < slot.Schedule.StartTime || command.StartTime > slot.Schedule.EndTime)
        {
            return Result.Failure<string>(SlotErrors.InvalidTimeRange);
        }

        slot.StartTime = command.StartTime;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Slot updated successfully.");
    }
}
