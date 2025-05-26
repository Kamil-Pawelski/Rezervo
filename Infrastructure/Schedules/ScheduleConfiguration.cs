using Domain.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Schedules;

internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(schedule => schedule.Id);

        builder.Property(schedule => schedule.SpecialistId)
            .IsRequired();

        builder.Property(schedule => schedule.StartTime)
            .IsRequired();

        builder.Property(schedule => schedule.EndTime)
            .IsRequired();

        builder.Property(schedule => schedule.Date)
            .IsRequired();
    }
}
