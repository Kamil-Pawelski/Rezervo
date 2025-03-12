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
        var scheduleData = await context.Schedules
        .Where(schedule => schedule.Id == command.Id)
        .Select(schedule => new
        {
            Schedule = schedule,
            SpecialistUserId = schedule.Specialist!.UserId
        })
        .FirstOrDefaultAsync(cancellationToken);

        if (scheduleData is null)
        {
            return Result.Failure<string>(ScheduleErrors.NotFoundSchedule);
        }

        if (scheduleData.SpecialistUserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        List<Slot> invalidSlots = await context.Slots
            .Where(slot => slot.ScheduleId == scheduleData.Schedule.Id &&
                           (slot.StartTime < command.StartTime || slot.StartTime > command.EndTime))
            .ToListAsync(cancellationToken);

        context.Slots.RemoveRange(invalidSlots);

        scheduleData.Schedule.StartTime = command.StartTime;
        scheduleData.Schedule.EndTime = command.EndTime;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success($"Schedule updated successfully. New work time {scheduleData.Schedule.StartTime}-{scheduleData.Schedule.EndTime}.");
    }
}
