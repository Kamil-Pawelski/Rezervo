using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;

namespace Application.Specialists.Delete;

public sealed class DeleteSpecialistCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteSpecialistCommand, string>
{
    public async Task<Result<string>> Handle(DeleteSpecialistCommand command, CancellationToken cancellationToken)
    {
        Specialist? specialist = dbContext.Specialists.FirstOrDefault(specialist => specialist.Id == command.Id);

        if (specialist is null)
        {
            return Result.Failure<string>(SpecialistErrors.NotFoundSpecialist);
        }

        dbContext.Specialists.Remove(specialist);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Result<string>("The specialist has been deleted successfully.", Error.None);
    }
}
