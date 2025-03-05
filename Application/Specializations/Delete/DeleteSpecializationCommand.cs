using Application.Abstractions.Messaging;

namespace Application.Specializations.Delete;

public sealed record DeleteSpecializationCommand(Guid Id) : ICommand<string>;

