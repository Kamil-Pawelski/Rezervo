using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;

namespace Application.Specializations.Create;

public sealed class CreateSpecializationCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSpecializationCommand, string>
{
    public async Task<Result<string>> Handle(CreateSpecializationCommand command, CancellationToken cancellationToken)
    {
        var specialization = new Domain.Specializations.Specialization
        {
            Name = command.Name
        };

        await context.Specializations.AddAsync(specialization, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Specialization created.");
    }
}
