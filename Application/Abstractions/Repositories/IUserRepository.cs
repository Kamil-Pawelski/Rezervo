using Domain.Users;

namespace Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailOrUsernameAsync(string login, CancellationToken cancellationToken);
    Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}
