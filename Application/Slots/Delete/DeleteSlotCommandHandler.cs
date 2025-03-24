using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Slots;

namespace Application.Slots.Delete;

public sealed class DeleteSlotCommandHandler(ISlotRepository slotRepository, IUserContext userContext) : ICommandHandler<DeleteSlotCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSlotCommand command, CancellationToken cancellationToken)
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

        await slotRepository.DeleteAsync(slot, cancellationToken);

        return Result.Success("Slot deleted successfully.");
    }
}

