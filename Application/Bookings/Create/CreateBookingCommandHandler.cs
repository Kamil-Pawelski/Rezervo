using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.Common;

namespace Application.Bookings.Create;

public sealed class CreateBookingCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateBookingCommand, string>
{
    public async Task<Result<string>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        await context.Bookings.AddAsync(new Booking
        {
            UserId = command.UserId,
            SlotId = command.SlotId
        }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking created.");
    }
}
