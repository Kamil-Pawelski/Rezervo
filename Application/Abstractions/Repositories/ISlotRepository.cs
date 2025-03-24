using Domain.Slots;

namespace Application.Abstractions.Repositories;

public interface ISlotRepository
{
    Task AddAsync(Slot slot, CancellationToken cancellationToken);
    Task DeleteAsync(Slot slot, CancellationToken cancellationToken);
    Task DeleteSlotsAsync(List<Slot> slots, CancellationToken cancellationToken);
    Task UpdateAsync(Slot slot, CancellationToken cancellationToken);
    Task<Slot?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Slot>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Slot>> GetScheduleSlotsAsync(Guid scheduleId, CancellationToken cancellationToken);
}
