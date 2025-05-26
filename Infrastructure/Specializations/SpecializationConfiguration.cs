using Domain.Specializations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Specializations;

internal sealed class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.HasKey(specialization => specialization.Id);

        builder.HasIndex(specialization => specialization.Name)
            .IsUnique();

        builder.Property(specialization => specialization.Name)
            .IsRequired();
    }
}
