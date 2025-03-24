using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Bookings;
using Domain.Common;

namespace Application.Bookings.GetById;

public sealed class GetByIdBookingQueryHandler(IBookingRepository bookingRepository, IUserContext userContext) : IQueryHandler<GetByIdBookingQuery, BookingResponse>
{
    public async Task<Result<BookingResponse>> Handle(GetByIdBookingQuery query, CancellationToken cancellationToken)
    {    
        Booking? result = await bookingRepository.GetByIdAsync(query.Id, cancellationToken);

        if (result is null)
        {
            return Result.Failure<BookingResponse>(BookingErrors.NotFoundBooking);
        }

        if (userContext.UserId != result.UserId)
        {
            return Result.Failure<BookingResponse>(CommonErrors.Unauthorized);
        }

        return Result.Success(result.MapToBookingResponse());
    }
}
