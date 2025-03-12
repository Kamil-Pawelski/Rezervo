using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Schedules;

namespace Application.Schedules.Create;

public sealed class CreateScheduleCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateScheduleCommand, string>
{
    public async Task<Result<string>> Handle(CreateScheduleCommand command, CancellationToken cancellationToken)
    {
        var schedule = new Schedule
        {
            Id = Guid.NewGuid(),
            SpecialistId = command.SpecialistId,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            Date = command.Day
        };

        schedule.GenerateSlots(command.SlotDuration);

        await context.Schedules.AddAsync(schedule, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success($"Created schedule with {schedule.Slots.Count} slots.");
    }
} 
