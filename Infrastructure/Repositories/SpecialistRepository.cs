using Application.Abstractions.Repositories;
using Domain.Specialists;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class SpecialistRepository(ApplicationDbContext context) : ISpecialistRepository
{
    public async Task AddAsync(Specialist specialist, CancellationToken cancellationToken)
    {
        await context.Specialists.AddAsync(specialist, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public Task DeleteAsync(Specialist specialist, CancellationToken cancellationToken)
    {
        context.Specialists.Remove(specialist);
        return context.SaveChangesAsync(cancellationToken);
    }
    public Task<List<Specialist>> GetAllAsync(CancellationToken cancellationToken) => context.Specialists
        .Include(specialist => specialist.User)
        .Include(specialist => specialist.Specialization)
        .ToListAsync(cancellationToken);

    public Task<Specialist?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => context.Specialists
        .Include(specialist => specialist.User)
        .Include(specialist => specialist.Specialization)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<List<Specialist>> GetBySpecializationAsync(Guid specialziationId, CancellationToken cancellationToken) => context.Specialists
        .Include(specialist => specialist.User)
        .Include(specialist => specialist.Specialization)
        .Where(x => x.SpecializationId == specialziationId)
        .ToListAsync(cancellationToken);
    public Task UpdateAsync(Specialist specialist, CancellationToken cancellationToken)
    {
        context.Specialists.Update(specialist);
        return context.SaveChangesAsync(cancellationToken);
    }
}
