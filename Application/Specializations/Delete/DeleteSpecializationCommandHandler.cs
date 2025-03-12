using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specializations;

namespace Application.Specializations.Delete;

public sealed class DeleteSpecializationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteSpecializationCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSpecializationCommand command, CancellationToken cancellationToken)
    {
        Specialization? specialization = await context.Specializations.FindAsync(command.Id, cancellationToken);
        if (specialization is null)
        {
            return Result.Failure<string>(SpecializationErrors.NotFoundSpecialziation);
        }
        context.Specializations.Remove(specialization);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Specialization deleted.");
    }
}
