using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Specialists;
using Domain.Specializations;

namespace Application.Specialists.Create;

public sealed class CreateSpecialistCommandHandler(ISpecialistRepository specialistRepository, ISpecializationRepository specializationRepository) : ICommandHandler<CreateSpecialistCommand>
{
    public async Task<Result> Handle(CreateSpecialistCommand command, CancellationToken cancellationToken)
    {
        Specialization? specialization = await specializationRepository.GetByIdAsync(command.SpecializationId, cancellationToken);

        if (specialization is null)
        {
            return Result.Failure(SpecializationErrors.NotFoundSpecializiation);
        }
        
        var specialist = new Specialist
        {
            UserId = command.UserId,
            SpecializationId = specialization.Id,
            PhoneNumber = command.PhoneNumber,
            Description = command.Description,
            City = command.City
        };

        await specialistRepository.AddAsync(specialist, cancellationToken);

        return Result.Success();
    }
}
