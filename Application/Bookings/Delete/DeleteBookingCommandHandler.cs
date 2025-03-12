using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Bookings.Delete;

public sealed class DeleteBookingCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteBookingCommand, string>
{
    public async Task<Result<string>> Handle(DeleteBookingCommand command, CancellationToken cancellationToken)
    {
        Booking? booking = await context.Bookings.FindAsync([command.Id], cancellationToken);

        if (booking is null)
        {
            return Result.Failure<string>(BookingErrors.NotFoundBooking);
        }

        if (booking.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        Slot? slot = await context.Slots.FindAsync([booking.SlotId], cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(SlotErrors.NotFoundSlot);
        }

        slot.Status = Status.Cancelled;

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking deleted successfully.");
    }
}
