using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Users;

namespace Application.Users.Login;

public sealed class LoginUserCommandHandler(
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository
) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailOrUsernameAsync(command.Login, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundUser);
        }

        bool isPasswordValid = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result.Failure<string>(UserErrors.InvalidPassword);
        }

        List<string> roles = await userRoleRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

        string token = tokenProvider.Create(user, roles);

        return new Result<string>(token, Error.None);
    }
}
