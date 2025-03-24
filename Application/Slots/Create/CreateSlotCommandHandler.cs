using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;

namespace Application.Slots.Create;

public sealed class CreateSlotCommandHandler(ISlotRepository slotRepository, IScheduleRepository scheduleRepository, IUserContext userContext) : ICommandHandler<CreateSlotCommand, string>
{
    public async Task<Result<string>> Handle(CreateSlotCommand command, CancellationToken cancellationToken)
    {
        Schedule? schedule = await scheduleRepository.GetByIdAsync(command.ScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<string>(ScheduleErrors.NotFoundSchedule);
        }

        if(schedule.Specialist!.UserId != userContext.UserId)
        {
            return Result.Failure<string>(CommonErrors.Unauthorized);
        }

        if (schedule.Slots.Any(slot => slot.StartTime == command.StartTime))
        {
            return Result.Failure<string>(SlotErrors.SlotAlreadyExist(command.StartTime));
        }

        var slot = new Slot
        {
            ScheduleId = command.ScheduleId,
            StartTime = command.StartTime,
            Status = Status.Available
        };

        await slotRepository.AddAsync(slot, cancellationToken);

        return Result.Success("Slot created successfully.");
    }
}
