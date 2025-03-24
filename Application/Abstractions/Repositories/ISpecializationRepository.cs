using Domain.Specializations;

namespace Application.Abstractions.Repositories;

public interface ISpecializationRepository
{
    Task AddAsync(Specialization specialization, CancellationToken cancellationToken);
    Task<Specialization?> GetByIdAsync(Guid id ,CancellationToken cancellationToken);
    Task DeleteAsync(Specialization specialization, CancellationToken cancellationToken);
    Task<List<Specialization>> GetAllAsync(CancellationToken cancellationToken);
}
