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
            entity.Property(specialist => specialist.City).IsRequired();

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
            entity.Property(schedule => schedule.Date).IsRequired();

            entity.HasOne(schedule => schedule.Specialist)
                .WithMany(specialist => specialist.Schedules)
                .HasForeignKey(schedule => schedule.SpecialistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.Slots)
                .WithOne(s => s.Schedule)
                .HasForeignKey(s => s.ScheduleId) 
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.Property(slot => slot.ScheduleId).IsRequired();
            entity.Property(slot => slot.StartTime).IsRequired();
            entity.Property(slot => slot.Status).IsRequired();

        });
        

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.Id);
            entity.HasIndex(booking => booking.ScheduleId).IsUnique();
            entity.Property(booking => booking.UserId).IsRequired();
            entity.Property(booking => booking.ScheduleId).IsRequired();
            entity.Property(booking => booking.CreatedDateTime).IsRequired();

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

        modelBuilder.Entity<Specialization>()
            .HasData(
                new Specialization { Id = new Guid("a3f5d1b2-6c3e-4b99-8e1a-9f6d4c7b2e3f"), Name = SpecializationNames.Plumber },
                new Specialization { Id = new Guid("b7e2c4d9-1a5f-4c88-97e3-d6a9b5f4c1e8"), Name = SpecializationNames.Hairdresser },
                new Specialization { Id = new Guid("c1d4f5e6-3b7a-4c99-8e2a-5d9b6f4c7a3e"), Name = SpecializationNames.Electrician },
                new Specialization { Id = new Guid("d6f2b3c4-5e7a-4c99-8d1a-9b4f7e6c5a3d"), Name = SpecializationNames.Carpenter },
                new Specialization { Id = new Guid("e1a3d5f7-6c4b-4e99-8d2a-9f5c7b2a3d6e"), Name = SpecializationNames.Mechanic },
                new Specialization { Id = new Guid("f5c7a3b2-6d4e-4e99-8d1a-9b6f2c4d7a3e"), Name = SpecializationNames.Painter },
                new Specialization { Id = new Guid("9b6f2c4d-7a3e-4e99-8d1a-5c7a3b2d6f4e"), Name = SpecializationNames.Masseur }
            );

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);
}
