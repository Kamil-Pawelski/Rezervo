using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Schedules.Put;

public sealed class PutScheduleCommandHandler(IScheduleRepository scheduleRepository, ISlotRepository slotRepository, IUserContext userContext) : ICommandHandler<PutScheduleCommand, string>
{
    public async Task<Result<string>> Handle(PutScheduleCommand command, CancellationToken cancellationToken)
    {
        Schedule? schedule = await scheduleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<string>(ScheduleErrors.NotFoundSchedule);
        }

        if (schedule.Specialist!.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        List<Slot> scheduleSlots = await slotRepository.GetScheduleSlotsAsync(schedule.Id, cancellationToken);

        var slotsToRemove = scheduleSlots
          .Where(slot => slot.StartTime < command.StartTime || slot.StartTime > command.EndTime)
          .ToList();

        
        await slotRepository.DeleteSlotsAsync(slotsToRemove, cancellationToken);

        schedule.StartTime = command.StartTime;
        schedule.EndTime = command.EndTime;
        await scheduleRepository.UpdateAsync(schedule, cancellationToken);

        return Result.Success($"Schedule updated successfully. New work time {schedule.StartTime}-{schedule.EndTime}.");
    }
}
