using Application.Abstractions.Data;
using Domain.Bookings;
using Domain.Schedule;
using Domain.Specialists;
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

        modelBuilder.Entity<User>()
            .HasKey(user => user.Id);

        modelBuilder.Entity<Role>()
            .HasKey(role => role.Id);

        modelBuilder.Entity<UserRole>()
            .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

        modelBuilder.Entity<Specialist>()
            .HasKey(specialist => specialist.Id);

        modelBuilder.Entity<Specialization>()
            .HasKey(specialization => specialization.Id);

        modelBuilder.Entity<Schedule>()
            .HasKey(schedule => schedule.Id);

        modelBuilder.Entity<Booking>()
            .HasKey(booking => booking.Id);


        /*modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = RolesConstants.User },
                new Role { Id = 2, Name = RolesConstants.Specialist }
                new Role { Id = 3, Name = RolesConstants.Administrator }
            );*/
    }
}
