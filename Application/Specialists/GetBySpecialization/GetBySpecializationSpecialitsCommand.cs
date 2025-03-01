using Application.Abstractions.Messaging;

namespace Application.Specialists.GetBySpecialization;

public sealed record GetBySpecializationSpecialitsCommand(Guid Id) : ICommand<List<SpecialistsResponse>>;

