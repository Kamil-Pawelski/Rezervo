using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.Delete;

public sealed class DeleteScheduleCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteScheduleCommand, string>
{
    public async Task<Result<string>> Handle(DeleteScheduleCommand command, CancellationToken cancellationToken)
    {
        Schedule? result = await context.Schedules
            .Include(schedule => schedule.Specialist)
            .FirstOrDefaultAsync(schedule => schedule.Id == command.Id, cancellationToken);

        if (result is null)
        {
            return Result.Failure<string>(new Error("NotFoundSchedule", "Schedule with the given id does not exist", ErrorType.NotFound));
        }

        if (result.Specialist!.UserId != userContext.UserId)
        {
            return Result.Failure<string>(new Error("Unauthorized", "You are not authorized to delete this schedule", ErrorType.Unauthorized));
        }

        context.Schedules.Remove(result);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Schedule deleted successfully");
    }
}
