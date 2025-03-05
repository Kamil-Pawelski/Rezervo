using Domain.Bookings;
using Domain.Schedules;
using Domain.Slots;
using Domain.Specialists;
using Domain.Users;
using Domain.Specializations;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Specialization> Specializations { get; }
    DbSet<Specialist> Specialists { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Schedule> Schedules { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Slot> Slots { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
