using Domain.Specialists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Specialists;

internal sealed class SpecialistConfiguration : IEntityTypeConfiguration<Specialist>
{
    public void Configure(EntityTypeBuilder<Specialist> builder)
    {
        builder.HasKey(specialist => specialist.Id);
        builder.HasIndex(specialist => specialist.UserId).IsUnique();
        builder.Property(specialist => specialist.UserId).IsRequired();
        builder.Property(specialist => specialist.SpecializationId).IsRequired();
        builder.Property(specialist => specialist.PhoneNumber).IsRequired();
        builder.Property(specialist => specialist.City).IsRequired();
        builder.HasOne(specialist => specialist.Specialization)
            .WithMany(specialization => specialization.Specialists)
            .HasForeignKey(specialist => specialist.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(specialist => specialist.User)
            .WithMany(user => user.Specialists)
            .HasForeignKey(specialist => specialist.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
