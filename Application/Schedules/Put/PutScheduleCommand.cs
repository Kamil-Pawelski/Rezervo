using Application.Abstractions.Messaging;

namespace Application.Schedules.Put;

public sealed record PutScheduleCommand(Guid Id, TimeOnly StartTime, TimeOnly EndTime) : ICommand<string>;

