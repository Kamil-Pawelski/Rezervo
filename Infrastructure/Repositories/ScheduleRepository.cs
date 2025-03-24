using Application.Abstractions.Repositories;
using Domain.Schedules;
using Domain.Slots;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ScheduleRepository(ApplicationDbContext context) : IScheduleRepository
{
    public async Task AddAsync(Schedule schedule, CancellationToken cancellationToken)
    {
        await context.Schedules.AddAsync(schedule, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Schedule schedule, CancellationToken cancellationToken)
    {
        context.Schedules.Remove(schedule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Schedule>> GetAllAsync(DateOnly today, CancellationToken cancellationToken) => await context.Schedules
            .Include(schedule => schedule.Slots)
            .Where(schedule => schedule.Date >= today)
            .ToListAsync(cancellationToken);

    public async Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await context.Schedules
        .Include(schedule => schedule.Specialist)
        .FirstOrDefaultAsync(schedule => schedule.Id == id, cancellationToken);
    public async Task<List<Schedule>> GetBySpecialistAsync(Guid specialistId, DateOnly today, CancellationToken cancellationToken) => await context.Schedules
        .Include(schedule => schedule.Slots)
        .Where(schedule => schedule.SpecialistId == specialistId && schedule.Date >= today)
        .ToListAsync(cancellationToken);

    public async Task UpdateAsync(Schedule schedule, CancellationToken cancellationToken)
    {
        context.Schedules.Update(schedule);
        await context.SaveChangesAsync(cancellationToken);
    }
}
