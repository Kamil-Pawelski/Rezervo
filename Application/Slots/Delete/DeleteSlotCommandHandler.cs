using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Slots.Delete;

public sealed class DeleteSlotCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteSlotCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSlotCommand command, CancellationToken cancellationToken)
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

        context.Slots.Remove(slot);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Slot deleted successfully.");
    }
}

