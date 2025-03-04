using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.Put;

public sealed class PutScheduleCommandHandler(IApplicationDbContext context) : ICommandHandler<PutScheduleCommand, string>
{
    public async Task<Result<string>> Handle(PutScheduleCommand command, CancellationToken cancellationToken)
    {

        if (command.EndTime < command.StartTime)
        {
            return Result.Failure<string>(
                new Error("InvalidTimeRange", "EndTime must be later than StartTime.", ErrorType.Validation));
        }

        Schedule? schedule = await context.Schedules.FindAsync(command.Id, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<string>(new Error("NotFoundSchedule", "Schedule with the given id does not exist", ErrorType.NotFound));
        }

        List<Slot> invalidSlots = await context.Slots
            .Where(slot => slot.ScheduleId == schedule.Id && (slot.StartTime < command.StartTime || slot.StartTime > command.EndTime))
            .ToListAsync(cancellationToken);
        

        context.Slots.RemoveRange(invalidSlots);

        schedule.StartTime = command.StartTime;
        schedule.EndTime = command.EndTime;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success($"Schedule updated successfully. New work time {schedule.StartTime}-{schedule.EndTime}");
    }
}
