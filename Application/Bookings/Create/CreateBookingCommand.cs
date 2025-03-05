using Application.Abstractions.Messaging;

namespace Application.Bookings.Create;

public sealed record CreateBookingCommand(Guid UserId, Guid SlotId) : ICommand<string>;
