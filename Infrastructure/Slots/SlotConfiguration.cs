using Domain.Slots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Slots;

internal sealed class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.HasKey(slot => slot.Id);
        builder.Property(slot => slot.ScheduleId).IsRequired();
        builder.Property(slot => slot.StartTime).IsRequired();
        builder.Property(slot => slot.Status).IsRequired();
    }
}
