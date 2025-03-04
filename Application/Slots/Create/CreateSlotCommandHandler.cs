using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;

namespace Application.Slots.Create;

public sealed class CreateSlotCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<CreateSlotCommand, string>
{
    public async Task<Result<string>> Handle(CreateSlotCommand command, CancellationToken cancellationToken)
    {
        Schedule? schedule = await context.Schedules
            .Include(schedule => schedule.Slots)
            .FirstOrDefaultAsync(schedule => schedule.Id == command.ScheduleId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<string>(new Error("NotFoundSchedule", "Schedule with the given id does not exist", ErrorType.NotFound));
        }

        if(schedule.Specialist!.UserId != userContext.UserId)
        {
            return Result.Failure<string>(new Error("Unauthorized", "You are not authorized to create slots for this schedule", ErrorType.Unauthorized));
        }

        if (schedule.StartTime > command.StartTime || schedule.EndTime < command.StartTime)
        {
            return Result.Failure<string>(new Error("InvalidTimeRange", "EndTime must be later than StartTime.", ErrorType.Validation));
                
        }

        if (schedule.Slots.Any(slot => slot.StartTime == command.StartTime))
        {
            return Result.Failure<string>(new Error("SlotAlreadyExists", "Slot with the given time already exists", ErrorType.Validation));
        }

        var slot = new Slot
        {
            ScheduleId = command.ScheduleId,
            StartTime = command.StartTime,
            Status = Status.Available
        };

        await context.Slots.AddAsync(slot, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Slot created successfully");
    }
}
