using Application.Abstractions.Messaging;
using Domain.Schedules;

namespace Application.Schedules.Create;

public sealed record CreateScheduleCommand(
    Guid SpecialistId,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDuration,
    DateOnly Day) : ICommand<string>;
