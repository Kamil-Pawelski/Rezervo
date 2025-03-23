using Domain.Users;

namespace Application.Abstractions.Repositories;

public interface IUserRoleRepository
{
    Task<List<string>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(UserRole userRole, CancellationToken cancellationToken);
}
