using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Bookings.GetById;

public sealed class GetByIdBookingQueryHandler(IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetByIdBookingQuery, BookingResponse>
{
    public async Task<Result<BookingResponse>> Handle(GetByIdBookingQuery query, CancellationToken cancellationToken)
    {
        BookingResponse? result = await context.Bookings
            .Where(booking => booking.UserId == userContext.UserId && booking.Id == query.Id)
            .Select(booking => new BookingResponse(
                booking.Id,
                booking.Slot!.Schedule!.Date.ToDateTime(booking.Slot.StartTime),
                $"{booking.Slot!.Schedule!.Specialist!.User!.FirstName} {booking.Slot.Schedule.Specialist.User.LastName}",
                booking.Slot.Schedule.Specialist.Specialization!.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return Result.Failure<BookingResponse>(BookingErrors.NotFoundBooking);
        }

        return Result.Success(result);
    }
}
