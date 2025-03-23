using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Login;

public sealed class LoginUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IUserRepository userRepository
) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FindByEmailOrUsernameAsync(command.Login, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundUser);
        }

        bool isPasswordValid = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result.Failure<string>(UserErrors.InvalidPassword);
        }

        List<string> roles = await context.UserRoles
         .Where(ur => ur.UserId == user.Id)
         .Select(ur => ur.Role.Name)
         .ToListAsync(cancellationToken);

        string token = tokenProvider.Create(user, roles);

        return new Result<string>(token, Error.None);
    }
}
