using Application.Abstractions.Repositories;
using Domain.Slots;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class SlotRepository(ApplicationDbContext context) : ISlotRepository
{
    public async Task AddAsync(Slot slot, CancellationToken cancellationToken)
    {
        await context.Slots.AddAsync(slot, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Slot slot, CancellationToken cancellationToken) 
    {
        context.Slots.Remove(slot);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteSlotsAsync(List<Slot> slots, CancellationToken cancellationToken)
    {
        context.Slots.RemoveRange(slots);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<List<Slot>> GetAllAsync(CancellationToken cancellationToken) => await context.Slots.ToListAsync(cancellationToken);
    public async Task<Slot?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await context.Slots.Where(slot => slot.Id == id).FirstOrDefaultAsync(cancellationToken);
    public async Task<List<Slot>> GetScheduleSlotsAsync(Guid scheduleId, CancellationToken cancellationToken) => await context.Slots.Where(slot => slot.ScheduleId == scheduleId).ToListAsync(cancellationToken);

    public Task UpdateAsync(Slot slot, CancellationToken cancellationToken)
    {
        context.Slots.Update(slot);
        return context.SaveChangesAsync(cancellationToken);
    }
}
