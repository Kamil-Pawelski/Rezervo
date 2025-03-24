using Application.Abstractions.Repositories;
using Domain.Bookings;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await context.Bookings.AddAsync(booking, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Booking booking, CancellationToken cancellationToken)
    {
        context.Bookings.Remove(booking);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<List<Booking>> GetAllAsync(Guid userId, CancellationToken cancellationToken) => await context.Bookings
        .Where(booking => booking.UserId == userId)
        .Include(booking => booking.Slot)
            .ThenInclude(slot => slot!.Schedule)
            .ThenInclude(schedule => schedule!.Specialist)
            .ThenInclude(specialist => specialist!.User)
        .Include(booking => booking.Slot)
            .ThenInclude(slot => slot!.Schedule)
            .ThenInclude(schedule => schedule!.Specialist)
            .ThenInclude(specialist => specialist!.Specialization)
        .ToListAsync(cancellationToken);
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await context.Bookings
        .Include(booking => booking.Slot)
            .ThenInclude(slot => slot!.Schedule)
            .ThenInclude(schedule => schedule!.Specialist)
            .ThenInclude(specialist => specialist!.User)
        .Include(booking => booking.Slot)
            .ThenInclude(slot => slot!.Schedule)
            .ThenInclude(schedule => schedule!.Specialist)
            .ThenInclude(specialist => specialist!.Specialization)
        .FirstOrDefaultAsync(booking => booking.Id == id ,cancellationToken);
}
