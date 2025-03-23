using System.Threading;
using Application.Abstractions.Repositories;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext context) : IUserRepository{

    public async Task AddAsync(User user, CancellationToken cancellationToken) 
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<User?> FindByEmailOrUsernameAsync(string login, CancellationToken cancellationToken) 
        => await context.Users.FirstOrDefaultAsync(u => u.Email == login || u.Username == login, cancellationToken);
    public async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken) 
        => await context.Users.AnyAsync(user => user.Email == email, cancellationToken);
    public async Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken) 
        => await context.Users.AnyAsync(user => user.Username == username, cancellationToken);
}
