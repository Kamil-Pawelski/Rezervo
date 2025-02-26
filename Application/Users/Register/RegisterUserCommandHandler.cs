using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Register;

public sealed class RegisterUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterUserCommand>
{
    public async Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(user => user.Email == command.Email, cancellationToken))
        {
            return Result.Failure(new Error("EmailTaken", "This email is already taken", ErrorType.Conflict));
        }

        if (await context.Users.AnyAsync(user => user.Username == command.Username, cancellationToken))
        {
            return Result.Failure(new Error("UsernameTaken", "This username is already taken", ErrorType.Conflict));
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

        context.Users.Add(user);

        Guid roleId = (await context.Roles.FirstAsync(ur => ur.Name == command.Role, cancellationToken: cancellationToken)).Id;

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = roleId
        };

        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
   }
}
