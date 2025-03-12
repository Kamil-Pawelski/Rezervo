using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Put;

public sealed class PutSpecialistCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<PutSpecialistCommand, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(PutSpecialistCommand command, CancellationToken cancellationToken)
    {
        if (userContext.UserId != command.UserId)
        {
            return Result.Failure<SpecialistsResponse>(CommonErrors.Unauthorized);
        }

        Specialist? specialist = await context.Specialists
            .Include(s => s.User)
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<SpecialistsResponse>(SpecialistErrors.NotFoundSpecialist);
        }

        specialist.PhoneNumber = command.PhoneNumber;
        specialist.Description = command.Description;
        specialist.City = command.City;

        await context.SaveChangesAsync(cancellationToken);

        var specialistResponse = new SpecialistsResponse
        {
            Id = specialist.Id,
            User = new UserDto(specialist.User!.Id, specialist.User.FirstName, specialist.User.LastName),
            Specialization = new SpecializationDto(specialist.Specialization!.Id, specialist.Specialization.Name),
            PhoneNumber = specialist.PhoneNumber,
            Description = specialist.Description,
            City = specialist.City
        };

        return Result.Success(specialistResponse);
    }
}
