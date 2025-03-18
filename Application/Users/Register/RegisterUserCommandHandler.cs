using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Register;

public sealed class RegisterUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterUserCommand, string>
{
    public async Task<Result<string>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(user => user.Email == command.Email, cancellationToken))
        {
            return Result.Failure<string>(UserErrors.EmailTaken);
        }

        if (await context.Users.AnyAsync(user => user.Username == command.Username, cancellationToken))
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

        Role? role = await context.Roles.FirstOrDefaultAsync(role => role.Id == command.RoleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundRole);
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.UserRoles.AddAsync(userRole, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("User has been successfully created.");
   }
}
