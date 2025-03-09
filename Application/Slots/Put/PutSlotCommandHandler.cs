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
            return Result.Failure<string>(new Error("NotFoundSlot", "Slot with the given id does not exist",
                ErrorType.NotFound));
        }

        if (slot.Schedule?.Specialist?.UserId != userContext.UserId)
        {
            return Result.Failure<string>(new Error("Unauthorized", "You are not allowed to delete this slot",
                ErrorType.Unauthorized));
        }

        if(command.StartTime < slot.Schedule.StartTime || command.StartTime > slot.Schedule.EndTime) // TODO move to validation
        {
            return Result.Failure<string>(new Error("InvalidTimeRange", "Slot time must be within the schedule time range",
                ErrorType.Conflict));
        }

        slot.StartTime = command.StartTime;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Slot updated successfully.");
    }
}
