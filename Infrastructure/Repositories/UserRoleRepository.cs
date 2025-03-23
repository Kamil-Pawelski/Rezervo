using Application.Abstractions.Repositories;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRoleRepository(ApplicationDbContext context) : IUserRoleRepository
{
    public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken)
    {
        await context.UserRoles.AddAsync(userRole, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public Task<List<string>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken) => 
        context.UserRoles
        .Where(ur => ur.UserId == userId)
        .Select(ur => ur.Role.Name)
        .ToListAsync(cancellationToken);
}
