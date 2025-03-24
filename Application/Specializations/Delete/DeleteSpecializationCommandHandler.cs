using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Specializations;

namespace Application.Specializations.Delete;

public sealed class DeleteSpecializationCommandHandler(ISpecializationRepository specializationRepository) : ICommandHandler<DeleteSpecializationCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSpecializationCommand command, CancellationToken cancellationToken)
    {
        Specialization? specialization = await specializationRepository.GetByIdAsync(command.Id, cancellationToken);

        if (specialization is null)
        {
            return Result.Failure<string>(SpecializationErrors.NotFoundSpecializiation);
        }

        await specializationRepository.DeleteAsync(specialization, cancellationToken);

        return Result.Success("Specialization deleted.");
    }
}
