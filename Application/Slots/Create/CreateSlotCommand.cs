using Application.Abstractions.Messaging;

namespace Application.Slots.Create;

public sealed record CreateSlotCommand(Guid ScheduleId, TimeOnly StartTime) : ICommand<string>;

