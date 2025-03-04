using Application.Abstractions.Messaging;

namespace Application.Slots.Delete;

public sealed record DeleteSlotCommand(Guid Id) : ICommand<string>;

