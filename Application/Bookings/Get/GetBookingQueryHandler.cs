using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Bookings;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Bookings.Get;

public sealed class GetBookingQueryHandler(IBookingRepository bookingRepository, IUserContext userContext) : IQueryHandler<GetBookingQuery, List<BookingResponse>>
{
    public async Task<Result<List<BookingResponse>>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        List<Booking> result = await bookingRepository.GetAllAsync(userContext.UserId, cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<BookingResponse>>(BookingErrors.NotFoundBookings);
        }

        return Result.Success(result.MapToBookingResponseList());
    }
}
