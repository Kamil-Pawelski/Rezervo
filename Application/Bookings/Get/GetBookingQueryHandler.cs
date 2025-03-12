using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Bookings;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Bookings.Get;

public sealed class GetBookingQueryHandler(IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetBookingQuery, List<BookingResponse>>
{
    public async Task<Result<List<BookingResponse>>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        List<BookingResponse> result = await context.Bookings
               .Where(booking => booking.UserId == userContext.UserId)
               .Select(booking => new BookingResponse(
                   booking.Id,
                   booking.Slot!.Schedule!.Date.ToDateTime(booking.Slot.StartTime),
                   $"{booking.Slot.Schedule.Specialist!.User!.FirstName} {booking.Slot.Schedule.Specialist.User.LastName}",
                   booking.Slot.Schedule.Specialist.Specialization!.Name))
               .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<BookingResponse>>(BookingErrors.NotFoundBookings);
        }

        return Result.Success(result);
    }
}
