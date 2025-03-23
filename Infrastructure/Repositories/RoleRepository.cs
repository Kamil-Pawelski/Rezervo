using Application.Abstractions.Repositories;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await context.Roles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken);
}
