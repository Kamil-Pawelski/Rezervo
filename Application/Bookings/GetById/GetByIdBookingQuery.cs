using Application.Abstractions.Messaging;
using Application.Bookings.Get;

namespace Application.Bookings.GetById;

public sealed record GetByIdBookingQuery(Guid Id) : IQuery<BookingResponse>;
