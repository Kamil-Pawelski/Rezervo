using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Bookings;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Bookings.Create;

public sealed class CreateBookingCommandHandler(ISlotRepository slotRepository, IBookingRepository bookingRepository, IUserContext userContext) : ICommandHandler<CreateBookingCommand, string>
{
    public async Task<Result<string>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {

        Slot? slot = await slotRepository.GetByIdAsync(command.SlotId, cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(SlotErrors.NotFoundSlot);
        }

        slot.Status = Status.Booked;

        var booking = new Booking
        {
            UserId = userContext.UserId,
            SlotId = command.SlotId
        };

        await bookingRepository.AddAsync(booking, cancellationToken);
        await slotRepository.UpdateAsync(slot, cancellationToken);

        return Result.Success("Booking created.");
    }
}
