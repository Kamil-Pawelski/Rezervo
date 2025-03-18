using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;
using Domain.Specializations;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Create;

public sealed class CreateSpecialistCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSpecialistCommand>
{
    public async Task<Result> Handle(CreateSpecialistCommand command, CancellationToken cancellationToken)
    {
        Specialization? specialization = await context.Specializations
            .FirstOrDefaultAsync(specialization => specialization.Id == command.SpecializationId, cancellationToken);

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

        await context.Specialists.AddAsync(specialist, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
