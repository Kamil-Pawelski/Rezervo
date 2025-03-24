using Application.Bookings;
using Domain.Bookings;

namespace Application.Mapper;

public static class MapBooking
{
    public static BookingResponse MapToBookingResponse(this Booking booking) => new(
        booking.Id,
        booking.Slot!.Schedule!.Date.ToDateTime(booking.Slot.StartTime),
        $"{booking.Slot.Schedule.Specialist!.User!.FirstName} {booking.Slot.Schedule.Specialist.User.LastName}",
        booking.Slot.Schedule.Specialist.Specialization!.Name
    );

    public static List<BookingResponse> MapToBookingResponseList(this List<Booking> bookings) => [.. bookings
        .Select(booking => new BookingResponse(
                booking.Id,
                booking.Slot!.Schedule!.Date.ToDateTime(booking.Slot.StartTime),
                $"{booking.Slot.Schedule.Specialist!.User!.FirstName} {booking.Slot.Schedule.Specialist.User.LastName}",
                booking.Slot.Schedule.Specialist.Specialization!.Name
        ))];
}
