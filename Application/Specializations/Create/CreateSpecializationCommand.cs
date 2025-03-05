using Application.Abstractions.Messaging;

namespace Application.Specializations.Create;

public sealed record CreateSpecializationCommand(string Name) : ICommand<string>;

