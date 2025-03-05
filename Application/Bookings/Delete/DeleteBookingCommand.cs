using Application.Abstractions.Messaging;

namespace Application.Bookings.Delete;

public sealed record DeleteBookingCommand(Guid Id) : ICommand<string>;

