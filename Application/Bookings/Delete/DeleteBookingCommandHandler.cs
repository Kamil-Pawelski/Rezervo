using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Bookings;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Bookings.Delete;

public sealed class DeleteBookingCommandHandler(ISlotRepository slotRepository, IBookingRepository bookingRepository, IUserContext userContext) : ICommandHandler<DeleteBookingCommand, string>
{
    public async Task<Result<string>> Handle(DeleteBookingCommand command, CancellationToken cancellationToken)
    {
        Booking? booking = await bookingRepository.GetByIdAsync(command.Id, cancellationToken);

        if (booking is null)
        {
            return Result.Failure<string>(BookingErrors.NotFoundBooking);
        }

        if (booking.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        Slot? slot = await slotRepository.GetByIdAsync(booking.SlotId, cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(SlotErrors.NotFoundSlot);
        }

        slot.Status = Status.Cancelled;

        await bookingRepository.DeleteAsync(booking, cancellationToken);
        await slotRepository.UpdateAsync(slot, cancellationToken);

        return Result.Success("Booking deleted successfully.");
    }
}
