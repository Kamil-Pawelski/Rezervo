using Application.Abstractions.Messaging;

namespace Application.Slots.Put;

public sealed record PutSlotCommand(Guid Id, TimeOnly StartTime) : ICommand<string>;

