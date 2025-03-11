using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.Put;

public sealed class PutScheduleCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<PutScheduleCommand, string>
{
    public async Task<Result<string>> Handle(PutScheduleCommand command, CancellationToken cancellationToken)
    {

        if (command.EndTime < command.StartTime)
        {
            return Result.Failure<string>(
                new Error("InvalidTimeRange", "EndTime must be later than StartTime.", ErrorType.Validation));
        }

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

        List<Slot> invalidSlots = await context.Slots
            .Where(slot => slot.ScheduleId == result.Id && (slot.StartTime < command.StartTime || slot.StartTime > command.EndTime))
            .ToListAsync(cancellationToken);
        

        context.Slots.RemoveRange(invalidSlots);

        result.StartTime = command.StartTime;
        result.EndTime = command.EndTime;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success($"Schedule updated successfully. New work time {result.StartTime}-{result.EndTime}");
    }
}
