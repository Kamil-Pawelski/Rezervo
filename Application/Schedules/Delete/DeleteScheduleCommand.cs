using Application.Abstractions.Messaging;

namespace Application.Schedules.Delete;

public sealed record DeleteScheduleCommand(Guid Id) : ICommand<string>;

