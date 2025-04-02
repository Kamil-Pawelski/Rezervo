using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) 
    {
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.Username).IsUnique();
        builder.Property(user => user.Email).IsRequired().HasMaxLength(64);
        builder.Property(user => user.Username).IsRequired().HasMaxLength(32);
        builder.Property(user => user.FirstName).IsRequired().HasMaxLength(32);
        builder.Property(user => user.LastName).IsRequired().HasMaxLength(32);
        builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(255);

    }
}
