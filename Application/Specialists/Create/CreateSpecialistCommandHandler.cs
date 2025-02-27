using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;

namespace Application.Specialists.Create;

public sealed class CreateSpecialistCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSpecialistCommand>
{
    public async Task<Result> Handle(CreateSpecialistCommand command, CancellationToken cancellationToken)
    {
        Guid specializationId = context.Specializations
            .Where(s => s.Name == command.SpecializationName)
            .Select(s => s.Id)
            .FirstOrDefault();

        var specialist = new Specialist
        {
            UserId = command.UserId,
            SpecializationId = specializationId,
            PhoneNumber = command.PhoneNumber,
            Description = command.Description,
            City = command.City
        };

        await context.Specialists.AddAsync(specialist, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
