using Application.Abstractions.Messaging;

namespace Application.Specialists.Delete;

public sealed record DeleteSpecialistCommand(Guid Id) : ICommand<string>;
