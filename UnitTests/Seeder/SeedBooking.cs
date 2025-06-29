using Domain.Bookings;
using Infrastructure.Database;

namespace Tests.Seeder;

public static class SeedBooking
{
    public static readonly Guid TestBookingId = Guid.NewGuid();
    public static readonly Guid TestBookingToDeleteId = Guid.NewGuid();

    public static void Seed(ApplicationDbContext dbContext)
    {
        var booking = new Booking
        {
            Id = TestBookingId,
            SlotId = SeedScheduleAndSlots.TestSlotForBooking2Id,
            UserId = SeedUser.TestUserId,
        };

        var bookingToDelete = new Booking
        {
            Id = TestBookingToDeleteId,
            SlotId = SeedScheduleAndSlots.TestSlotId,
            UserId = SeedUser.TestUserId,
        };

        dbContext.Bookings.AddRange(booking, bookingToDelete);
        dbContext.SaveChanges();
    }
}
