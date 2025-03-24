using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Specialists;

namespace Application.Specialists.Delete;

public sealed class DeleteSpecialistCommandHandler(ISpecialistRepository specialistRepository) : ICommandHandler<DeleteSpecialistCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSpecialistCommand command, CancellationToken cancellationToken)
    {
        Specialist? specialist = await specialistRepository.GetByIdAsync(command.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<string>(SpecialistErrors.NotFoundSpecialist);
        }

        await specialistRepository.DeleteAsync(specialist, cancellationToken);

        return Result.Success("The specialist has been deleted successfully.");
    }
}
