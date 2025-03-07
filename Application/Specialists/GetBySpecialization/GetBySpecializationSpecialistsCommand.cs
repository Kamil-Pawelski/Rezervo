using Application.Abstractions.Messaging;

namespace Application.Specialists.GetBySpecialization;

public sealed record GetBySpecializationSpecialistsCommand(Guid Id) : ICommand<List<SpecialistsResponse>>;

