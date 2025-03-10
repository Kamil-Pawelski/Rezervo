﻿using Application.Abstractions.Authentication;
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

        Role? role = await context.Roles.FirstOrDefaultAsync(ur => ur.Id == command.RoleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(new Error("RoleNotFound", "Role with the given id does not exist", ErrorType.NotFound));
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await context.UserRoles.AddAsync(userRole, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
   }
}
