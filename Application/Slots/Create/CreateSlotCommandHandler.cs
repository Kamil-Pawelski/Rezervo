using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Slots.Create;

public sealed class CreateSlotCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<CreateSlotCommand, string>
{
    public async Task<Result<string>> Handle(CreateSlotCommand command, CancellationToken cancellationToken)
    {
        Schedule? schedule = await context.Schedules
            .Include(schedule => schedule.Slots)
            .Include(s => s.Specialist)
            .FirstOrDefaultAsync(schedule => schedule.Id == command.ScheduleId, cancellationToken);

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

        await context.Slots.AddAsync(slot, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Slot created successfully.");
    }
}
