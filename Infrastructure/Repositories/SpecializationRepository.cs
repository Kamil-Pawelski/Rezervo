using Application.Abstractions.Repositories;
using Domain.Specializations;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class SpecializationRepository(ApplicationDbContext context) : ISpecializationRepository
{
    public async Task AddAsync(Specialization specialization, CancellationToken cancellationToken)
    {
        await context.Specializations.AddAsync(specialization, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Specialization specialization, CancellationToken cancellationToken) 
    {
        context.Specializations.Remove(specialization);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<List<Specialization>> GetAllAsync(CancellationToken cancellationToken) => await context.Specializations.ToListAsync(cancellationToken);
    public async Task<Specialization?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await context.Specializations.FirstOrDefaultAsync(specialzation => specialzation.Id == id, cancellationToken);
}
