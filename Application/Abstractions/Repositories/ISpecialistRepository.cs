using Domain.Specialists;

namespace Application.Abstractions.Repositories;

public interface ISpecialistRepository
{
    Task<Specialist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Specialist>> GetBySpecializationAsync(Guid specialziationId, CancellationToken cancellationToken);
    Task<List<Specialist>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Specialist specialist, CancellationToken cancellationToken);
    Task UpdateAsync(Specialist specialist, CancellationToken cancellationToken);
    Task DeleteAsync(Specialist specialist, CancellationToken cancellationToken);
}
