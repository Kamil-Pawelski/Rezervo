using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Bookings.Create;

public sealed class CreateBookingCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<CreateBookingCommand, string>
{
    public async Task<Result<string>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {

        Slot? slot = await context.Slots.FindAsync([command.SlotId], cancellationToken);

        if (slot is null)
        {
            return Result.Failure<string>(new Error("NotFoundSlot", "Slot with the given id does not exist",
                ErrorType.NotFound));
        }

        slot.Status = Status.Booked;

        await context.Bookings.AddAsync(new Booking
        {
            UserId = userContext.UserId,
            SlotId = command.SlotId
        }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking created.");
    }
}
