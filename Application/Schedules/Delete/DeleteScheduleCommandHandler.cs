using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Schedules;

namespace Application.Schedules.Delete;

public sealed class DeleteScheduleCommandHandler(IScheduleRepository scheduleRepository, IUserContext userContext) : ICommandHandler<DeleteScheduleCommand, string>
{
    public async Task<Result<string>> Handle(DeleteScheduleCommand command, CancellationToken cancellationToken)
    {
        Schedule? result = await scheduleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (result is null)
        {
            return Result.Failure<string>(ScheduleErrors.NotFoundSchedule);
        }

        if (result.Specialist!.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        await scheduleRepository.DeleteAsync(result, cancellationToken);

        return Result.Success("Schedule deleted successfully.");
    }
}
