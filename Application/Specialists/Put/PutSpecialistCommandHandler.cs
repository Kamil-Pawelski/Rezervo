using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;
namespace Application.Specialists.Put;

public sealed class PutSpecialistCommandHandler(ISpecialistRepository specialistRepository, IUserContext userContext) : ICommandHandler<PutSpecialistCommand, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(PutSpecialistCommand command, CancellationToken cancellationToken)
    {
        if (userContext.UserId != command.UserId)
        {
            return Result.Failure<SpecialistsResponse>(CommonErrors.Unauthorized);
        }

        Specialist? specialist = await specialistRepository.GetByIdAsync(command.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<SpecialistsResponse>(SpecialistErrors.NotFoundSpecialist);
        }

        specialist.PhoneNumber = command.PhoneNumber;
        specialist.Description = command.Description;
        specialist.City = command.City;

        await specialistRepository.UpdateAsync(specialist, cancellationToken);

        return Result.Success(specialist.MapToSpecialistResponse());
    }
}
