using Application.Abstractions.Data;
using Domain.Bookings;
using Domain.Schedules;
using Domain.Specialists;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasIndex(user => user.Username).IsUnique();
            entity.Property(user => user.Email).IsRequired().HasMaxLength(64);
            entity.Property(user => user.Username).IsRequired().HasMaxLength(32);
            entity.Property(user => user.FirstName).IsRequired().HasMaxLength(32);
            entity.Property(user => user.LastName).IsRequired().HasMaxLength(32);
            entity.Property(user => user.PasswordHash).IsRequired().HasMaxLength(255);
        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(role => role.Id);
            entity.HasIndex(role => role.Name).IsUnique();
            entity.Property(role => role.Name).IsRequired().HasMaxLength(32);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(userRole => new {userRole.RoleId, userRole.UserId});

            entity.HasOne(userRole => userRole.User)
                .WithMany(user => user.UserRoles)
                .HasForeignKey(userRole => userRole.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(userRole => userRole.Role)
                .WithMany(role => role.UserRoles)
                .HasForeignKey(userRole => userRole.RoleId);
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(specialist => specialist.Id);
            entity.HasIndex(specialist => specialist.UserId).IsUnique();
            entity.Property(specialist => specialist.UserId).IsRequired();
            entity.Property(specialist => specialist.SpecializationId).IsRequired();
            entity.Property(specialist => specialist.PhoneNumber).IsRequired();

            entity.HasOne(specialist => specialist.Specialization)
                .WithMany(specialization => specialization.Specialists)
                .HasForeignKey(specialist => specialist.SpecializationId);

            entity.HasOne(specialist => specialist.User)
                .WithMany(user => user.Specialists)
                .HasForeignKey(specialist => specialist.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(specialization => specialization.Id);
            entity.HasIndex(specialization => specialization.Name).IsUnique();
            entity.Property(specialization => specialization.Name).IsRequired().HasMaxLength(32);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(schedule => schedule.Id);
            entity.Property(schedule => schedule.SpecialistId).IsRequired();
            entity.Property(schedule => schedule.StartTime).IsRequired();
            entity.Property(schedule => schedule.EndTime).IsRequired();
            entity.Property(schedule => schedule.Status).IsRequired();

            entity.HasOne(schedule => schedule.Specialist)
                .WithMany(specialist => specialist.Schedules)
                .HasForeignKey(schedule => schedule.SpecialistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.Id);
            entity.HasIndex(booking => booking.ScheduleId).IsUnique();
            entity.Property(booking => booking.UserId).IsRequired();
            entity.Property(booking => booking.ScheduleId).IsRequired();
            entity.Property(booking => booking.Created).IsRequired();

            entity.HasOne(booking => booking.User)
                .WithMany(user => user.Bookings)
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(booking => booking.Schedule)
                .WithMany(schedule => schedule.Bookings)
                .HasForeignKey(booking => booking.ScheduleId);
        });

        modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = new Guid("dc6c3733-c8b7-41fa-bfa0-b77eb710f9c3"), Name = RolesNames.Admin },
                new Role { Id = new Guid("7a4a1573-aa6e-4504-885e-bbb3a04872f5"), Name = RolesNames.Specialist },
                new Role { Id = new Guid("dd514642-f330-4950-ab3d-a3b454de9fc9"), Name = RolesNames.Client }
            );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);
}
