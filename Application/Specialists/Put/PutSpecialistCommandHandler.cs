﻿using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Put;

public sealed class PutSpecialistCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<PutSpecialistCommand, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(PutSpecialistCommand command, CancellationToken cancellationToken)
    {
        Guid loggedUserId = userContext.UserId;

        if (loggedUserId != command.UserId)
        {
            return Result.Failure<SpecialistsResponse>(new Error("WrongUser",
                "You are not allowed to edit another user's data.", ErrorType.Forbidden));
        }

        Specialist? specialist = await context.Specialists
            .Include(s => s.Specialization)
            .Include(s => s.User)
            .FirstOrDefaultAsync(specialist => specialist.Id == command.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<SpecialistsResponse>(new Error("SpecialistNotFound", "Specialist with the given id does not exist", ErrorType.NotFound));
        }

        specialist.PhoneNumber = command.PhoneNumber;
        specialist.Description = command.Description;
        specialist.City = command.City;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(specialist.MapToSpecialistResponse());

    }
}
