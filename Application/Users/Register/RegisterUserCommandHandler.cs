using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Users;

namespace Application.Users.Register;

public sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUserRoleRepository userRoleRepository
    )
    : ICommandHandler<RegisterUserCommand, string>
{
    public async Task<Result<string>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.IsEmailTakenAsync(command.Email, cancellationToken))
        {
            return Result.Failure<string>(UserErrors.EmailTaken);
        }

        if (await userRepository.IsUsernameTakenAsync(command.Username, cancellationToken))
        {
            return Result.Failure<string>(UserErrors.UsernameTaken);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Username = command.Username,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = passwordHasher.Hash(command.Password)
        };

        Role? role = await roleRepository.GetByIdAsync(command.RoleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundRole);
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRoleRepository.AddAsync(userRole, cancellationToken);

        return Result.Success("User has been successfully created.");
    }
}
