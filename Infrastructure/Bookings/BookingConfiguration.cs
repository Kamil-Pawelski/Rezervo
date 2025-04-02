using Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Bookings;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(booking => booking.Id);
        builder.HasIndex(booking => booking.SlotId).IsUnique();
        builder.Property(booking => booking.UserId).IsRequired();
        builder.Property(booking => booking.SlotId).IsRequired();
        builder.Property(booking => booking.CreatedDateTime).IsRequired();
        builder.HasOne(booking => booking.User)
            .WithMany(user => user.Bookings)
            .HasForeignKey(booking => booking.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(booking => booking.Slot)
            .WithMany(slot => slot.Bookings)
            .HasForeignKey(booking => booking.SlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
