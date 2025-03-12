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
            return Result.Failure<string>(new Error("NotFoundBooking", "Booking with the given id does not exist",
                ErrorType.NotFound));
        }

        if (booking.UserId != userContext.UserId)
        {
            return Result.Failure<string>(new Error("Unauthorized", "You are not authorized to delete this booking",
                ErrorType.Unauthorized));
        }

        Slot? slot = await context.Slots.FindAsync([booking.SlotId], cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(new Error("NotFoundSlot", "Slot with the given id does not exist",
                ErrorType.NotFound));
        }

        slot.Status = Status.Cancelled;

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking deleted successfully.");
    }
}
