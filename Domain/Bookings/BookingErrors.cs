using Domain.Common;

namespace Domain.Bookings;


public static class BookingErrors
{
    public static readonly Error NotFoundBooking = Error.NotFound("NotFoundBooking", "Booking with the specified ID does not exist.");
    public static readonly Error NotFoundBookings = Error.NotFound("NotFoundBookings", "You don't have any bookings.");
}
