using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Slots;

namespace Application.Slots.Put;

public sealed class PutSlotCommandHandler(ISlotRepository slotRepository, IUserContext userContext) : ICommandHandler<PutSlotCommand, string>
{
    public async Task<Result<string>> Handle(PutSlotCommand command, CancellationToken cancellationToken)
    {
        Slot? slot = await slotRepository.GetByIdAsync(command.Id, cancellationToken);

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
        await slotRepository.UpdateAsync(slot, cancellationToken);

        return Result.Success("Slot updated successfully.");
    }
}
