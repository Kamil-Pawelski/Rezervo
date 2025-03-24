using Domain.Schedules;
using Domain.Slots;

namespace Application.Abstractions.Repositories;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule, CancellationToken cancellationToken);
    Task DeleteAsync(Schedule schedule, CancellationToken cancellationToken);
    Task UpdateAsync(Schedule schedule, CancellationToken cancellationToken);
    Task<List<Schedule>> GetAllAsync(DateOnly today, CancellationToken cancellationToken);
    Task<List<Schedule>> GetBySpecialistAsync(Guid specialistId, DateOnly today, CancellationToken cancellationToken);
    Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
