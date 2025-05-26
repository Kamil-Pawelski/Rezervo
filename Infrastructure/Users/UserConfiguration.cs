using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) 
    {
        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.Username)
            .IsUnique();

        builder.Property(user => user.Email)
            .IsRequired();

        builder.Property(user => user.Username)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .IsRequired();

        builder.Property(user => user.LastName)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .IsRequired();
    }
}
