using Application.Abstractions.Messaging;

namespace Application.Bookings.Create;

public sealed record CreateBookingCommand(Guid SlotId) : ICommand<string>;
