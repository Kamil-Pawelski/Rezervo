using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;

namespace Application.Specializations.Create;

public sealed class CreateSpecializationCommandHandler(ISpecializationRepository specializationRepository) : ICommandHandler<CreateSpecializationCommand, string>
{
    public async Task<Result<string>> Handle(CreateSpecializationCommand command, CancellationToken cancellationToken)
    {
        var specialization = new Domain.Specializations.Specialization
        {
            Name = command.Name
        };

        await specializationRepository.AddAsync(specialization, cancellationToken);

        return Result.Success("Specialization created.");
    }
}
