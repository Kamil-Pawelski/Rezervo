using Application.Abstractions.Messaging;

namespace Application.Bookings.Get;

public sealed record GetBookingQuery : IQuery<List<BookingResponse>>;
